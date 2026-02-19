# BridgertonGame - Instructions de Démarrage

## ⚠️ IMPORTANT : Démarrer les Deux Projets

Cette application nécessite que **le serveur ET le client** soient en cours d'exécution simultanément.

## 🚀 Méthode 1 : Démarrage Automatique (Recommandé)

Double-cliquez sur le fichier :
```
start-both.bat
```

Ce fichier va :
1. Démarrer le serveur API sur https://localhost:7191
2. Attendre que le serveur soit prêt
3. Démarrer le client Blazor sur https://localhost:7113

## 🚀 Méthode 2 : Démarrage Manuel

### Étape 1 : Démarrer le Serveur
Double-cliquez sur `start-server.bat` OU exécutez dans un terminal :
```bash
cd BridgertonGame.Server
dotnet run
```

### Étape 2 : Démarrer le Client
Dans un NOUVEAU terminal, double-cliquez sur `start-client.bat` OU exécutez :
```bash
cd BridgertonGame.Client
dotnet run
```

## 🌐 URLs de l'Application

- **Client (Interface Web)** : https://localhost:7113
- **Serveur API** : https://localhost:7191
- **Swagger (Documentation API)** : https://localhost:7191/swagger

## 🔧 Configuration

### Ports Configurés
- Client : 7113 (HTTPS) / 5257 (HTTP)
- Serveur : 7191 (HTTPS) / 5062 (HTTP)

### Architecture
- **BridgertonGame.Client** : Application Blazor WebAssembly
- **BridgertonGame.Server** : API ASP.NET Core
- **BridgertonGame.Shared** : Modèles partagés

## ❗ Résolution des Problèmes

### Erreur "ERR_CONNECTION_REFUSED"
Cette erreur signifie que le serveur n'est pas démarré. Assurez-vous que :
1. Le serveur est en cours d'exécution (fenêtre console ouverte)
2. Le serveur écoute sur le port 7191
3. Aucun firewall ne bloque la connexion

### Vérifier si le serveur fonctionne
Ouvrez votre navigateur et allez sur : https://localhost:7191/swagger

Si Swagger s'affiche, le serveur fonctionne correctement.

## 📝 Notes

- Les deux applications doivent rester ouvertes pendant l'utilisation
- Ne fermez pas les fenêtres de console
- Le serveur doit démarrer AVANT le client
