-- Script pour ajouter des questions de test au quiz Bridgerton
-- Exécutez ce script pour avoir des questions d'exemple

USE bridgerton;

-- Nettoyer les questions existantes (optionnel)
-- DELETE FROM QuizAnswers;
-- DELETE FROM Quizzes;

-- Question 1 : Facile - Famille Featherington
INSERT INTO Quizzes (QuestionNumber, Question, OptionA, OptionB, OptionC, OptionD, CorrectAnswer) 
VALUES (1, 'Dans quelle famille Penelope est-elle née ?', 'Bridgerton', 'Featherington', 'Sharma', 'Danbury', 'B')
ON DUPLICATE KEY UPDATE 
    Question = VALUES(Question),
    OptionA = VALUES(OptionA),
    OptionB = VALUES(OptionB),
    OptionC = VALUES(OptionC),
    OptionD = VALUES(OptionD),
    CorrectAnswer = VALUES(CorrectAnswer);

-- Question 2 : Moyenne - Lady Whistledown
INSERT INTO Quizzes (QuestionNumber, Question, OptionA, OptionB, OptionC, OptionD, CorrectAnswer) 
VALUES (2, 'Qui est la mystérieuse Lady Whistledown ?', 'Daphné Bridgerton', 'Kate Sharma', 'Penelope Featherington', 'Eloise Bridgerton', 'C')
ON DUPLICATE KEY UPDATE 
    Question = VALUES(Question),
    OptionA = VALUES(OptionA),
    OptionB = VALUES(OptionB),
    OptionC = VALUES(OptionC),
    OptionD = VALUES(OptionD),
    CorrectAnswer = VALUES(CorrectAnswer);

-- Question 3 : Moyenne - Lieu
INSERT INTO Quizzes (QuestionNumber, Question, OptionA, OptionB, OptionC, OptionD, CorrectAnswer) 
VALUES (3, 'Dans quelle ville se déroule principalement l''histoire ?', 'Paris', 'Londres', 'Vienne', 'Edinburgh', 'B')
ON DUPLICATE KEY UPDATE 
    Question = VALUES(Question),
    OptionA = VALUES(OptionA),
    OptionB = VALUES(OptionB),
    OptionC = VALUES(OptionC),
    OptionD = VALUES(OptionD),
    CorrectAnswer = VALUES(CorrectAnswer);

-- Question 4 : Difficile - Nombre d'enfants
INSERT INTO Quizzes (QuestionNumber, Question, OptionA, OptionB, OptionC, OptionD, CorrectAnswer) 
VALUES (4, 'Combien d''enfants Bridgerton y a-t-il au total ?', '6', '7', '8', '9', 'C')
ON DUPLICATE KEY UPDATE 
    Question = VALUES(Question),
    OptionA = VALUES(OptionA),
    OptionB = VALUES(OptionB),
    OptionC = VALUES(OptionC),
    OptionD = VALUES(OptionD),
    CorrectAnswer = VALUES(CorrectAnswer);

-- Question 5 : Facile - Reine
INSERT INTO Quizzes (QuestionNumber, Question, OptionA, OptionB, OptionC, OptionD, CorrectAnswer) 
VALUES (5, 'Comment s''appelle la reine de la haute société ?', 'Reine Victoria', 'Reine Charlotte', 'Reine Elizabeth', 'Reine Anne', 'B')
ON DUPLICATE KEY UPDATE 
    Question = VALUES(Question),
    OptionA = VALUES(OptionA),
    OptionB = VALUES(OptionB),
    OptionC = VALUES(OptionC),
    OptionD = VALUES(OptionD),
    CorrectAnswer = VALUES(CorrectAnswer);

-- Initialiser l'état du quiz (désactivé par défaut)
INSERT INTO QuizStates (IsEnabled, CurrentQuestionNumber) 
VALUES (0, 0)
ON DUPLICATE KEY UPDATE 
    IsEnabled = 0,
    CurrentQuestionNumber = 0;

-- Vérifier les questions insérées
SELECT 
    QuestionNumber as 'N°',
    LEFT(Question, 50) as 'Question',
    CorrectAnswer as 'Réponse'
FROM Quizzes 
ORDER BY QuestionNumber;

-- Afficher l'état actuel
SELECT * FROM QuizStates;

-- Compter les questions
SELECT COUNT(*) as 'Nombre total de questions' FROM Quizzes;
