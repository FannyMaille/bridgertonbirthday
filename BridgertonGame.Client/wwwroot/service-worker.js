// Service Worker pour les notifications push
const CACHE_NAME = 'bridgerton-v2'; // Incrémenter la version pour forcer le rafraîchissement
const urlsToCache = [
    '/',
    '/images/LadyWithldown.png',
    '/images/FleursBG.png',
    '/manifest.json'
    // Retirer les CSS du cache pour toujours les recharger
];

// Installation du Service Worker
self.addEventListener('install', event => {
    console.log('[Service Worker] Installation...');
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                console.log('[Service Worker] Cache ouvert');
                return cache.addAll(urlsToCache);
            })
            .then(() => self.skipWaiting()) // Activer immédiatement
    );
});

// Activation du Service Worker
self.addEventListener('activate', event => {
    console.log('[Service Worker] Activation...');
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cacheName => {
                    if (cacheName !== CACHE_NAME) {
                        console.log('[Service Worker] Suppression ancien cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        }).then(() => self.clients.claim()) // Prendre le contrôle immédiatement
    );
});

// Interception des requêtes réseau
self.addEventListener('fetch', event => {
    const url = new URL(event.request.url);
    
    // Pour les fichiers CSS, toujours aller sur le réseau (network first)
    if (url.pathname.endsWith('.css')) {
        event.respondWith(
            fetch(event.request)
                .then(response => {
                    // Mettre à jour le cache avec la nouvelle version
                    const responseClone = response.clone();
                    caches.open(CACHE_NAME).then(cache => {
                        cache.put(event.request, responseClone);
                    });
                    return response;
                })
                .catch(() => caches.match(event.request)) // Fallback sur le cache si réseau indisponible
        );
    } 
    // Pour les autres fichiers, utiliser le cache first
    else {
        event.respondWith(
            caches.match(event.request)
                .then(response => {
                    return response || fetch(event.request);
                })
        );
    }
});

// Gestion des notifications push
self.addEventListener('push', event => {
    console.log('[Service Worker] Push reçu:', event);
    
    let data = {
        title: '📰 Nouvelle Chronique',
        body: 'Lady Whistledown a publié une nouvelle chronique !',
        icon: '/images/LadyWithldown.png',
        badge: '/images/LadyWithldown.png',
        tag: 'bridgerton-notification',
        requireInteraction: false
    };

    if (event.data) {
        try {
            const pushData = event.data.json();
            data = {
                title: pushData.title || data.title,
                body: pushData.message || data.body,
                icon: data.icon,
                badge: data.badge,
                tag: pushData.type || data.tag,
                requireInteraction: false,
                data: {
                    url: pushData.articleId ? '/' : '/',
                    articleId: pushData.articleId,
                    familyName: pushData.familyName
                }
            };
        } catch (e) {
            console.error('[Service Worker] Erreur parsing push data:', e);
        }
    }

    event.waitUntil(
        self.registration.showNotification(data.title, {
            body: data.body,
            icon: data.icon,
            badge: data.badge,
            tag: data.tag,
            requireInteraction: data.requireInteraction,
            data: data.data,
            vibrate: [200, 100, 200], // Vibration du téléphone
            actions: [
                {
                    action: 'open',
                    title: 'Voir',
                    icon: '/images/LadyWithldown.png'
                },
                {
                    action: 'close',
                    title: 'Fermer'
                }
            ]
        })
    );
});

// Gestion du clic sur la notification
self.addEventListener('notificationclick', event => {
    console.log('[Service Worker] Notification cliquée:', event);
    
    event.notification.close();

    if (event.action === 'close') {
        return;
    }

    // Ouvrir ou focus sur la page
    event.waitUntil(
        clients.matchAll({ type: 'window', includeUncontrolled: true })
            .then(clientList => {
                // Si une fenêtre est déjà ouverte, la focus
                for (let client of clientList) {
                    if (client.url === '/' && 'focus' in client) {
                        return client.focus();
                    }
                }
                // Sinon, ouvrir une nouvelle fenêtre
                if (clients.openWindow) {
                    const url = event.notification.data?.url || '/';
                    return clients.openWindow(url);
                }
            })
    );
});
