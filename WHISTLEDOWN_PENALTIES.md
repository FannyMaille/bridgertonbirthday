# Système de Pénalités et Récompenses Whistledown

## 📋 Fonctionnement

Le jeu The Bridgerton Game intègre un système **double** pour les publications d'articles de Lady Whistledown.

### Règles du Système

Chaque article publié entraîne **deux effets simultanés** :

#### 1. **Pénalité pour la Famille** 🏚️
- **-10 points** sont retirés du score total de la famille
- Cela affecte le classement général
- La famille perd des points au tableau

#### 2. **Récompense pour Lady Whistledown** ⭐
- **+10 points personnels** pour Lady Whistledown
- Ces points sont individuels et ne comptent PAS dans le score de la famille
- Classement personnel des Lady Whistledown

### Récapitulatif
```
Publication d'1 article = -10 pts famille + 10 pts Lady Whistledown
```

## 🎯 Impact sur les Scores

### Calcul du Score Famille

```
Score Total Famille = Somme des jeux - Pénalités Whistledown
```

**Exemple Famille Bridgerton :**
- Jeu 1 : 50 points
- Jeu 2 : 30 points
- Jeu 3 : 20 points
- Articles publiés : 3
- **Pénalités famille** : 3 × 10 = -30 points
- **Score Total Famille** : 50 + 30 + 20 - 30 = **70 points**

### Points Lady Whistledown

**Exemple Daphné Bridgerton (Lady Whistledown) :**
- Articles publiés : 3
- **Points personnels** : 3 × 10 = **30 points**

Ces 30 points sont **séparés** du score de la famille !

## 📊 Affichage

### Page Mon Espace (Lady Whistledown)

Affichage de **deux notifications** :
```
⚠️ Famille : Chaque publication = -10 points
✨ Vous : Chaque publication = +10 points
```

**Compteur personnel bien visible :**
```
┌─────────────────────────────┐
│  Vos points personnels      │
│       30 pts                │
│  Lady Whistledown           │
└─────────────────────────────┘
```

### Page Classement

**Deux sections distinctes :**

1. **Classement des Familles**
   - Basé sur le score total (avec pénalités)
   - Détermine le gagnant de l'événement

2. **Section Lady Whistledown**
   - Photo de chaque Lady Whistledown
   - Points personnels affichés
   - Classement individuel des publications

### Page Admin

- **Nombre d'articles** par famille affiché
- **Pénalités** calculées automatiquement
- **Points Lady Whistledown** visibles
- Possibilité d'ajustement manuel si nécessaire

## 🔧 Fonctionnalités Techniques

### Automatisation

**Publication (`PublishArticleAsync`) :**
- ✅ -10 points à la famille (pénalité)
- ✅ +10 points à Lady Whistledown (récompense)
- ✅ Sauvegarde en base de données

**Suppression (`DeleteArticleAsync`) :**
- ✅ +10 points rendus à la famille
- ✅ -10 points retirés à Lady Whistledown
- ✅ Synchronisation parfaite

**Calcul (`GetAllGameScoresAsync`) :**
- ✅ Ligne "Pénalités Whistledown"
- ✅ Déduction automatique du total
- ✅ Mise à jour des rangs

### Base de Données

**Nouvelle colonne `Players.Points` :**
```sql
ALTER TABLE Players ADD COLUMN Points INT NOT NULL DEFAULT 0;
```

**Migration automatique :**
```bash
migrate-player-points.bat
```

## 💡 Stratégie de Jeu

### Pour les Familles
- ⚖️ **Équilibrer** : Gagner aux jeux vs publier des articles
- 🎯 **Stratégie** : Quand publier pour minimiser l'impact
- 🏆 **Objectif** : Maximiser le score total malgré les pénalités

### Pour Lady Whistledown
- 📝 **Publier régulièrement** pour accumuler des points personnels
- 🎭 **Double jeu** : Aider la famille aux jeux, mais aussi briller individuellement
- 🏅 **Prestige** : Devenir la Lady Whistledown avec le plus de points

## 📈 Exemples Réels

### Famille Bridgerton
**Score Famille :**
- Points des jeux : 120
- Articles publiés : 5  
- Pénalités : -50 points
- **Score final famille : 70 points** → Classement général

**Lady Whistledown (Daphné) :**
- Articles publiés : 5
- **Points personnels : 50 points** → Classement Lady Whistledown

---

### Famille Featherington
**Score Famille :**
- Points des jeux : 80
- Articles publiés : 2
- Pénalités : -20 points
- **Score final famille : 60 points** → Classement général

**Lady Whistledown (Penelope) :**
- Articles publiés : 2
- **Points personnels : 20 points** → Classement Lady Whistledown

---

## 🏆 Double Classement

### 1. Classement des Familles 👑
Basé sur : Score total - Pénalités

**Exemple :**
```
1. Bridgerton    : 70 pts
2. Featherington : 60 pts
3. Sharma        : 55 pts
```

### 2. Classement Lady Whistledown ⭐
Basé sur : Points personnels

**Exemple :**
```
1. Daphné (Bridgerton)     : 50 pts  (5 articles)
2. Kate (Sharma)           : 40 pts  (4 articles)
3. Penelope (Featherington): 20 pts  (2 articles)
```

## 🎮 Équilibre du Jeu

Ce système crée :
- **Tension** : Publier aide personnellement mais pénalise la famille
- **Choix stratégiques** : Quand et combien publier
- **Double compétition** : Famille ET individuelle
- **Rôle valorisé** : Lady Whistledown a un impact visible
- **Récompense personnelle** : Motivation à publier malgré la pénalité

## ⚠️ Points d'Attention

1. **Deux classements séparés** : Famille ≠ Lady Whistledown
2. **Accumulation infinie** : Plus on publie, plus on gagne de points personnels
3. **Impact famille** : Chaque article coûte au groupe
4. **Réversible** : Suppression d'article = ajustement des deux côtés
5. **Visible en temps réel** : Tout se met à jour immédiatement

## 🎯 Objectifs de Game Design

- ✅ Valoriser le rôle de Lady Whistledown
- ✅ Créer des choix stratégiques intéressants
- ✅ Équilibrer coopération et compétition
- ✅ Ajouter une couche de profondeur au jeu
- ✅ Encourager la participation active
