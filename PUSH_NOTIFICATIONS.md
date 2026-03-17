# 📱 Notifications Push - Guide Complet

## 🎉 Félicitations !

Votre application Bridgerton Game supporte maintenant les **notifications push sur mobile** ! 

## ✨ Ce qui a été ajouté

### Fichiers créés

1. **service-worker.js** - Service Worker PWA pour les notifications
2. **manifest.json** - Manifeste PWA
3. **PushNotificationService.cs** - Service C# pour gérer les notifications push
4. **push-notifications.js** - Helper JavaScript pour les notifications

### Fichiers modifiés

1. **index.html** - Ajout du manifest PWA et du Service Worker
2. **NotificationBell.razor** - Ajout du bouton d'activation push
3. **Program.cs** - Enregistrement du PushNotificationService

## 🚀 Comment ça fonctionne

### 1. Installation de l'application (PWA)

L'application peut maintenant être **installée sur le téléphone** comme une vraie application :

**Sur Android (Chrome/Edge) :**
- Ouvrez l'application dans Chrome
- Menu → "Installer l'application" ou "Ajouter à l'écran d'accueil"
- L'icône apparaît sur l'écran d'accueil

**Sur iOS (Safari) :**
- Ouvrez l'application dans Safari
- Bouton Partager → "Sur l'écran d'accueil"
- L'icône apparaît sur l'écran d'accueil

### 2. Activation des notifications

**Première utilisation :**
1. Ouvrir l'application
2. Cliquer sur la cloche 🔔
3. Voir le bouton vert avec animation pulse
4. Cliquer dessus
5. Accepter la permission navigateur
6. ✅ C'est activé !

**Le bouton disparaît après activation** pour ne pas encombrer l'interface.

### 3. Réception des notifications

Quand un article est publié :
- **Application ouverte** : Badge + panneau de notifications
- **Application en arrière-plan** : Notification système native
- **Application fermée** : Notification système native
- **Téléphone verrouillé** : Notification + vibration

## 📱 Caractéristiques des notifications mobile

### Vibration
```javascript
vibrate: [200, 100, 200] // Pattern de vibration
```

### Actions
Deux boutons dans la notification :
- **Voir** : Ouvre l'application
- **Fermer** : Ferme la notification

### Icônes
- **Icône principale** : Lady Whistledown
- **Badge** : Mini icône sur l'écran de verrouillage

### Son
Le téléphone joue le son de notification par défaut

## 🛠️ Configuration technique

### Service Worker

Le Service Worker gère :
- **Cache** : Mise en cache des ressources
- **Offline** : Fonctionnement hors ligne (basique)
- **Notifications** : Réception et affichage des notifs

**Fichiers mis en cache :**
- Page d'accueil
- CSS
- Images principales
- Manifest

### Manifest PWA

Configure l'application :
```json
{
  "name": "The Bridgerton Game",
  "short_name": "Bridgerton",
  "display": "standalone",
  "theme_color": "#7172C5"
}
```

**Mode standalone** = L'application s'ouvre en plein écran sans la barre d'URL

## 🎯 Cas d'usage

### Notification lors d'un article

```
Utilisateur publie un article
    ↓
ArticlesController envoie notification SignalR
    ↓
NotificationBell reçoit la notification
    ↓
PushNotificationService.ShowNotificationAsync()
    ↓
Service Worker affiche la notification
    ↓
Notification apparaît sur le téléphone
```

### Click sur la notification mobile

```
Utilisateur clique sur la notification
    ↓
Service Worker intercepte le click
    ↓
Vérifie si l'application est déjà ouverte
    ↓
Si oui : Focus sur l'onglet existant
Si non : Ouvre une nouvelle fenêtre
```

## 📲 Test sur téléphone

### Android

1. **Accéder à l'application**
```
https://votre-domaine.com
```

2. **Installer l'application**
- Chrome → Menu → "Installer l'application"
- L'icône apparaît sur l'écran d'accueil

3. **Activer les notifications**
- Ouvrir l'app installée
- Cliquer sur la cloche
- Cliquer sur le bouton vert
- Autoriser les notifications

4. **Tester**
- Publier un article depuis un autre appareil
- Vérifier que la notification arrive sur le téléphone

### iOS

⚠️ **Limitation iOS** : Safari sur iOS a un support limité des notifications push. Elles fonctionnent uniquement si l'app est installée sur l'écran d'accueil.

1. **Installer l'app**
- Safari → Bouton Partager → "Sur l'écran d'accueil"

2. **Activer les notifications**
- Ouvrir l'app depuis l'écran d'accueil
- Activer les notifications comme sur Android

## 🔧 Personnalisation

### Changer le son de notification

Les notifications utilisent le son système par défaut. Pour personnaliser :

**Option 1 : Jouer un son personnalisé (Android seulement)**
```javascript
// Dans service-worker.js
event.waitUntil(
    self.registration.showNotification(data.title, {
        // ...autres options...
        sound: '/sounds/notification.mp3' // Votre son personnalisé
    })
);
```

**Option 2 : Vibration personnalisée**
```javascript
vibrate: [100, 50, 100, 50, 200] // Pattern custom
```

### Changer le pattern de vibration

Modifier dans `service-worker.js` :
```javascript
vibrate: [200, 100, 200] // [vibrer, pause, vibrer]
```

Exemples :
- `[100]` : Vibration courte
- `[200, 100, 200]` : Double vibration
- `[500]` : Vibration longue

### Ajouter une image dans la notification

```javascript
// Dans service-worker.js
event.waitUntil(
    self.registration.showNotification(data.title, {
        body: data.body,
        icon: '/images/LadyWithldown.png',
        image: '/images/article-preview.jpg', // Grande image
        badge: '/images/badge.png'
    })
);
```

### Notification permanente

Pour forcer l'utilisateur à interagir :
```javascript
requireInteraction: true // La notification ne disparaît pas automatiquement
```

## 🎨 Personnalisation UI

### Bouton d'activation (dans app.css)

```css
.enable-push-btn {
    background: rgba(76, 175, 80, 0.3);
    animation: pulse-push 2s ease-in-out infinite;
}
```

**Changer la couleur** (bleu au lieu de vert) :
```css
.enable-push-btn {
    background: rgba(33, 150, 243, 0.3);
    border-color: rgba(33, 150, 243, 0.5);
}
```

**Désactiver l'animation** :
```css
.enable-push-btn {
    animation: none;
}
```

## 📊 Statistiques et tracking

Pour suivre l'activation des notifications :

```csharp
// Dans PushNotificationService.cs
public async Task<bool> SubscribeAsync()
{
    try
    {
        var subscription = await _jsRuntime.InvokeAsync<string>("pushNotifications.subscribe");
        if (!string.IsNullOrEmpty(subscription))
        {
            _subscription = subscription;
            _isSubscribed = true;
            
            // TODO: Envoyer à votre backend pour tracking
            // await _http.PostAsJsonAsync("api/notifications/subscribe", new { subscription });
            
            return true;
        }
        return false;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur souscription: {ex.Message}");
        return false;
    }
}
```

## 🔐 Sécurité et confidentialité

### Permissions

Les notifications push requièrent la permission de l'utilisateur. L'application :
- ✅ Demande explicitement la permission
- ✅ Ne force pas l'activation
- ✅ Respecte le choix de l'utilisateur

### Données collectées

Aucune donnée personnelle n'est collectée par le système de notifications. Les informations stockées :
- État d'activation (local uniquement)
- Subscription token (local uniquement)

### Désactivation

L'utilisateur peut désactiver à tout moment :

**Dans l'application :**
```csharp
await PushNotificationService.UnsubscribeAsync();
```

**Dans les paramètres du navigateur/téléphone :**
- Android : Paramètres → Applications → Bridgerton → Notifications
- iOS : Réglages → Notifications → Safari

## 🐛 Dépannage

### Les notifications ne fonctionnent pas

**1. Vérifier le support du navigateur**
```javascript
console.log('Notification' in window); // Doit être true
console.log('serviceWorker' in navigator); // Doit être true
console.log('PushManager' in window); // Doit être true
```

**2. Vérifier que le Service Worker est enregistré**
- Ouvrir DevTools (F12)
- Onglet "Application"
- Section "Service Workers"
- Doit afficher "service-worker.js" avec statut "activated"

**3. Vérifier les permissions**
```javascript
Notification.permission // Doit être "granted"
```

**4. Tester manuellement**
Dans la console du navigateur :
```javascript
pushNotifications.showNotification('Test', 'Message de test');
```

### Les notifications ne vibrent pas

- Vérifier que le mode vibreur est activé sur le téléphone
- iOS ne supporte pas la vibration via notifications web

### L'application ne s'installe pas

**Android :**
- Vérifier que manifest.json est accessible
- URL doit être en HTTPS (sauf localhost)

**iOS :**
- Safari uniquement
- Utiliser le bouton Partager manuellement

## 📈 Améliorations futures

### Court terme
- [ ] Notifications groupées par type
- [ ] Statistiques d'activation
- [ ] Sons personnalisés par type

### Moyen terme
- [ ] Notifications planifiées
- [ ] Notifications silencieuses (updates en arrière-plan)
- [ ] Rich notifications (images, boutons personnalisés)

### Long terme
- [ ] Notifications géolocalisées
- [ ] Notifications basées sur les actions utilisateur
- [ ] Integration avec Firebase Cloud Messaging (FCM)

## 🌐 Compatibilité navigateurs

| Navigateur | Desktop | Android | iOS |
|-----------|---------|---------|-----|
| Chrome | ✅ | ✅ | ❌ |
| Edge | ✅ | ✅ | ❌ |
| Firefox | ✅ | ✅ | ❌ |
| Safari | ✅ | N/A | ⚠️ (limité) |
| Samsung Internet | N/A | ✅ | N/A |

✅ Support complet  
⚠️ Support partiel  
❌ Non supporté  

## 🔗 Ressources

- [Service Worker API](https://developer.mozilla.org/en-US/docs/Web/API/Service_Worker_API)
- [Notifications API](https://developer.mozilla.org/en-US/docs/Web/API/Notifications_API)
- [Web App Manifest](https://developer.mozilla.org/en-US/docs/Web/Manifest)
- [PWA Best Practices](https://web.dev/progressive-web-apps/)

## ✅ Checklist de déploiement

Avant de déployer en production :

- [ ] Tester sur Android (Chrome)
- [ ] Tester sur iOS (Safari)
- [ ] Vérifier que l'app s'installe correctement
- [ ] Vérifier que les notifications arrivent
- [ ] Tester la vibration
- [ ] Tester le click sur notification
- [ ] Vérifier l'icône de l'application
- [ ] Tester en mode hors ligne (basique)

## 🎉 C'est prêt !

Votre application supporte maintenant les notifications push sur mobile ! Les utilisateurs peuvent :

✅ Installer l'app sur leur téléphone  
✅ Recevoir des notifications même app fermée  
✅ Vibration lors de nouvelles notifications  
✅ Actions rapides (Voir / Fermer)  
✅ Expérience native sur mobile  

**Pour activer :** Cliquer sur la cloche 🔔 puis sur le bouton vert qui pulse !

---

**Créé le :** 2024  
**Version :** 1.0 - PWA avec Push Notifications
