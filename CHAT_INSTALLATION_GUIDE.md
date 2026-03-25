# Guide d'Installation - Chat Lady Whistledown

## Vue d'ensemble

Ce système ajoute un chat privé entre les Lady Whistledown avec :
- ✅ Messages en temps réel via SignalR
- ✅ Conservation des messages en base de données
- ✅ Interface admin pour consulter et supprimer les messages
- ✅ Restriction : seules les Lady Whistledown peuvent envoyer des messages

## Étapes d'installation

### 1. Créer la migration de base de données

```bash
create-chat-migration.bat
```

### 2. Appliquer la migration

```bash
apply-chat-migration.bat
```

### 3. Vérifier l'installation

La table `ChatMessages` doit être créée avec les colonnes :
- `Id` (int, auto-increment, primary key)
- `SenderId` (varchar)
- `SenderName` (varchar)
- `FamilyName` (varchar)
- `Content` (varchar(300))
- `SentAt` (datetime)

## Fonctionnalités

### Côté Utilisateur (Lady Whistledown)

Le composant de chat apparaît automatiquement dans "Mon Espace" pour les joueurs ayant le rôle "Lady Whistledown".

**Fonctionnalités :**
- Voir tous les messages de toutes les Lady Whistledown
- Envoyer des messages (max 300 caractères)
- Voir le nom et la famille de chaque expéditeur
- Messages en temps réel sans rafraîchissement de page
- Limitation : seules les Lady Whistledown peuvent poster

**Emplacement :** Section "Équipe Lady Whistledown" dans MonEspace.razor

### Côté Admin

L'admin peut :
- Voir tous les messages du chat
- Voir le nombre total de messages
- Supprimer tous les messages d'un coup
- Les messages sont affichés avec :
  - Nom de l'expéditeur
  - Famille
  - Contenu
  - Date et heure

**Emplacement :** Nouvel onglet "Chat LW" dans Admin.razor

## Architecture

### Composants créés

1. **Server/Data/Entities/ChatMessage.cs** - Entité de base de données
2. **Shared/Models/ChatMessage.cs** - Modèle partagé
3. **Server/Controllers/ChatController.cs** - API endpoints
4. **Server/Hubs/ChatHub.cs** - Hub SignalR pour temps réel
5. **Client/Services/ChatService.cs** - Service client
6. **Client/Shared/LadyWhistledownChat.razor** - Composant UI

### API Endpoints

- `GET /api/chat` - Récupérer tous les messages
- `POST /api/chat` - Envoyer un message
- `DELETE /api/chat` - Supprimer tous les messages
- `GET /api/chat/count` - Obtenir le nombre de messages

### SignalR Events

- `ReceiveMessage` - Nouveau message reçu
- `MessagesCleared` - Tous les messages supprimés

## Modifications de fichiers existants

### BridgertonGame.Server/Data/BridgertonDbContext.cs
- ✅ Ajout du DbSet<ChatMessage>

### BridgertonGame.Server/Services/DatabaseGameDataService.cs
- ✅ Méthodes de gestion du chat ajoutées

### BridgertonGame.Server/Program.cs
- ✅ Route ChatHub ajoutée

### BridgertonGame.Client/Program.cs
- ✅ ChatService enregistré

### BridgertonGame.Client/Pages/MonEspace.razor
- ✅ Composant LadyWhistledownChat intégré

### BridgertonGame.Client/Pages/Admin.razor.cs
- ✅ Méthodes de gestion du chat ajoutées

### BridgertonGame.Client/wwwroot/css/mon-espace.css
- ✅ Styles du chat ajoutés

## Test de l'installation

1. Connectez-vous en tant que Lady Whistledown sur 2 navigateurs différents
2. Envoyez un message depuis le premier navigateur
3. Vérifiez que le message apparaît immédiatement sur le second
4. Connectez-vous en admin
5. Allez dans l'onglet "Chat LW"
6. Vérifiez que vous voyez tous les messages
7. Testez la suppression de tous les messages

## Sécurité

- ✅ Seules les Lady Whistledown peuvent envoyer des messages (validation côté serveur)
- ✅ Limite de 300 caractères par message
- ✅ L'admin peut tout supprimer pour modération

## Support

Si vous rencontrez des problèmes :
1. Vérifiez que la migration a bien été appliquée
2. Vérifiez que SignalR fonctionne (teste avec le NotificationHub existant)
3. Vérifiez les logs serveur pour les erreurs de connexion

## Prochaines étapes possibles

- [ ] Ajouter la suppression de messages individuels (admin)
- [ ] Ajouter un système de pagination pour les anciens messages
- [ ] Ajouter des notifications quand un nouveau message arrive
- [ ] Ajouter la possibilité d'éditer/supprimer ses propres messages
