# Configuration SignalR WebSocket en Production

## Problème résolu

Les connexions WebSocket échouaient en production avec l'erreur :
```
WebSocket connection to 'wss://bridgerton-birthday.fr/notificationHub' failed
```

## Modifications apportées

### 1. Configuration serveur ASP.NET Core (`Program.cs`)

✅ **Ajout du middleware WebSockets** avec configuration optimisée
✅ **Configuration SignalR améliorée** avec timeouts adaptés
✅ **Support des origines WebSocket** pour le reverse proxy

### 2. Configuration client SignalR (Services)

✅ **Fallback automatique** : WebSocket → LongPolling si WebSocket échoue
✅ **Reconnexion automatique** avec stratégie exponentielle
✅ **Logs de diagnostic** pour faciliter le débogage

### 3. Configuration du serveur web

Vous DEVEZ configurer votre reverse proxy (Nginx ou Apache) pour supporter les WebSockets.

## Installation en production

### Option A : Nginx

1. **Copier la configuration** :
```bash
sudo cp nginx.conf.example /etc/nginx/sites-available/bridgerton-birthday.fr
```

2. **Modifier les chemins SSL** si nécessaire :
```bash
sudo nano /etc/nginx/sites-available/bridgerton-birthday.fr
```

3. **Activer le site** :
```bash
sudo ln -s /etc/nginx/sites-available/bridgerton-birthday.fr /etc/nginx/sites-enabled/
```

4. **Tester la configuration** :
```bash
sudo nginx -t
```

5. **Redémarrer Nginx** :
```bash
sudo systemctl restart nginx
```

### Option B : Apache

1. **Activer les modules nécessaires** :
```bash
sudo a2enmod proxy
sudo a2enmod proxy_http
sudo a2enmod proxy_wstunnel
sudo a2enmod ssl
sudo a2enmod headers
sudo a2enmod rewrite
```

2. **Copier la configuration** :
```bash
sudo cp apache.conf.example /etc/apache2/sites-available/bridgerton-birthday.fr.conf
```

3. **Modifier les chemins SSL** si nécessaire :
```bash
sudo nano /etc/apache2/sites-available/bridgerton-birthday.fr.conf
```

4. **Activer le site** :
```bash
sudo a2ensite bridgerton-birthday.fr.conf
```

5. **Tester la configuration** :
```bash
sudo apache2ctl configtest
```

6. **Redémarrer Apache** :
```bash
sudo systemctl restart apache2
```

## Points clés à vérifier

### ✅ Liste de vérification

- [ ] Application ASP.NET Core redéployée avec les nouvelles modifications
- [ ] Reverse proxy (Nginx/Apache) configuré pour WebSocket
- [ ] Certificats SSL valides et à jour
- [ ] Port 5000 accessible depuis le reverse proxy
- [ ] Pare-feu autorise les connexions WebSocket
- [ ] En-têtes `Upgrade` et `Connection` correctement transmis
- [ ] Timeouts suffisamment longs (86400s pour WebSocket)

### 🔍 Commandes de diagnostic

**Tester la connexion WebSocket directement** :
```bash
# Installer wscat si nécessaire
npm install -g wscat

# Tester la connexion
wscat -c wss://bridgerton-birthday.fr/notificationHub
```

**Vérifier les logs Nginx** :
```bash
sudo tail -f /var/log/nginx/bridgerton-birthday_error.log
```

**Vérifier les logs Apache** :
```bash
sudo tail -f /var/log/apache2/bridgerton-birthday_error.log
```

**Vérifier les logs ASP.NET Core** :
```bash
sudo journalctl -u bridgerton-birthday.service -f
```

### 🎯 Comportement attendu

Avec les modifications, l'application va :

1. **Essayer WebSocket** en premier (transport le plus performant)
2. **Basculer sur LongPolling** automatiquement si WebSocket échoue
3. **Se reconnecter automatiquement** en cas de déconnexion
4. **Logger les états de connexion** dans la console du navigateur

### 📊 Logs dans la console du navigateur

Vous devriez voir :
```
NotificationHub connected successfully
ChatHub connected successfully
```

En cas d'erreur, vous verrez :
```
NotificationHub connection error: [détails de l'erreur]
NotificationHub reconnecting: [raison]
```

## Déploiement

1. **Commit et push** les modifications :
```bash
git add .
git commit -m "Fix: WebSocket configuration for production SignalR"
git push origin master
```

2. **Déployer sur le serveur** (selon votre méthode de déploiement)

3. **Redémarrer l'application** :
```bash
sudo systemctl restart bridgerton-birthday.service
```

4. **Configurer/redémarrer le reverse proxy** (voir instructions ci-dessus)

## Support

Si les WebSockets ne fonctionnent toujours pas après configuration :

1. L'application basculera automatiquement sur **LongPolling** (fonctionnel mais moins performant)
2. Vérifiez les logs du navigateur (F12 → Console)
3. Vérifiez les logs du serveur web
4. Vérifiez les logs de l'application ASP.NET Core

## Ressources

- [SignalR Hosting and Scaling](https://docs.microsoft.com/en-us/aspnet/core/signalr/scale)
- [Nginx WebSocket Proxy](https://nginx.org/en/docs/http/websocket.html)
- [Apache mod_proxy_wstunnel](https://httpd.apache.org/docs/2.4/mod/mod_proxy_wstunnel.html)
