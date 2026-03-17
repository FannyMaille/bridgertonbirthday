// Push Notifications Helper
window.pushNotifications = {
    // Vérifier si les notifications sont supportées
    isSupported: function() {
        return 'Notification' in window && 'serviceWorker' in navigator && 'PushManager' in window;
    },

    // Demander la permission
    requestPermission: async function() {
        if (!this.isSupported()) {
            return 'denied';
        }

        try {
            const permission = await Notification.requestPermission();
            console.log('Permission notifications:', permission);
            return permission;
        } catch (error) {
            console.error('Erreur permission:', error);
            return 'denied';
        }
    },

    // S'abonner aux notifications push
    subscribe: async function() {
        if (!this.isSupported()) {
            return null;
        }

        try {
            const registration = await navigator.serviceWorker.ready;
            
            // Clé publique VAPID (vous devrez la générer)
            // Pour l'instant, on utilise juste les notifications locales
            const subscription = await registration.pushManager.getSubscription();
            
            if (subscription) {
                console.log('Déjà abonné:', subscription);
                return JSON.stringify(subscription);
            }

            // Pour l'instant, on retourne juste un token fictif
            // En production, vous devrez générer une vraie clé VAPID
            console.log('Abonnement réussi (mode local)');
            return JSON.stringify({ endpoint: 'local', keys: {} });
        } catch (error) {
            console.error('Erreur souscription:', error);
            return null;
        }
    },

    // Se désabonner
    unsubscribe: async function() {
        try {
            const registration = await navigator.serviceWorker.ready;
            const subscription = await registration.pushManager.getSubscription();
            
            if (subscription) {
                await subscription.unsubscribe();
                console.log('Désabonné avec succès');
                return true;
            }
            return false;
        } catch (error) {
            console.error('Erreur désabonnement:', error);
            return false;
        }
    },

    // Afficher une notification locale
    showNotification: async function(title, body, icon) {
        if (!this.isSupported()) {
            console.warn('Notifications non supportées');
            return;
        }

        const permission = await Notification.requestPermission();
        if (permission !== 'granted') {
            console.warn('Permission refusée');
            return;
        }

        try {
            const registration = await navigator.serviceWorker.ready;
            
            await registration.showNotification(title, {
                body: body,
                icon: icon || '/images/LadyWithldown.png',
                badge: '/images/LadyWithldown.png',
                tag: 'bridgerton-notification',
                requireInteraction: false,
                vibrate: [200, 100, 200],
                data: {
                    url: '/',
                    dateOfArrival: Date.now()
                },
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
            });

            console.log('Notification affichée:', title);
        } catch (error) {
            console.error('Erreur affichage notification:', error);
            
            // Fallback: notification navigateur simple
            if (Notification.permission === 'granted') {
                new Notification(title, {
                    body: body,
                    icon: icon || '/images/LadyWithldown.png'
                });
            }
        }
    }
};

// Initialiser au chargement
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function() {
        console.log('Push Notifications Helper chargé');
    });
} else {
    console.log('Push Notifications Helper chargé');
}
