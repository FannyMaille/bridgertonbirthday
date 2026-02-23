# 🎭 Synchronisation automatique du rôle Lady Whistledown

## ✅ Fonctionnalité ajoutée

### 🎯 Problème résolu
Avant, il fallait manuellement :
1. Changer le rôle en "Lady Whistledown"
2. **ET** cliquer sur le bouton 🎭 pour activer `IsLadyWhistledown`

C'était redondant et source d'erreurs !

### ✨ Solution implémentée

**Maintenant, tout est automatique !**

Quand vous sélectionnez le rôle dans le formulaire d'édition :
- ✅ Rôle = **"Lady Whistledown"** → `IsLadyWhistledown` = `true` (automatique)
- ✅ Rôle = **"Maîtresse de maison"** → `IsLadyWhistledown` = `false` (automatique)
- ✅ Rôle = **"Invité(e)"** → `IsLadyWhistledown` = `false` (automatique)

---

## 🔄 Comment ça marche

### 1. Lors de la création ou modification d'un personnage

```
┌─────────────────────────────────────────┐
│  Admin → Utilisateurs → ✏️ Modifier     │
└──────────────────┬──────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────┐
│  Sélectionner le rôle:                  │
│  ○ Maîtresse de maison                  │
│  ● Lady Whistledown  ← Sélectionné      │
│  ○ Invité(e)                            │
└──────────────────┬──────────────────────┘
                   │
                   │ Déclenchement automatique
                   ▼
┌─────────────────────────────────────────┐
│  ✓ IsLadyWhistledown = true             │
│  ✓ Badge 🎭 activé                      │
│  ✓ Capacité de publier des articles     │
└─────────────────────────────────────────┘
```

### 2. Code implémenté

**Dans le formulaire (Admin.razor) :**
```csharp
<select @bind="editingPlayer.Role" @bind:after="OnRoleChanged" class="form-control">
    <option value="">-- Sélectionner un rôle --</option>
    <option value="Maîtresse de maison">Maîtresse de maison</option>
    <option value="Lady Whistledown">Lady Whistledown</option>
    <option value="Invité(e)">Invité(e)</option>
</select>
```

**Méthode de synchronisation :**
```csharp
private void OnRoleChanged()
{
    if (editingPlayer == null) return;
    
    // Mettre à jour IsLadyWhistledown automatiquement selon le rôle
    editingPlayer.IsLadyWhistledown = editingPlayer.Role == "Lady Whistledown";
}
```

**Synchronisation au chargement :**
```csharp
private void EditUser(Player player)
{
    // ...copie des données...
    
    // S'assurer que le rôle et IsLadyWhistledown sont synchronisés
    if (editingPlayer.IsLadyWhistledown && editingPlayer.Role != "Lady Whistledown")
    {
        editingPlayer.Role = "Lady Whistledown";
    }
    else if (!editingPlayer.IsLadyWhistledown && editingPlayer.Role == "Lady Whistledown")
    {
        editingPlayer.IsLadyWhistledown = true;
    }
}
```

---

## 📝 Utilisation

### Créer une nouvelle Lady Whistledown

1. Admin → Utilisateurs → **➕ Ajouter un personnage**
2. Remplir les champs (Nom, Titre, Code, etc.)
3. Dans **Rôle**, sélectionner **"Lady Whistledown"**
   - ✅ Un message vert apparaît : "✓ Ce joueur sera marqué comme Lady Whistledown"
4. Cliquer sur **➕ Créer**
5. ✅ Le personnage est créé avec `IsLadyWhistledown = true`

### Modifier un personnage existant

**Scénario 1 : Promouvoir un invité en Lady Whistledown**
1. Admin → Utilisateurs → Trouver le personnage
2. Cliquer sur **✏️ Modifier**
3. Changer le rôle de "Invité(e)" à **"Lady Whistledown"**
4. ✅ Message vert : "✓ Ce joueur sera marqué comme Lady Whistledown"
5. Enregistrer
6. ✅ Le badge 🎭 apparaît automatiquement !

**Scénario 2 : Rétrograder une Lady Whistledown**
1. Admin → Utilisateurs → Trouver la Lady Whistledown
2. Cliquer sur **✏️ Modifier**
3. Changer le rôle de "Lady Whistledown" à **"Invité(e)"**
4. Enregistrer
5. ✅ Le badge 🎭 disparaît automatiquement !
6. ✅ Le joueur ne peut plus publier d'articles

---

## 🎨 Indicateur visuel

Quand vous sélectionnez "Lady Whistledown" dans le formulaire, un message s'affiche :

```
┌──────────────────────────────────────┐
│ Rôle                                 │
│ [Lady Whistledown ▼]                 │
│ ✓ Ce joueur sera marqué comme        │
│   Lady Whistledown                   │
└──────────────────────────────────────┘
```

Ce message vous confirme que l'activation est automatique !

---

## 🔄 Synchronisation bidirectionnelle

### Cas 1 : Changement de rôle → Mise à jour IsLadyWhistledown
```
Rôle: "Lady Whistledown"  →  IsLadyWhistledown: true
Rôle: "Invité(e)"          →  IsLadyWhistledown: false
```

### Cas 2 : Chargement d'un utilisateur existant
Si un utilisateur a `IsLadyWhistledown = true` mais `Role ≠ "Lady Whistledown"` :
- ✅ Le rôle est automatiquement corrigé en "Lady Whistledown"

Si un utilisateur a `Role = "Lady Whistledown"` mais `IsLadyWhistledown = false` :
- ✅ IsLadyWhistledown est automatiquement mis à `true`

---

## ✅ Avantages

| Avant | Maintenant |
|-------|------------|
| ❌ Changer le rôle manuellement | ✅ Changer le rôle = tout automatique |
| ❌ Cliquer sur le bouton 🎭 | ✅ Pas besoin, c'est synchronisé ! |
| ❌ Risque d'oubli → incohérence | ✅ Impossible d'avoir des données incohérentes |
| ❌ 2 étapes pour activer LW | ✅ 1 seule sélection suffit |

---

## 🧪 Tests

### Test 1 : Créer une Lady Whistledown
1. ✅ Créer un personnage avec rôle "Lady Whistledown"
2. ✅ Vérifier que le badge 🎭 apparaît dans la liste
3. ✅ Se connecter avec ce code dans "Mon Espace"
4. ✅ Vérifier que la section "Publier une Chronique" est visible

### Test 2 : Changer un rôle existant
1. ✅ Modifier un personnage "Invité(e)" en "Lady Whistledown"
2. ✅ Badge 🎭 apparaît
3. ✅ L'utilisateur peut publier des articles

### Test 3 : Rétrograder une Lady Whistledown
1. ✅ Modifier une "Lady Whistledown" en "Invité(e)"
2. ✅ Badge 🎭 disparaît
3. ✅ L'utilisateur ne voit plus la section publication

---

## 🔍 Vérification dans la base de données

```sql
-- Vérifier la synchronisation
SELECT 
    Name, 
    Role, 
    IsLadyWhistledown,
    CASE 
        WHEN Role = 'Lady Whistledown' AND IsLadyWhistledown = 1 THEN '✓ OK'
        WHEN Role != 'Lady Whistledown' AND IsLadyWhistledown = 0 THEN '✓ OK'
        ELSE '❌ INCOHÉRENT'
    END as Status
FROM Players
ORDER BY FamilyId, Name;
```

Tous les résultats devraient afficher "✓ OK" !

---

## 💡 Note importante

**Le bouton 🎭 existe toujours** dans la liste des utilisateurs, mais il sert maintenant principalement à :
- Activer rapidement une Lady Whistledown **sans ouvrir le formulaire**
- Voir visuellement qui est Lady Whistledown

**Mais la méthode recommandée est maintenant :**
1. Cliquer sur **✏️ Modifier**
2. Sélectionner le rôle **"Lady Whistledown"**
3. Enregistrer

C'est plus clair et plus explicite ! 🎭

---

## 📚 Documentation connexe

- `AUTO_REFRESH_DATA.md` - Rechargement automatique des données
- `LADY_WHISTLEDOWN_POINTS.md` - Système de points LW
- `LADY_WHISTLEDOWN_ADMIN.md` - Gestion des Lady Whistledown

