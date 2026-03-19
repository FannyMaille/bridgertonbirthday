-- Nettoyer les réponses existantes
DELETE FROM QuizAnswers;

-- Insérer des réponses de test pour chaque famille
-- Pour créer un classement varié

-- Question 1 : Toutes les familles répondent correctement
INSERT INTO QuizAnswers (PlayerId, QuestionNumber, SelectedAnswer, IsCorrect, AnsweredAt) VALUES
('s1', 1, 'A', 1, NOW()),
('b1', 1, 'A', 1, NOW()),
('h1', 1, 'A', 1, NOW()),
('f1', 1, 'A', 1, NOW()),
('d1', 1, 'A', 1, NOW());

-- Question 2 : Sharma, Bridgerton, Hastings répondent bien
INSERT INTO QuizAnswers (PlayerId, QuestionNumber, SelectedAnswer, IsCorrect, AnsweredAt) VALUES
('s1', 2, 'B', 1, NOW()),
('b1', 2, 'B', 1, NOW()),
('h1', 2, 'B', 1, NOW()),
('f1', 2, 'C', 0, NOW()),
('d1', 2, 'D', 0, NOW());

-- Question 3 : Sharma, Bridgerton répondent bien
INSERT INTO QuizAnswers (PlayerId, QuestionNumber, SelectedAnswer, IsCorrect, AnsweredAt) VALUES
('s1', 3, 'C', 1, NOW()),
('b1', 3, 'C', 1, NOW()),
('h1', 3, 'A', 0, NOW()),
('f1', 3, 'B', 0, NOW()),
('d1', 3, 'D', 0, NOW());

-- Question 4 : Sharma, Bridgerton, Featherington répondent bien
INSERT INTO QuizAnswers (PlayerId, QuestionNumber, SelectedAnswer, IsCorrect, AnsweredAt) VALUES
('s1', 4, 'D', 1, NOW()),
('b1', 4, 'D', 1, NOW()),
('h1', 4, 'A', 0, NOW()),
('f1', 4, 'D', 1, NOW()),
('d1', 4, 'B', 0, NOW());

-- Question 5 : Sharma, Hastings répondent bien
INSERT INTO QuizAnswers (PlayerId, QuestionNumber, SelectedAnswer, IsCorrect, AnsweredAt) VALUES
('s1', 5, 'A', 1, NOW()),
('b1', 5, 'C', 0, NOW()),
('h1', 5, 'A', 1, NOW()),
('f1', 5, 'B', 0, NOW()),
('d1', 5, 'D', 0, NOW());

-- Question 6 : Sharma, Bridgerton, Hastings, Featherington répondent bien
INSERT INTO QuizAnswers (PlayerId, QuestionNumber, SelectedAnswer, IsCorrect, AnsweredAt) VALUES
('s1', 6, 'B', 1, NOW()),
('b1', 6, 'B', 1, NOW()),
('h1', 6, 'B', 1, NOW()),
('f1', 6, 'B', 1, NOW()),
('d1', 6, 'A', 0, NOW());

-- Résultats attendus :
-- Sharma : 6/6 = 100% (VERT)
-- Bridgerton : 5/6 = 83% (VERT)
-- Hastings : 4/6 = 67% (JAUNE)
-- Featherington : 3/6 = 50% (ORANGE)
-- Danbury : 1/6 = 17% (ROUGE)

SELECT 
    f.Name as Famille,
    COUNT(*) as TotalReponses,
    SUM(CASE WHEN qa.IsCorrect = 1 THEN 1 ELSE 0 END) as BonnesReponses,
    CONCAT(
        SUM(CASE WHEN qa.IsCorrect = 1 THEN 1 ELSE 0 END), 
        '/', 
        COUNT(*)
    ) as Score,
    CONCAT(
        ROUND(SUM(CASE WHEN qa.IsCorrect = 1 THEN 1 ELSE 0 END) * 100.0 / COUNT(*), 0),
        '%'
    ) as TauxReussite
FROM QuizAnswers qa
JOIN Players p ON qa.PlayerId = p.Id
JOIN Families f ON p.FamilyId = f.Id
GROUP BY f.Name
ORDER BY SUM(CASE WHEN qa.IsCorrect = 1 THEN 1 ELSE 0 END) DESC;
