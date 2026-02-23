# 🔧 Dépannage - Page Mon Espace

## ❌ Erreur : "The JSON value could not be converted to Family"

### 🔍 Diagnostic

Cette erreur se produit lorsque :
1. ❌ **Le serveur n'est pas démarré** (ERR_CONNECTION_REFUSED)
2. ❌ Le serveur retourne une erreur au lieu des données attendues
3. ❌ Les données JSON ne correspondent pas au modèle `Family`

### ✅ Solution

#### 1. Vérifier que le serveur est démarré

**Option A - Démarrage automatique (recommandé)**
```bash
start-both.bat
```

**Option B - Démarrage manuel**
```bash
# Terminal 1 - Serveur
cd BridgertonGame.Server
dotnet run

# Terminal 2 - Client (dans un nouveau terminal)
cd BridgertonGame.Client
dotnet run
```

#### 2. Vérifier la connexion à la base de données

Assurez-vous que MySQL est démarré et accessible.

**Windows - Vérifier le service MySQL**
```bash
# Dans PowerShell ou CMD
sc query MySQL80
```

**Démarrer MySQL si nécessaire**
```bash
net start MySQL80
```

#### 3. Tester l'API directement

Ouvrez votre navigateur et testez :
- `http://localhost:5104/api/families` → Doit retourner la liste des familles
- `http://localhost:5104/api/families/[ID]` → Doit retourner une famille spécifique

Si ces URLs ne fonctionnent pas, le serveur n'est pas démarré correctement.

#### 4. Vérifier les logs du serveur

Dans le terminal du serveur, vérifiez s'il y a des erreurs :
- ✅ `Now listening on: http://localhost:5104` → Serveur OK
- ❌ Erreurs de connexion à la base de données → Vérifier MySQL
- ❌ Erreurs de migration → Lancer `migrate-database.bat`

### 🆕 Améliorations apportées

✅ **Gestion d'erreur robuste** :
- La page ne crash plus si le serveur n'est pas disponible
- Message d'erreur clair affiché à l'utilisateur
- Les logs dans la console pour le débogage

✅ **Message utilisateur** :
```
⚠️ Erreur: Impossible de charger les données de votre famille. 
Veuillez vérifier que le serveur est démarré.
```

### 📝 Checklist de démarrage

- [ ] MySQL est démarré (`sc query MySQL80`)
- [ ] Le serveur .NET est lancé (`cd BridgertonGame.Server && dotnet run`)
- [ ] Le client .NET est lancé (`cd BridgertonGame.Client && dotnet run`)
- [ ] L'URL du serveur est correcte dans `launchSettings.json`
- [ ] Les migrations de base de données sont appliquées

### 🚀 Commande rapide de démarrage

```bash
# Dans le dossier racine du projet
start-both.bat
```

Attendez que les deux serveurs soient démarrés :
- ✅ Serveur : `Now listening on: http://localhost:5104`
- ✅ Client : `Now listening on: http://localhost:5177`

Puis ouvrez : `http://localhost:5177/mon-espace`

---

## 💡 Autres erreurs possibles

### "Failed to load resource: net::ERR_CONNECTION_REFUSED"

**Cause** : Le serveur backend n'est pas démarré.

**Solution** : Lancez `start-both.bat` ou démarrez le serveur manuellement.

### "Erreur de connexion au serveur"

**Cause** : Problème réseau ou configuration incorrecte.

**Solution** : Vérifiez que le port 5104 n'est pas utilisé par une autre application.

### Page blanche sans message

**Cause** : Erreur JavaScript non gérée.

**Solution** : 
1. Ouvrez la console du navigateur (F12)
2. Rechargez la page
3. Copiez l'erreur complète
4. Vérifiez les logs du serveur
