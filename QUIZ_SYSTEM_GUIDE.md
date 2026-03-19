# 📝 Système de Quiz - Guide Complet

## Vue d'ensemble

Le système de quiz a été ajouté avec succès à votre application Bridgerton. Il permet aux administrateurs de créer et gérer des quiz interactifs que les joueurs peuvent compléter depuis leur espace personnel.

## 🎯 Fonctionnalités

### Pour l'administrateur

1. **Gestion des questions**
   - ➕ Créer des questions avec 4 options (A, B, C, D)
   - ✏️ Modifier les questions existantes
   - 🗑️ Supprimer des questions
   - 📊 Voir les statistiques de réponses en temps réel

2. **Contrôle du quiz**
   - ⚙️ Activer/Désactiver le quiz
   - 🔢 Sélectionner quelle question afficher aux joueurs
   - 📈 Suivre les taux de réponses

3. **Statistiques détaillées**
   - Nombre total de réponses par question
   - Distribution des réponses (A, B, C, D) en pourcentage
   - Visualisation graphique des réponses

### Pour les joueurs

1. **Accès au quiz**
   - Le quiz apparaît automatiquement dans "Mon Espace" quand il est activé
   - Une seule réponse par question autorisée
   - Interface intuitive avec sélection visuelle

2. **Feedback immédiat**
   - Confirmation de la réponse envoyée
   - Affichage de la réponse déjà donnée pour éviter les doublons

## 📚 Guide d'utilisation Admin

### 1. Créer une question

1. Aller dans **Admin > Quiz**
2. Cliquer sur **"➕ Ajouter une question"**
3. Remplir le formulaire :
   - **Numéro de la question** : Numéro unique (1, 2, 3...)
   - **Question** : Le texte de la question
   - **Options A, B, C, D** : Les 4 réponses possibles
   - **Bonne réponse** : Sélectionner A, B, C ou D
4. Cliquer sur **"➕ Créer"**

### 2. Activer le quiz

1. Dans la section **"Contrôle du Quiz"**
2. Activer le toggle **"Quiz activé"**
3. Sélectionner la question à afficher dans le menu déroulant
4. Les joueurs voient immédiatement la question dans leur espace

### 3. Changer de question

1. Dans la section **"Contrôle du Quiz"**
2. Sélectionner une autre question dans **"Question Affichée"**
3. La nouvelle question s'affiche automatiquement pour tous les joueurs
4. Les joueurs qui ont déjà répondu voient leur réponse précédente

### 4. Consulter les statistiques

1. Descendre jusqu'à la section **"📊 Statistiques des Réponses"**
2. Voir pour chaque question :
   - Nombre total de réponses
   - Répartition en pourcentage des options A, B, C, D
   - Barres de progression visuelles
   - La bonne réponse est mise en évidence en vert

### 5. Modifier une question

1. Cliquer sur le bouton **✏️** sur la carte de la question
2. Modifier les informations souhaitées
3. Cliquer sur **"💾 Enregistrer"**
4. ⚠️ Les réponses existantes ne sont pas supprimées

### 6. Supprimer une question

1. Cliquer sur le bouton **🗑️** sur la carte de la question
2. Confirmer la suppression
3. ⚠️ **Toutes les réponses associées seront également supprimées**

## 🎮 Guide d'utilisation Joueur

### Répondre à une question

1. Se connecter à "Mon Espace" avec son code
2. Si un quiz est actif, il apparaît automatiquement dans l'espace personnel
3. Lire attentivement la question
4. Cliquer sur l'une des 4 options (A, B, C, D)
5. L'option sélectionnée s'illumine en bleu
6. Cliquer sur **"Valider ma réponse"**
7. Un message de confirmation s'affiche

### Voir sa réponse

- Si vous avez déjà répondu à la question actuelle
- Un encadré bleu affiche votre réponse précédente
- Vous ne pouvez pas modifier votre réponse
- Quand l'admin change de question, vous pouvez répondre à la nouvelle

## 🗄️ Structure de la base de données

Trois nouvelles tables ont été créées :

### `Quizzes`
- `Id` : Identifiant unique
- `QuestionNumber` : Numéro de la question (unique)
- `Question` : Texte de la question
- `OptionA`, `OptionB`, `OptionC`, `OptionD` : Les 4 options
- `CorrectAnswer` : La bonne réponse (A, B, C ou D)

### `QuizAnswers`
- `Id` : Identifiant unique
- `PlayerId` : ID du joueur
- `QuestionNumber` : Numéro de la question
- `SelectedAnswer` : Réponse choisie (A, B, C ou D)
- `IsCorrect` : Si la réponse est correcte
- `AnsweredAt` : Date et heure de la réponse

### `QuizStates`
- `Id` : Identifiant unique
- `IsEnabled` : Si le quiz est actif
- `CurrentQuestionNumber` : Numéro de la question affichée

## 🔌 API Endpoints

### Admin
- `GET /api/quiz/state` - Obtenir l'état du quiz
- `PUT /api/quiz/state` - Mettre à jour l'état
- `GET /api/quiz/questions` - Liste toutes les questions
- `POST /api/quiz/questions` - Créer une question
- `PUT /api/quiz/questions/{id}` - Modifier une question
- `DELETE /api/quiz/questions/{id}` - Supprimer une question
- `GET /api/quiz/statistics/{questionNumber}` - Stats d'une question
- `GET /api/quiz/all-statistics` - Stats de toutes les questions

### Joueurs
- `GET /api/quiz/current` - Obtenir la question actuelle
- `POST /api/quiz/answer` - Soumettre une réponse
- `GET /api/quiz/player-answer/{playerId}/{questionNumber}` - Vérifier si répondu

## 💡 Conseils d'utilisation

### Pour un quiz progressif
1. Créer toutes les questions à l'avance (Question 1, 2, 3...)
2. Activer le quiz avec la Question 1
3. Attendre que tout le monde réponde
4. Passer à la Question 2, etc.

### Pour un quiz libre
1. Activer le quiz
2. Les joueurs répondent à leur rythme
3. Consulter les stats régulièrement
4. Changer de question quand vous le souhaitez

### Bonnes pratiques
- ✅ Tester une question avant de l'activer
- ✅ Vérifier que la bonne réponse est correcte
- ✅ Utiliser des numéros de questions séquentiels (1, 2, 3...)
- ✅ Consulter les stats avant de passer à la question suivante
- ❌ Ne pas supprimer une question active
- ❌ Ne pas désactiver le quiz pendant que les joueurs répondent

## 🎨 Personnalisation

### Couleurs
Les couleurs du quiz sont définies dans :
- `mon-espace.css` pour l'interface joueur
- `admin.css` pour l'interface admin

### Nombre d'options
Actuellement fixé à 4 (A, B, C, D). Pour modifier, il faut ajuster :
- Les modèles `Quiz.cs`
- Le contrôleur `QuizController.cs`
- Les formulaires admin et joueur

## 🐛 Résolution de problèmes

### Le quiz n'apparaît pas pour les joueurs
- Vérifier que le quiz est activé dans l'admin
- Vérifier qu'une question est sélectionnée (CurrentQuestionNumber > 0)
- Vérifier que le joueur n'est pas une "Maîtresse de maison"

### Les statistiques ne s'affichent pas
- Vérifier que des réponses ont été soumises
- Rafraîchir la page admin
- Vérifier la console pour les erreurs

### Impossible de créer une question
- Vérifier que le numéro de question n'existe pas déjà
- Vérifier que tous les champs sont remplis
- Vérifier qu'une bonne réponse est sélectionnée

## 📞 Support

Pour toute question ou problème :
1. Vérifier les logs du serveur
2. Vérifier la console du navigateur
3. Consulter ce guide
4. Contacter le développeur

## 🚀 Évolutions futures possibles

- [ ] Quiz à choix multiples (plusieurs réponses correctes)
- [ ] Timer par question
- [ ] Scores et classements
- [ ] Images dans les questions
- [ ] Mode "blind test"
- [ ] Export des résultats en CSV/PDF
- [ ] Questions aléatoires
- [ ] Différents types de questions (vrai/faux, texte libre)

---

**Version** : 1.0  
**Date** : Mars 2026  
**Auteur** : GitHub Copilot
