// Service Worker pour les notifications push
const CACHE_NAME = 'bridgerton-v1';
const urlsToCache = [
    '/',
    '/css/app.css',
    '/images/LadyWithldown.png',
    '/images/FleursBG.png',
    '/manifest.json'
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
        })
    );
});

// Interception des requêtes réseau
self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                // Cache first, puis réseau
                return response || fetch(event.request);
            })
    );
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
