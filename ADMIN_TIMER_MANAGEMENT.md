# 🎭 Gestion des Timers Lady Whistledown - Admin

## 📋 Vue d'ensemble

Nouvelle fonctionnalité dans l'onglet **Articles** de l'interface d'administration permettant de gérer les timers de publication des articles Lady Whistledown pour chaque famille.

## ✨ Fonctionnalités

### 1. **Réinitialiser le Timer** 🔄
- Permet d'autoriser une publication immédiate
- Supprime complètement le cooldown actuel
- Confirmation requise avant l'action

### 2. **Définir le Timer Manuellement** ⏱️
- Choisir le nombre de minutes restantes (0-60)
- `0 min` = publication immédiate autorisée
- `30 min` = durée normale du cooldown
- Interface intuitive avec exemples

### 3. **Affichage du Statut** 📊
- Temps restant avant la prochaine publication
- Heure de la dernière publication
- Statut actuel (peut publier ou non)

## 🎯 Emplacement

**Admin Dashboard** → **Onglet Articles** (📰)

La section est divisée en deux parties :
1. **Gestion des Timers** (en haut)
2. **Articles Publiés** (en bas)

## 📱 Interface

### Carte de Famille

Chaque famille affiche :
- **Nom de la famille**
- **Nombre d'articles publiés** (badge)
- **Lady Whistledown** (masquable avec 👁️)
- **Statut du timer** (temps restant / peut publier)
- **2 boutons d'action** :
  - 🔄 **Réinitialiser** : Publication immédiate
  - ⏱️ **Définir Timer** : Configuration manuelle

## 🔧 Utilisation

### Réinitialiser un Timer

```
1. Cliquer sur "🔄 Réinitialiser"
2. Confirmer l'action
3. Le timer est supprimé ✅
4. La famille peut publier immédiatement
```

### Définir un Timer Personnalisé

```
1. Cliquer sur "⏱️ Définir Timer"
2. Entrer le nombre de minutes (0-60)
3. Confirmer
4. Le timer est mis à jour ✅
```

**Exemples de valeurs :**
- **0** : Publication immédiate possible
- **15** : Peut publier dans 15 minutes  
- **30** : Durée normale (30 minutes)

## 🔗 Endpoints API

### **DELETE** `/api/families/{id}/timer`
Réinitialise le timer de publication

### **POST** `/api/families/{id}/timer/set`
Définit le timer manuellement
```json
{
  "Minutes": 15
}
```

### **GET** `/api/families/{id}/timer/status`
Récupère le statut du timer
```json
{
  "CanPublish": false,
  "TimeUntilNext": "00:15:30",
  "LastPublicationTime": "2024-01-15T14:30:00Z"
}
```

## 💾 Base de Données

La table `PublicationCooldowns` stocke :
- `FamilyId` : ID de la famille
- `LastPublicationTime` : Heure de la dernière publication

Le calcul se fait automatiquement : `LastPublicationTime + 30min = NextAllowedTime`

## 📐 Logique Métier

### Calcul du Timer

```csharp
// Peut publier si 30 minutes se sont écoulées
bool canPublish = (DateTime.UtcNow - LastPublicationTime).TotalMinutes >= 30;

// Temps restant
TimeSpan timeUntilNext = (LastPublicationTime + 30min) - DateTime.UtcNow;
```

### Définir le Timer Manuellement

```csharp
// Pour un cooldown de X minutes à partir de maintenant
LastPublicationTime = DateTime.UtcNow - (30 - X) minutes;

// Exemple: Pour 15 minutes restantes
LastPublicationTime = DateTime.UtcNow - 15 minutes;
// Résultat: Pourra publier dans 15 minutes
```

## 🎨 Style

Les cartes de timers utilisent :
- Design moderne avec `info-card`
- Badge pour le nombre d'articles
- Couleurs thématiques (bleu pour les timers)
- Boutons avec emojis pour la clarté

## ⚠️ Avertissements

### Sécurité
- Seuls les administrateurs authentifiés peuvent gérer les timers
- Confirmations requises pour les réinitialisations

### Limites
- Valeur maximale : 60 minutes
- Valeur minimale : 0 minutes (immédiat)

## 📝 Messages

### Succès
- ✅ "Timer réinitialisé pour la famille {Nom}"
- ✅ "Timer défini à {X} minutes pour la famille {Nom}"

### Erreurs
- ❌ "Erreur lors de la réinitialisation du timer"
- ❌ "Veuillez entrer une valeur entre 0 et 60 minutes"

## 🚀 Cas d'Usage

### Scénario 1 : Correction d'erreur
Un joueur a publié trop tôt à cause d'un bug → Réinitialiser le timer pour autoriser une republication immédiate.

### Scénario 2 : Ajustement du rythme
Vous voulez espacer les publications → Définir le timer à 45 minutes au lieu de 30.

### Scénario 3 : Test
Tester les publications → Réinitialiser les timers de toutes les familles.

### Scénario 4 : Event spécial
Pendant un moment clé → Réduire le cooldown à 10 minutes pour toutes les familles.

## 🔄 Workflow Complet

```
1. 📰 Lady Whistledown publie un article
   └─> LastPublicationTime = now
   └─> Timer démarre (30 min)

2. ⏱️ Pendant le cooldown
   └─> Affichage du temps restant
   └─> Bouton de publication désactivé

3. 🔄 Admin réinitialise (optionnel)
   └─> Supprime le cooldown
   └─> Publication immédiate autorisée

4. ⏱️ Admin définit timer (optionnel)
   └─> Ajuste le temps restant
   └─> Nouveau calcul du cooldown

5. ✅ Après 30 minutes
   └─> Timer expiré
   └─> Nouvelle publication autorisée
```

## 🎯 Bénéfices

1. **Flexibilité** : Ajustement en temps réel des cooldowns
2. **Contrôle** : Gestion fine des publications
3. **Correction** : Résolution rapide des problèmes
4. **Événements** : Adaptation pour des moments spéciaux

## 📚 Fichiers Modifiés

### Frontend
- `BridgertonGame.Client/Pages/Admin.razor` - Interface utilisateur
- `BridgertonGame.Client/Pages/Admin.razor.cs` - Logique métier

### Backend
- `BridgertonGame.Server/Controllers/FamiliesController.cs` - Endpoints API
- `BridgertonGame.Server/Services/DatabaseGameDataService.cs` - Accès données

## ✅ Checklist de Test

- [ ] Réinitialiser un timer
- [ ] Définir un timer à 0 minutes
- [ ] Définir un timer à 30 minutes
- [ ] Définir un timer à 60 minutes
- [ ] Vérifier l'affichage du statut
- [ ] Tester avec plusieurs familles
- [ ] Vérifier la persistance après rechargement
- [ ] Tester la validation (0-60)

## 🎉 Conclusion

Cette fonctionnalité offre un contrôle total sur les timers de publication Lady Whistledown, permettant une gestion flexible et réactive de l'événement Bridgerton !
