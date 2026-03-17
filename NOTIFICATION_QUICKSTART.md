# 🚀 Quickstart - Système de Notifications

## Installation rapide

Le système de notifications est **déjà installé et configuré** ! Rien à faire de plus.

## Utilisation

### Pour les utilisateurs

1. **Voir les notifications**
   - Cliquez sur l'icône 🔔 en haut à droite
   - Le badge rouge indique le nombre de notifications non lues

2. **Lire une notification**
   - Cliquez sur la notification pour la marquer comme lue
   - Pour les notifications d'articles, vous serez redirigé vers la page d'accueil

3. **Gérer les notifications**
   - ✓ : Marquer toutes comme lues
   - 🗑️ : Effacer toutes les notifications
   - ✕ : Fermer une notification individuelle

### Pour les développeurs

#### Envoyer une notification personnalisée

**Dans n'importe quel contrôleur :**

```csharp
using Microsoft.AspNetCore.SignalR;
using BridgertonGame.Server.Hubs;

public class MonController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public MonController(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task MaMethode()
    {
        // Envoyer une notification à tous les clients
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", 
            "🎉 Titre",              // Titre
            "Mon message",           // Message
            "success",               // Type: article, success, warning, info
            null,                    // ID optionnel (ex: article ID)
            null);                   // Info optionnelle (ex: nom famille)
    }
}
```

## Test rapide

1. **Ouvrir deux navigateurs**
   - Navigateur 1 : Chrome
   - Navigateur 2 : Firefox (ou Chrome en mode incognito)

2. **Se connecter**
   - Sur chaque navigateur, se connecter avec un compte différent

3. **Publier un article**
   - Sur le navigateur 1 : Aller dans "Mon Espace"
   - Se connecter comme Lady Whistledown
   - Publier un article

4. **Vérifier la notification**
   - Sur le navigateur 2 : Observer la cloche 🔔
   - Un badge rouge devrait apparaître
   - Cliquer pour voir la notification

✅ **Ça fonctionne !** Les notifications sont envoyées en temps réel à tous les utilisateurs connectés.

## Types de notifications disponibles

| Type | Icône | Utilisation |
|------|-------|-------------|
| `article` | 📰 | Nouvelle chronique publiée |
| `success` | ✅ | Opération réussie |
| `warning` | ⚠️ | Avertissement |
| `info` | ℹ️ | Information générale |

## Exemples d'utilisation avancée

### Notification pour un nouveau vote

```csharp
await _hubContext.Clients.All.SendAsync("ReceiveNotification", 
    "🗳️ Nouveau vote",
    "Un membre de votre famille a voté !",
    "info",
    null,
    familyName);
```

### Notification de changement de classement

```csharp
await _hubContext.Clients.All.SendAsync("ReceiveNotification", 
    "🏆 Classement mis à jour",
    $"La famille {familyName} prend la tête !",
    "success",
    null,
    familyName);
```

### Notification d'avertissement

```csharp
await _hubContext.Clients.All.SendAsync("ReceiveNotification", 
    "⚠️ Attention",
    "Le temps de cooldown approche de sa fin",
    "warning",
    null,
    null);
```

## Personnalisation rapide

### Changer la couleur du badge

Dans `app.css` :
```css
.notification-badge {
    background: linear-gradient(135deg, #4CAF50 0%, #45a049 100%); /* Vert */
}
```

### Changer le nombre max de notifications

Dans `NotificationService.cs` :
```csharp
// Garder seulement les 50 dernières notifications
if (_notifications.Count > 50)
{
    _notifications.RemoveAt(_notifications.Count - 1);
}
```

### Ajouter un son

Dans `NotificationBell.razor` :
```csharp
private void HandleNewNotification(Notification notification)
{
    InvokeAsync(async () =>
    {
        RefreshNotifications();
        
        // Jouer un son
        await JS.InvokeVoidAsync("playNotificationSound");
        
        StateHasChanged();
    });
}
```

Puis dans `index.html` :
```html
<script>
    window.playNotificationSound = function() {
        const audio = new Audio('/sounds/notification.mp3');
        audio.play();
    };
</script>
```

## Dépannage express

❌ **Notifications ne s'affichent pas ?**
- Vérifiez la console du navigateur (F12)
- Recherchez des erreurs SignalR

❌ **Badge ne disparaît pas ?**
- Rechargez la page (Ctrl+R)
- Videz le cache (Ctrl+Shift+R)

❌ **Connexion SignalR échoue ?**
- Vérifiez que le serveur est démarré
- Vérifiez les CORS dans `Program.cs`

## C'est tout !

Le système est prêt à l'emploi. Les notifications sont automatiquement envoyées lors de la publication d'articles.

Pour plus de détails, consultez `NOTIFICATION_SYSTEM.md`.
