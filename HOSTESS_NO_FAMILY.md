# 👑 Maîtresse de maison - Fonctionnalité sans famille

## ✅ Fonctionnalité implémentée

### 🎯 Objectif
Permettre à une "Maîtresse de maison" de ne pas être associée à une famille, car elle est l'hôte du bal et ne joue pas pour une équipe.

### ✨ Caractéristiques

**Quand un joueur a le rôle "Maîtresse de maison" :**
- ✅ Le champ "Famille" est **optionnel** (peut être vide)
- ✅ Le sélecteur de famille est **désactivé** dans le formulaire
- ✅ Un message explicatif s'affiche : "ℹ️ Une Maîtresse de maison n'appartient à aucune famille"
- ✅ Dans "Mon Espace", un message spécial s'affiche au lieu des informations de famille

---

## 🔧 Modifications apportées

### 1. Modèle `Player.cs`
```csharp
public string? FamilyId { get; set; } // Nullable pour les Maîtresses de maison
```
Le `FamilyId` est maintenant **nullable** (`string?`)

### 2. Validation dans `Admin.razor`
```csharp
private bool IsPlayerValid()
{
    if (editingPlayer == null) return false;
    
    // Pour une Maîtresse de maison, la famille n'est pas obligatoire
    bool familyValid = editingPlayer.Role == "Maîtresse de maison" || 
                      !string.IsNullOrWhiteSpace(editingPlayer.FamilyId);
    
    return !string.IsNullOrWhiteSpace(editingPlayer.Name) &&
           !string.IsNullOrWhiteSpace(editingPlayer.Title) &&
           !string.IsNullOrWhiteSpace(editingPlayer.Code) &&
           !string.IsNullOrWhiteSpace(editingPlayer.Role) &&
           !string.IsNullOrWhiteSpace(editingPlayer.ImageUrl) &&
           familyValid;
}
```

### 3. Formulaire d'édition
Le sélecteur de famille est désactivé automatiquement :
```razor
<select @bind="editingPlayer.FamilyId" 
        class="form-control" 
        disabled="@(editingPlayer.Role == "Maîtresse de maison")">
    <option value="">
        @(editingPlayer.Role == "Maîtresse de maison" 
            ? "Non applicable (Hôte du bal)" 
            : "-- Sélectionner une famille --")
    </option>
    ...
</select>
```

### 4. Synchronisation automatique
Quand on sélectionne "Maîtresse de maison", la famille est automatiquement retirée :
```csharp
private void OnRoleChanged()
{
    if (editingPlayer == null) return;
    
    editingPlayer.IsLadyWhistledown = editingPlayer.Role == "Lady Whistledown";
    
    // Si c'est une Maîtresse de maison, retirer la famille
    if (editingPlayer.Role == "Maîtresse de maison")
    {
        editingPlayer.FamilyId = null;
    }
}
```

### 5. Affichage dans "Mon Espace"
```csharp
private async Task LoadPlayerData()
{
    if (currentPlayer == null) return;

    try
    {
        // Si c'est une Maîtresse de maison, elle n'a pas de famille
        if (currentPlayer.Role == "Maîtresse de maison")
        {
            // Pas besoin de charger la famille ni les membres
            return;
        }
        
        // ...reste du code...
    }
}
```

Message spécial affiché :
```
┌──────────────────────────────────────┐
│           👑                         │
│      Hôte du Bal                     │
│                                      │
│ Bienvenue, [Nom de la Maîtresse] !  │
│ Vous êtes l'hôte de cet événement   │
│ prestigieux.                          │
└──────────────────────────────────────┘
```

---

## 📝 Utilisation

### Créer une Maîtresse de maison

1. Admin → Utilisateurs → **➕ Ajouter un personnage**
2. Remplir les informations :
   - Nom : Ex. "Lady Danbury"
   - Titre : Ex. "Maîtresse de maison"
   - Code : Ex. "DANBURY"
   - Rôle : **"Maîtresse de maison"** ← Sélectionner ce rôle
   - Image : URL de l'image
3. ✅ Le champ "Famille" se grise automatiquement
4. ✅ Message : "ℹ️ Une Maîtresse de maison n'appartient à aucune famille"
5. Enregistrer

### Modifier un joueur existant en Maîtresse de maison

1. Admin → Utilisateurs → Trouver le joueur
2. Cliquer sur **✏️ Modifier**
3. Changer le rôle vers **"Maîtresse de maison"**
4. ✅ Le champ "Famille" se vide et se grise automatiquement
5. Enregistrer

---

## 🎨 Expérience utilisateur

### Dans l'Admin

**Formulaire d'édition :**
```
┌────────────────────────────────────────┐
│ Rôle                                   │
│ [Maîtresse de maison ▼]                │
│ ✓ Famille automatiquement retirée      │
├────────────────────────────────────────┤
│ Famille                                │
│ [Non applicable (Hôte du bal) ▼] 🔒   │
│ ℹ️ Une Maîtresse de maison             │
│    n'appartient à aucune famille       │
└────────────────────────────────────────┘
```

### Dans "Mon Espace"

**Pour les joueurs normaux :**
- ✅ Affichage de la famille
- ✅ Liste des membres de la famille
- ✅ Statistiques de la famille

**Pour la Maîtresse de maison :**
```
┌───────────────────────────────────┐
│ [Photo]  Nom                      │
│          Titre                     │
│          Rôle: Maîtresse de...    │
├───────────────────────────────────┤
│            👑                      │
│       Hôte du Bal                 │
│                                   │
│ Bienvenue, Lady Danbury !         │
│ Vous êtes l'hôte de cet événement │
│ prestigieux.                       │
└───────────────────────────────────┘
```

---

## 🔄 Flux complet

```
Sélection du rôle "Maîtresse de maison"
              │
              ▼
    OnRoleChanged() déclenché
              │
              ├─► editingPlayer.FamilyId = null
              └─► editingPlayer.IsLadyWhistledown = false
              │
              ▼
    Champ Famille désactivé (grayed out)
              │
              ▼
    Message affiché : "Non applicable (Hôte du bal)"
              │
              ▼
    Validation : familyValid = true (car Maîtresse)
              │
              ▼
    Sauvegarde en BDD avec FamilyId = NULL
              │
              ▼
    Dans "Mon Espace" :
    - Détection du rôle "Maîtresse de maison"
    - Pas de chargement de famille
    - Affichage du message "Hôte du Bal"
```

---

## ✅ Avantages

| Fonctionnalité | Avant | Maintenant |
|----------------|-------|------------|
| Famille obligatoire ? | ✅ Oui, toujours | ✅ Non, si Maîtresse |
| Message clair ? | ❌ Confusion | ✅ Message explicite |
| Sélecteur désactivé ? | ❌ Actif (confus) | ✅ Grisé automatiquement |
| Synchronisation auto ? | ❌ Manuelle | ✅ Automatique au changement de rôle |

---

## 🧪 Tests

### Test 1 : Créer une Maîtresse de maison
1. ✅ Créer un personnage avec rôle "Maîtresse de maison"
2. ✅ Vérifier que le champ Famille est grisé
3. ✅ Enregistrer sans famille
4. ✅ Se connecter avec ce code dans "Mon Espace"
5. ✅ Vérifier que le message "Hôte du Bal" s'affiche

### Test 2 : Changer un joueur en Maîtresse de maison
1. ✅ Modifier un joueur normal (avec famille)
2. ✅ Changer le rôle en "Maîtresse de maison"
3. ✅ Vérifier que le FamilyId se vide automatiquement
4. ✅ Enregistrer
5. ✅ Vérifier que l'utilisateur ne voit plus sa famille dans "Mon Espace"

### Test 3 : Changer une Maîtresse en joueur normal
1. ✅ Modifier une Maîtresse de maison
2. ✅ Changer le rôle en "Invité(e)"
3. ✅ Sélectionner une famille (le champ se réactive)
4. ✅ Enregistrer
5. ✅ Vérifier que l'utilisateur voit maintenant sa famille

---

## 🔍 Vérification dans la base de données

```sql
-- Voir les joueurs sans famille
SELECT Name, Title, Role, FamilyId
FROM Players
WHERE Role = 'Maîtresse de maison';

-- Résultat attendu :
-- Name           | Title                | Role                 | FamilyId
-- Lady Danbury   | Maîtresse de maison  | Maîtresse de maison  | NULL
```

---

## 🛠️ Migration de base de données

Pour appliquer cette modification en base de données :

```bash
# Créer la migration
create-nullable-family-migration.bat

# Appliquer la migration
cd BridgertonGame.Server
dotnet ef database update
```

Cela modifiera la colonne `FamilyId` pour accepter les valeurs NULL.

---

## 💡 Note importante

**Les 3 rôles disponibles :**

| Rôle | Famille requise ? | Peut publier ? | Affichage spécial ? |
|------|-------------------|----------------|---------------------|
| **Maîtresse de maison** | ❌ Non (NULL) | ❌ Non | ✅ Oui (Hôte du Bal) |
| **Lady Whistledown** | ✅ Oui | ✅ Oui (articles) | ❌ Non |
| **Invité(e)** | ✅ Oui | ❌ Non | ❌ Non |

---

## 📚 Documentation connexe

- `ROLE_SYNC_FEATURE.md` - Synchronisation automatique des rôles
- `AUTO_REFRESH_DATA.md` - Rechargement automatique des données
- `DATABASE_MIGRATION.md` - Guide des migrations

