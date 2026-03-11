# 🔧 Dépannage des Votes - Guide Complet

## ⚠️ Problème : Les votes ne sont pas comptabilisés

### Symptôme
Isabelle a voté pour Julien (qui n'est pas Lady Whistledown), mais le compteur de votes incorrects reste à 0.

---

## 🔍 Diagnostic

### Étape 1 : Vérifier l'état de la famille

#### Exécuter le diagnostic
```bash
diagnose-votes.bat
```

Ou directement en SQL :
```sql
SELECT 
    f.Name,
    f.VotingEnabled,
    f.Revealed,
    f.LadyWhistledownId,
    p.Name as LadyWhistledownName
FROM Families f
LEFT JOIN Players p ON f.LadyWhistledownId = p.Id
WHERE f.Name = 'Bridgerton';
```

#### Vérifications nécessaires :
✅ `VotingEnabled` doit être à `1` (TRUE)
✅ `Revealed` doit être à `1` (TRUE) pour que les points soient calculés
✅ `LadyWhistledownId` doit être défini

---

## 🎯 Cause la Plus Probable

### ⚠️ La famille n'est PAS ENCORE RÉVÉLÉE

**IMPORTANT** : Les votes ne sont **pas comptabilisés tant que la famille n'est pas révélée**.

#### Fonctionnement du système :

1. **Phase de vote** (Revealed = 0)
   - Les votes sont enregistrés dans la table `Votes`
   - Les compteurs restent à 0
   - Aucun point n'est attribué
   - **C'est normal !**

2. **Révélation** (Admin active le toggle)
   - `Revealed` passe à 1
   - La méthode `CalculateAndAwardVotePointsAsync` est appelée
   - Les votes corrects/incorrects sont comptabilisés
   - Les points sont calculés et ajoutés

3. **Après révélation** (Revealed = 1)
   - Les statistiques sont visibles
   - Les points sont dans le classement

---

## ✅ Solution

### Si la famille N'EST PAS révélée (cas normal)

**C'est le comportement attendu !**

1. Les votes sont enregistrés mais pas encore comptabilisés
2. Aller dans **Admin > Onglet Révélations**
3. Activer le toggle pour la famille Bridgerton
4. Les votes seront automatiquement comptabilisés

### Si la famille EST révélée mais compteur à 0

#### Vérification 1 : Le vote est-il enregistré ?
```sql
SELECT 
    voter.Name as VoterName,
    votedFor.Name as VotedForName
FROM Votes v
INNER JOIN Players voter ON v.VoterId = voter.Id
INNER JOIN Players votedFor ON v.VotedForId = votedFor.Id
WHERE v.FamilyId = 'bridgerton';
```

Si le vote n'apparaît pas :
- Le vote n'a pas été sauvegardé
- Vérifier que le VotingEnabled était activé au moment du vote

#### Vérification 2 : Les VoteResults existent-ils ?
```sql
SELECT * FROM VoteResults WHERE FamilyId = 'bridgerton';
```

Si aucun résultat :
- La révélation n'a pas déclenché le calcul
- **Solution** : Désactiver puis réactiver le toggle de révélation

#### Vérification 3 : Lady Whistledown est-elle définie ?
```sql
SELECT 
    f.LadyWhistledownId,
    p.Name,
    p.IsLadyWhistledown
FROM Families f
LEFT JOIN Players p ON f.LadyWhistledownId = p.Id
WHERE f.Name = 'Bridgerton';
```

Si `LadyWhistledownId` est NULL :
- **Problème** : Impossible de calculer les votes sans Lady Whistledown
- **Solution** : Définir Lady Whistledown dans Admin > Familles

---

## 🔄 Procédure de Correction

### Méthode 1 : Toggle Révélation (Recommandé)

1. **Admin > Onglet Révélations**
2. **Désactiver** le toggle pour Bridgerton (OFF)
3. Attendre 2 secondes
4. **Réactiver** le toggle (ON)
5. Les votes seront recalculés automatiquement

### Méthode 2 : SQL Direct (Avancé)

```sql
-- Forcer le recalcul en désactivant puis réactivant la révélation
UPDATE Families 
SET Revealed = 0 
WHERE Name = 'Bridgerton';

-- Puis réactiver via l'interface admin
-- Ou directement en SQL (déconseillé) :
UPDATE Families 
SET Revealed = 1 
WHERE Name = 'Bridgerton';

-- Note : Le recalcul automatique nécessite de passer par l'API
```

### Méthode 3 : Recalcul Manuel (Temporaire)

Si le toggle ne fonctionne pas, forcer via SQL :

```sql
-- 1. Compter les votes
SET @familyId = 'bridgerton';
SET @ladyWhistledownId = (
    SELECT LadyWhistledownId 
    FROM Families 
    WHERE Id = @familyId
);

SET @correctVotes = (
    SELECT COUNT(*) 
    FROM Votes 
    WHERE FamilyId = @familyId 
      AND VotedForId = @ladyWhistledownId
);

SET @incorrectVotes = (
    SELECT COUNT(*) 
    FROM Votes 
    WHERE FamilyId = @familyId 
      AND VotedForId != @ladyWhistledownId
);

SET @pointsAwarded = (@correctVotes * 10) - (@incorrectVotes * 10);

-- 2. Mettre à jour VoteResults
INSERT INTO VoteResults (FamilyId, CorrectVotes, IncorrectVotes, PointsAwarded, RevealedAt)
VALUES (@familyId, @correctVotes, @incorrectVotes, @pointsAwarded, NOW())
ON DUPLICATE KEY UPDATE
    CorrectVotes = @correctVotes,
    IncorrectVotes = @incorrectVotes,
    PointsAwarded = @pointsAwarded,
    RevealedAt = NOW();

-- 3. Mettre à jour GameScores
INSERT INTO GameScores (GameName, FamilyId, Score)
VALUES ('Votes Lady Whistledown', @familyId, @pointsAwarded)
ON DUPLICATE KEY UPDATE
    Score = @pointsAwarded;

SELECT 
    'Votes recalculés avec succès!' as Status,
    @correctVotes as CorrectVotes,
    @incorrectVotes as IncorrectVotes,
    @pointsAwarded as PointsAwarded;
```

---

## 📊 Vérification Après Correction

### Dans l'Admin
1. **Onglet Votes** : Ouvrir les détails de Bridgerton
   - Voir le nombre de votes
   - Bordure rouge pour le vote d'Isabelle (incorrect)
   - Résultats calculés affichés

2. **Onglet Révélations** : Voir les statistiques
   - Votes corrects/incorrects
   - Points attribués

3. **Onglet Scores** : Vérifier
   - Ligne "Votes Lady Whistledown"
   - Points pour famille Bridgerton

### Sur la Page Classement
- Carte Lady Whistledown avec résultats des votes
- Points affichés

---

## 🎯 Checklist Complète

Avant de révéler, vérifier :
- [ ] Lady Whistledown est définie
- [ ] Le vote est activé
- [ ] Des votes sont enregistrés
- [ ] Les votes apparaissent dans l'onglet Votes

Après révélation, vérifier :
- [ ] Le toggle est sur ON (RÉVÉLÉ)
- [ ] Les VoteResults existent en base
- [ ] Les points apparaissent dans GameScores
- [ ] Le classement est mis à jour

---

## 💡 Conseils

### Pour éviter ce problème
1. ✅ Toujours définir Lady Whistledown AVANT d'activer le vote
2. ✅ Ne révéler qu'une fois tous les votes enregistrés
3. ✅ Vérifier le compteur de votes avant de révéler

### Comportement Normal
- ⏳ Avant révélation : Votes visibles mais compteurs à 0
- ✅ Après révélation : Compteurs et points calculés

### Si rien ne fonctionne
1. Vérifier les logs du serveur
2. Redémarrer l'application
3. Exécuter `diagnose-votes.bat`
4. Contacter le support avec les résultats du diagnostic

---

## 📞 Support SQL

### Vérifier tout le système de votes
```sql
-- État complet pour la famille Bridgerton
SELECT 
    'Famille' as Type,
    f.Name,
    f.VotingEnabled,
    f.Revealed,
    p.Name as LadyWhistledown
FROM Families f
LEFT JOIN Players p ON f.LadyWhistledownId = p.Id
WHERE f.Name = 'Bridgerton'

UNION ALL

SELECT 
    'Votes' as Type,
    voter.Name,
    '->' as Arrow,
    votedFor.Name
FROM Votes v
INNER JOIN Players voter ON v.VoterId = voter.Id
INNER JOIN Players votedFor ON v.VotedForId = votedFor.Id
WHERE v.FamilyId = 'bridgerton'

UNION ALL

SELECT 
    'Résultats' as Type,
    CONCAT(vr.CorrectVotes, ' corrects') as Data1,
    CONCAT(vr.IncorrectVotes, ' incorrects') as Data2,
    CONCAT(vr.PointsAwarded, ' pts') as Data3
FROM VoteResults vr
WHERE vr.FamilyId = 'bridgerton';
```

---

## 🎓 Rappel du Fonctionnement

```
Joueur vote
    ↓
Vote enregistré dans table Votes
    ↓
Compteurs RESTENT À 0 (normal)
    ↓
Admin révèle (toggle ON)
    ↓
CalculateAndAwardVotePointsAsync() appelée
    ↓
Compteurs mis à jour
Points calculés
VoteResults créé
GameScore "Votes Lady Whistledown" ajouté
    ↓
Résultats visibles partout
```

---

## ✅ Résumé

**Si les compteurs sont à 0 ET la famille n'est pas révélée** : C'est NORMAL !

**Si les compteurs sont à 0 ET la famille EST révélée** : Désactiver/Réactiver le toggle

**Si rien ne fonctionne** : Exécuter `diagnose-votes.bat` et vérifier les résultats
