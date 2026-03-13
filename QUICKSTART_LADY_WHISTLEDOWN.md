# 🎭 Guide Rapide - Correction Lady Whistledown

## ⚡ Action Immédiate

### 1. Synchroniser la Base de Données

```bash
sync-lady-whistledown.bat
```

Cela va :
- Réinitialiser tous les rôles Lady Whistledown
- Synchroniser automatiquement selon `Families.LadyWhistledownId`
- Mettre à jour `Player.IsLadyWhistledown` et `Player.Role`

### 2. Vérifier dans l'Interface

1. Démarrez le serveur
2. Allez dans **Admin** (http://localhost:5000/admin)
3. Connectez-vous
4. Allez dans **Utilisateurs**
5. Vérifiez que les bons personnages ont le badge 🎭

## 🔧 Comment Définir un Lady Whistledown

### Méthode 1 : Via le Bouton 🎭

1. **Admin** → **Utilisateurs**
2. Trouvez le joueur
3. Cliquez sur le bouton **🎭**
4. Confirmez

✅ Synchronisation automatique de tout !

### Méthode 2 : Via Modifier le Joueur

1. **Admin** → **Utilisateurs**
2. Cliquez sur **✏️** (Modifier)
3. Changez le **Rôle** vers "Lady Whistledown"
4. Sauvegardez

✅ Synchronisation automatique de tout !

## 🎯 Vérifications

### Vérifier qui est Lady Whistledown

**Interface Admin → Familles :**
1. Allez dans **Familles**
2. Cliquez sur 👁️ à côté de "Lady Whistledown"
3. Vérifiez le nom affiché

**Interface Admin → Utilisateurs :**
- Les Lady Whistledown ont un badge 🎭 doré

### Vérifier que les Articles Fonctionnent

1. Connectez-vous en tant que Lady Whistledown (Mon Espace)
2. Vous devriez voir la section "Publier une Chronique"
3. Publiez un article
4. Vérifiez dans **Admin → Scores → Pénalités Whistledown** que les points sont corrects

### Vérifier que les Votes Fonctionnent

1. Activez le vote pour une famille (**Admin → Votes**)
2. Connectez-vous en tant que membre de cette famille
3. Votez pour quelqu'un
4. Dans **Admin → Votes**, vérifiez que le vote apparaît

## 🔄 Le Système Maintient Maintenant

### Synchronisation Automatique

Quand vous :
- ✅ Cliquez sur le bouton 🎭
- ✅ Modifiez le rôle d'un joueur
- ✅ Créez un nouveau joueur avec le rôle "Lady Whistledown"

Le système synchronise automatiquement :
- `Player.IsLadyWhistledown`
- `Player.Role`
- `Family.LadyWhistledownId`

### Protection Automatique

- Un seul Lady Whistledown par famille
- L'ancien perd automatiquement le rôle quand un nouveau est défini
- La référence dans la famille est toujours à jour

## 📊 Comprendre les Points

### Points Lady Whistledown (Personnels)

**Affiché dans :**
- Page Classement (badges avec couronne 👑)
- Mon Espace (pour les Lady Whistledown)

**Calculé par :**
- +10 points par article publié

### Pénalités Famille

**Affiché dans :**
- Admin → Scores → Pénalités Whistledown
- Tableau des Points (ligne "Pénalités Whistledown")

**Calculé par :**
- -10 points par article publié (pénalité pour la famille)

### Points de Votes

**Affiché dans :**
- Page Classement (après révélation)
- Admin → Votes (après révélation)

**Calculé par :**
- +10 points par vote correct
- -10 points par vote incorrect

## 🐛 En Cas de Problème

### Les articles ne s'affichent pas pour un joueur

```bash
# Vérifiez que IsLadyWhistledown est à true
sync-lady-whistledown.bat
```

### Les votes pointent vers la mauvaise personne

```bash
# Resynchronisez la base de données
sync-lady-whistledown.bat
```

### Les points ne sont pas corrects

1. **Admin → Scores**
2. Vérifiez les pénalités manuelles
3. Recalculez si nécessaire

## ✨ Nouvelles Fonctionnalités

### Page Admin → Utilisateurs

- **Badge 🎭** : Indique visuellement qui est Lady Whistledown
- **Bouton 🎭** : Bascule facilement le rôle Lady Whistledown
- **Synchronisation automatique** : Tout est mis à jour ensemble

### Page Admin → Familles

- **Affichage Lady Whistledown** : Voir qui est Lady Whistledown dans chaque famille
- **Masquage/Affichage** : Bouton 👁️ pour protéger l'identité

## 📝 Checklist Finale

- [ ] Exécuté `sync-lady-whistledown.bat`
- [ ] Vérifié les badges 🎭 dans Admin → Utilisateurs
- [ ] Vérifié les noms dans Admin → Familles
- [ ] Testé la publication d'article en tant que Lady Whistledown
- [ ] Vérifié que les pénalités sont correctes
- [ ] Testé le système de vote
- [ ] Vérifié l'affichage dans la page Classement

✅ Si tout est coché, le système est prêt !
