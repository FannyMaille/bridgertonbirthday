# 🗳️ Système de Votes Lady Whistledown

## Vue d'ensemble

Le système de votes permet aux joueurs de voter pour identifier Lady Whistledown dans leur famille. Lorsque l'admin révèle l'identité, les points sont automatiquement attribués en fonction des votes corrects et incorrects.

## 📊 Règles de Points

- **Vote correct** : +10 points pour la famille
- **Vote incorrect** : -10 points pour la famille
- Les points sont calculés automatiquement lors de la révélation

## 🎮 Fonctionnement

### Pour les Joueurs (Mon Espace)

1. **Activer le vote** : L'admin doit d'abord activer le vote pour la famille dans l'onglet "Votes"
2. **Voter** : Les joueurs peuvent sélectionner un membre de leur famille et confirmer leur vote
3. **Modification** : Un joueur peut changer son vote tant que la révélation n'a pas eu lieu
4. **Voir les résultats** : Une fois révélé, les joueurs peuvent voir les résultats des votes sur la page "Classement"

### Pour l'Admin (Admin Dashboard)

#### Onglet "Votes"
- Activer/désactiver le vote pour chaque famille
- **Voir le nombre de votes** enregistrés par famille
- **Détails des votes en temps réel** :
  - Qui a voté pour qui
  - Indicateur correct/incorrect (✓/✗)
  - **Bouton de suppression** pour retirer un vote spécifique
  - Résultats calculés (votes corrects/incorrects + points) si révélé
  - Statut d'attente si non révélé
- Contrôle individuel par famille

#### Onglet "Révélations"
- Toggle pour révéler/masquer l'identité de Lady Whistledown
- **Lors de la révélation** :
  - Les votes sont comptabilisés automatiquement
  - Les points sont calculés (+10 par vote correct, -10 par vote incorrect)
  - Un nouveau jeu "Votes Lady Whistledown" est ajouté dans les scores
  - Les détails des votes s'affichent dans l'interface admin

- **Informations affichées** :
  - Nom de la vraie Lady Whistledown
  - Nombre de votes corrects
  - Nombre de votes incorrects
  - Total des points attribués
  - Détail de chaque vote (qui a voté pour qui + points)
  - **Bouton de suppression** pour retirer un vote spécifique

#### Fonctionnalités du Toggle Révélation
- ✅ **ON** → Révèle l'identité et calcule les points
- ❌ **OFF** → Masque l'identité et retire les points calculés
- Permet de corriger une révélation accidentelle

#### Suppression de Votes 🗑️
- **Bouton disponible** dans les onglets "Votes" et "Révélations"
- **Confirmation requise** avant suppression
- **Recalcul automatique** des points si famille révélée
- Permet de corriger des erreurs de vote

### Page Classement (Public)

#### Section Lady Whistledown
- Affichage des Lady Whistledown révélées avec leur photo
- Classement personnel basé sur les points (articles + votes)
- **Résultats des votes visibles** :
  - Nombre de votes corrects
  - Nombre de votes incorrects
  - Points attribués
- Compteur d'articles publiés
- Couronne pour la 1ère place

## 🔧 Installation

1. **Créer la migration** :
   ```bash
   create-vote-migration.bat
   ```

2. **Appliquer la migration** :
   ```bash
   apply-vote-migration.bat
   ```

3. **Redémarrer l'application** pour prendre en compte les nouveaux modèles

## 📁 Structure de la Base de Données

### Table `Votes`
| Colonne | Type | Description |
|---------|------|-------------|
| Id | int | Clé primaire auto-incrémentée |
| FamilyId | string | ID de la famille |
| VoterId | string | ID du joueur qui vote |
| VotedForId | string | ID du joueur pour qui on vote |
| VotedAt | DateTime | Date/heure du vote |

### Table `VoteResults`
| Colonne | Type | Description |
|---------|------|-------------|
| Id | int | Clé primaire auto-incrémentée |
| FamilyId | string | ID de la famille |
| CorrectVotes | int | Nombre de votes corrects |
| IncorrectVotes | int | Nombre de votes incorrects |
| PointsAwarded | int | Points nets attribués |
| RevealedAt | DateTime | Date/heure de la révélation |

## 🎯 Exemple de Calcul

**Famille Bridgerton** (5 membres)
- Lady Whistledown réelle : Daphné
- Votes :
  - Simon → Daphné ✅ (+10 points)
  - Eloïse → Daphné ✅ (+10 points)
  - Anthony → Penelope ❌ (-10 points)

**Résultat** : 2 votes corrects, 1 vote incorrect = +10 points nets pour la famille

## 🚀 Workflow Complet

1. **Préparation** (Admin)
   - Définir Lady Whistledown pour chaque famille (onglet Familles)
   - Activer le vote pour les familles (onglet Votes)
   - Observer les votes en temps réel dans l'onglet Votes

2. **Phase de Vote** (Joueurs)
   - Les joueurs se connectent sur "Mon Espace"
   - Ils votent pour un membre de leur famille
   - Ils peuvent modifier leur vote
   - L'admin peut voir qui a voté et pour qui (onglet Votes)

3. **Révélation** (Admin)
   - Aller dans l'onglet "Révélations"
   - Activer le toggle pour révéler
   - Les points sont calculés automatiquement
   - Les détails s'affichent dans l'interface
   - Les résultats sont aussi visibles dans l'onglet "Votes"

4. **Résultats** (Tous)
   - Les points apparaissent dans le classement
   - L'admin peut voir les détails des votes (onglets Votes et Révélations)
   - Les joueurs voient les résultats sur la page "Classement"
   - **Affichage public** : Cartes Lady Whistledown avec résultats de votes

## 📊 Affichage des Votes

### Dans l'Admin - Onglet "Votes"
Pour chaque famille, vous pouvez voir :
- ✅ **Statut du vote** : Activé/Désactivé
- 👤 **Lady Whistledown définie** : Nom
- 🗳️ **Nombre de votes** : Compteur mis à jour en temps réel
- 📋 **Détails des votes** (accordéon déroulant) :
  - Liste de tous les votes : "Nom du votant → Nom voté"
  - Indicateur visuel ✓ (correct) ou ✗ (incorrect)
  - Bordure verte pour votes corrects, rouge pour incorrects
- 📊 **Résultats calculés** (si révélé) :
  - Nombre de votes corrects/incorrects
  - Points totaux attribués
- ⏳ **Statut d'attente** (si non révélé mais votes présents)

### Dans l'Admin - Onglet "Révélations"
Pour chaque famille révélée :
- 🎭 Nom de la vraie Lady Whistledown
- 📊 Statistiques :
  - Votes corrects (badge vert)
  - Votes incorrects (badge rouge)
  - Points totaux (badge avec couleur selon positif/négatif)
- 📋 Détails de chaque vote (accordéon déroulant)

### Sur la Page Classement (Public)
Pour chaque Lady Whistledown **révélée** :
- 🗳️ Section "Résultats des votes"
- Grille avec :
  - Votes corrects (badge vert)
  - Votes incorrects (badge rouge)
- Total des points attribués (badge coloré selon score)
- Design intégré dans les cartes personnages

## ⚠️ Points d'Attention

- Un joueur ne peut voter qu'une seule fois (mais peut modifier son vote)
- Les points ne sont attribués qu'au moment de la révélation
- Si vous désactivez la révélation, les points sont retirés
- Assurez-vous qu'une Lady Whistledown est définie avant d'activer le vote
- **La suppression d'un vote** :
  - Nécessite une confirmation
  - Recalcule automatiquement les points si la famille est révélée
  - Ne peut pas être annulée (suppression définitive)

## 🔗 API Endpoints

- `POST /api/families/{id}/vote` - Enregistrer un vote
- `GET /api/families/{id}/vote-results` - Obtenir les résultats d'une famille
- `GET /api/families/vote-results` - Obtenir tous les résultats
- `POST /api/families/{id}/toggle-reveal` - Révéler/masquer l'identité
- `DELETE /api/families/{familyId}/vote/{voterId}` - Supprimer un vote spécifique
