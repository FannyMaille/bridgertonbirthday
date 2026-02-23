# 🔄 Migration : FamilyId Nullable

## ⚠️ Migration nécessaire !

Pour que les **Maîtresses de maison** puissent ne pas avoir de famille, il faut modifier la base de données MySQL.

---

## 🚀 Application de la migration

### Méthode 1 : Script automatique (Recommandé)

```bash
create-nullable-family-migration.bat
```

Ce script va :
1. ✅ Créer la migration EF Core
2. ✅ Appliquer la migration à la base de données MySQL
3. ✅ Vérifier que tout s'est bien passé

### Méthode 2 : Manuelle

```bash
# 1. Aller dans le dossier serveur
cd BridgertonGame.Server

# 2. Créer la migration
dotnet ef migrations add MakeFamilyIdNullable

# 3. Appliquer la migration
dotnet ef database update
```

---

## 🔍 Vérifier que la migration est appliquée

### Option 1 : Script de vérification

```bash
verify-family-migration.bat
```

### Option 2 : Manuellement avec MySQL

```bash
mysql -u root -p
```

Puis dans MySQL :
```sql
USE bridgerton;
DESCRIBE Players;
```

**Résultat attendu :**
```
Field     | Type         | Null | Key | Default | Extra
----------|--------------|------|-----|---------|-------
...
FamilyId  | varchar(255) | YES  |     | NULL    |
...
```

✅ Si `Null` = **YES** → La migration est appliquée !
❌ Si `Null` = **NO** → La migration n'est PAS appliquée

---

## 📋 Ce que fait la migration

La migration modifie la table `Players` dans MySQL :

**Avant :**
```sql
FamilyId varchar(255) NOT NULL
```

**Après :**
```sql
FamilyId varchar(255) NULL
```

Cela permet d'avoir des joueurs avec `FamilyId = NULL` (les Maîtresses de maison).

---

## 🧪 Test après migration

### 1. Créer une Maîtresse de maison

1. Démarrez l'application : `start-both.bat`
2. Allez sur : http://localhost:5177/admin
3. Connectez-vous
4. Utilisateurs → **➕ Ajouter un personnage**
5. Remplir :
   - Nom : "Lady Danbury"
   - Titre : "Maîtresse de Bridgerton House"
   - Code : "DANBURY"
   - Rôle : **"Maîtresse de maison"**
   - Image : "images/AdminAvatar.png"
6. ✅ Le champ "Famille" doit être grisé
7. Cliquer sur **"➕ Créer"**

### 2. Vérifier en base de données

```sql
SELECT Name, Role, FamilyId 
FROM Players 
WHERE Role = 'Maîtresse de maison';
```

**Résultat attendu :**
```
Name          | Role                 | FamilyId
Lady Danbury  | Maîtresse de maison  | NULL
```

✅ `FamilyId` doit être **NULL** !

### 3. Vérifier dans "Mon Espace"

1. Allez sur : http://localhost:5177/mon-espace
2. Entrez le code : `DANBURY`
3. ✅ Message spécial doit s'afficher :
   ```
   👑
   Hôte du Bal
   
   Bienvenue, Lady Danbury !
   Vous êtes l'hôte de cet événement prestigieux.
   ```

---

## ❌ Que faire en cas d'erreur ?

### Erreur 1 : "Build failed"

**Cause** : Le projet ne compile pas.

**Solution** :
```bash
cd BridgertonGame.Server
dotnet build
```

Corrigez les erreurs de compilation avant de créer la migration.

### Erreur 2 : "Unable to create migration"

**Cause** : Entity Framework Core n'est pas installé.

**Solution** :
```bash
dotnet tool install --global dotnet-ef
```

Puis réessayez la migration.

### Erreur 3 : "Unable to connect to database"

**Cause** : MySQL n'est pas démarré.

**Solution** :
```bash
net start MySQL80
```

Puis réessayez.

### Erreur 4 : "Column 'FamilyId' cannot be null"

**Cause** : La migration n'a pas été appliquée.

**Solution** :
1. Vérifiez que la migration existe : `BridgertonGame.Server/Migrations/`
2. Appliquez-la : `dotnet ef database update`

---

## 🔄 Rollback (Annuler la migration)

Si vous voulez revenir en arrière :

```bash
cd BridgertonGame.Server

# Revenir à la migration précédente
dotnet ef database update <nom-migration-precedente>

# Supprimer le fichier de migration
# Supprimez manuellement le fichier dans BridgertonGame.Server/Migrations/
```

**⚠️ Attention** : Vous perdrez tous les joueurs "Maîtresse de maison" sans famille !

---

## 📊 Impact de la migration

| Avant | Après |
|-------|-------|
| ❌ Tous les joueurs DOIVENT avoir une famille | ✅ Les Maîtresses de maison peuvent ne PAS avoir de famille |
| ❌ Impossible de créer une Maîtresse sans famille | ✅ Création possible avec `FamilyId = NULL` |
| ❌ Erreur SQL si FamilyId est vide | ✅ Valeur NULL acceptée |

---

## 📝 Fichiers de migration créés

Après avoir exécuté `create-nullable-family-migration.bat`, vous aurez :

```
BridgertonGame.Server/Migrations/
  ├── 20XXXXXX_MakeFamilyIdNullable.cs
  └── 20XXXXXX_MakeFamilyIdNullable.Designer.cs
```

Ces fichiers contiennent le code EF Core pour modifier la base de données.

---

## ✅ Checklist de vérification

- [ ] Migration créée (`dotnet ef migrations add MakeFamilyIdNullable`)
- [ ] Migration appliquée (`dotnet ef database update`)
- [ ] Colonne FamilyId est nullable (vérification MySQL)
- [ ] Test création Maîtresse de maison (sans famille)
- [ ] Test affichage "Mon Espace" (message "Hôte du Bal")
- [ ] Aucune erreur dans les logs du serveur

---

## 🆘 Besoin d'aide ?

Si vous rencontrez des problèmes :

1. **Vérifier les logs du serveur** dans la console
2. **Vérifier la structure de la table** : `DESCRIBE Players;`
3. **Consulter** `TROUBLESHOOTING_MON_ESPACE.md`
4. **Redémarrer** MySQL et l'application

---

## 📚 Documentation connexe

- `HOSTESS_NO_FAMILY.md` - Fonctionnalité Maîtresse de maison
- `DATABASE_MIGRATION.md` - Guide général des migrations
- `MYSQL_READY.md` - Configuration MySQL

