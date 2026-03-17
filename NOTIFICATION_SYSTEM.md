# 🔔 Système de Notifications - Guide Complet

## Vue d'ensemble

Un système de notifications en temps réel a été implémenté pour avertir tous les utilisateurs lorsqu'un article est publié. Le système utilise **SignalR** pour les communications en temps réel.

## Architecture

### Backend (Server)

1. **NotificationHub.cs** (`BridgertonGame.Server/Hubs/`)
   - Hub SignalR qui gère les connexions et diffuse les notifications
   - Méthodes :
     - `SendNotification()` : Envoie une notification à tous les clients
     - `OnConnectedAsync()` : Gère les nouvelles connexions
     - `OnDisconnectedAsync()` : Gère les déconnexions

2. **ArticlesController.cs** (Modifié)
   - Lors de la publication d'un article, envoie une notification via le hub
   - Notification contient : titre, message, type, ID article, nom de famille

### Frontend (Client)

1. **NotificationService.cs** (`BridgertonGame.Client/Services/`)
   - Service qui gère la connexion SignalR
   - Stocke les notifications (max 20)
   - Événements :
     - `OnNotificationReceived` : Nouvelle notification reçue
     - `OnNotificationsChanged` : Liste de notifications modifiée
   - Méthodes :
     - `StartAsync()` : Démarre la connexion SignalR
     - `MarkAsRead()` : Marquer comme lu
     - `ClearNotification()` : Supprimer une notification
     - `GetUnreadCount()` : Nombre de notifications non lues

2. **NotificationBell.razor** (`BridgertonGame.Client/Shared/`)
   - Composant UI qui affiche l'icône de cloche avec badge
   - Panneau déroulant pour voir toutes les notifications
   - Fonctionnalités :
     - Badge avec nombre de notifications non lues
     - Animation pulse sur le badge
     - Click sur notification d'article → navigation vers Home
     - Actions : Marquer tout comme lu, Tout effacer, Fermer une notification

3. **Header.razor** (Modifié)
   - Intègre le composant `NotificationBell`
   - Positionnement à côté du menu burger

### Modèles

**Notification.cs** (`BridgertonGame.Shared/Models/`)
```csharp
public class Notification
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; } // "info", "success", "warning", "article"
    public DateTime CreatedAt { get; set; }
    public string? ArticleId { get; set; }
    public string? FamilyName { get; set; }
    public bool IsRead { get; set; }
}
```

## Flux de fonctionnement

### 1. Publication d'un article

```
Utilisateur clique "Publier"
    ↓
ArticlesController.Publish()
    ↓
Sauvegarde l'article en DB
    ↓
Envoie notification via NotificationHub
    ↓
Tous les clients connectés reçoivent la notification
    ↓
NotificationService traite la notification
    ↓
NotificationBell met à jour l'UI
    ↓
Badge apparaît avec le nombre de notifications
```

### 2. Consultation des notifications

```
Utilisateur clique sur la cloche
    ↓
Panneau s'ouvre avec la liste
    ↓
Toutes les notifications sont marquées comme lues
    ↓
Badge disparaît
    ↓
Click sur une notification d'article → Navigation vers Home
```

## Personnalisation

### Types de notifications

Le système supporte plusieurs types :
- **article** : Nouvelle chronique publiée (icône 📰)
- **success** : Succès (icône ✅)
- **warning** : Avertissement (icône ⚠️)
- **info** : Information (icône ℹ️)

### Ajouter un nouveau type de notification

**Côté Server :**
```csharp
await _hubContext.Clients.All.SendAsync("ReceiveNotification", 
    "Titre", 
    "Message", 
    "type",  // Nouveau type
    null,    // ID optionnel
    null);   // Info optionnelle
```

**Côté Client :**
Modifier `GetNotificationIcon()` dans `NotificationBell.razor` :
```csharp
private string GetNotificationIcon(string type) => type switch
{
    "article" => "📰",
    "nouveauType" => "🎉",
    // ... autres types
};
```

## Styling

### Classes CSS principales

- `.notification-bell` : Icône de cloche dans le header
- `.notification-badge` : Badge rouge avec le nombre
- `.notification-panel` : Panneau déroulant
- `.notification-item` : Une notification individuelle
- `.notification-item.unread` : Notification non lue
- `.notification-article` : Style spécifique pour les articles

### Personnalisation des couleurs

Dans `app.css` :
```css
.notification-badge {
    background: linear-gradient(135deg, #f44336 0%, #e53935 100%);
}

.notification-item.unread {
    border-left-color: #7172C5; /* Couleur de la bordure */
}
```

## Responsive Design

Le système est entièrement responsive :

- **Desktop** : Panneau de 380px à droite
- **Tablet** : Panneau adapté avec marges réduites
- **Mobile** : Panneau pleine largeur, ajustement des tailles

## Configuration SignalR

### Connection String
Dans `Program.cs` (Client) :
```csharp
_hubConnection = new HubConnectionBuilder()
    .WithUrl(navigationManager.ToAbsoluteUri("/notificationHub"))
    .WithAutomaticReconnect() // Reconnexion automatique
    .Build();
```

### CORS
Dans `Program.cs` (Server) :
```csharp
options.AddPolicy("AllowBlazorClient",
    policy => policy.WithOrigins(...)
                    .AllowCredentials()); // Important pour SignalR
```

## Limitations et considérations

1. **Stockage local** : Les notifications sont stockées en mémoire côté client
   - Perdues lors du rechargement de la page
   - Maximum 20 notifications conservées

2. **Temps réel** : Les notifications ne sont envoyées qu'aux clients connectés
   - Un utilisateur qui se connecte après ne verra pas les anciennes notifications

3. **Performance** : Avec SignalR, chaque notification est envoyée à **tous** les clients
   - Pour un grand nombre d'utilisateurs, considérer des groupes SignalR

## Améliorations futures possibles

1. **Persistance** : Sauvegarder les notifications en base de données
2. **Notifications par utilisateur** : Notifications ciblées
3. **Sons** : Jouer un son lors d'une nouvelle notification
4. **Toast** : Afficher un toast temporaire en plus du badge
5. **Groupes SignalR** : Notifications par famille
6. **Historique** : Page dédiée à l'historique des notifications
7. **Push notifications** : Notifications web push (PWA)

## Dépannage

### Les notifications ne s'affichent pas

1. Vérifier que SignalR est bien configuré dans `Program.cs` (Server)
2. Vérifier la connexion WebSocket dans les DevTools
3. Regarder la console pour des erreurs SignalR
4. Vérifier que le `NotificationService` est bien injecté

### Le badge ne se met pas à jour

1. Vérifier que `StateHasChanged()` est appelé
2. Vérifier les événements `OnNotificationReceived` et `OnNotificationsChanged`

### Les notifications sont perdues au refresh

C'est normal, elles sont en mémoire. Pour les conserver, implémenter la persistance.

## Tests

Pour tester :
1. Ouvrir deux navigateurs (ou un en mode incognito)
2. Se connecter avec des comptes différents
3. Publier un article depuis un compte
4. Vérifier que la notification apparaît sur l'autre navigateur

## Support

Le système de notifications fonctionne automatiquement pour :
- ✅ Publications d'articles Lady Whistledown
- 🔄 Peut être étendu pour d'autres événements (votes, scores, etc.)

---

**Créé le :** 2024  
**Dernière mise à jour :** 2024  
**Version :** 1.0
