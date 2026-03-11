-- Script pour comparer les points du classement vs tableau des scores
-- Ce script identifie les divergences

USE bridgerton_game;

-- 1. Voir les points dans la table Families (utilisés pour le classement)
SELECT 
    Id as FamilyId,
    Name as FamilyName,
    Points as PointsInRanking
FROM Families
ORDER BY Name;

-- 2. Calculer le total depuis GameScores (comme le fait GetAllGameScoresAsync)
SELECT 
    f.Id as FamilyId,
    f.Name as FamilyName,
    SUM(CASE WHEN gs.GameName != 'Total' THEN gs.Score ELSE 0 END) as CalculatedSubtotal,
    COALESCE(wp.Penalty, 0) as Penalties,
    SUM(CASE WHEN gs.GameName != 'Total' THEN gs.Score ELSE 0 END) - COALESCE(wp.Penalty, 0) as CalculatedTotal
FROM Families f
LEFT JOIN GameScores gs ON f.Id = gs.FamilyId
LEFT JOIN WhistledownPenalties wp ON f.Id = wp.FamilyId
GROUP BY f.Id, f.Name, wp.Penalty
ORDER BY f.Name;

-- 3. Voir tous les scores par jeu pour chaque famille
SELECT 
    f.Name as FamilyName,
    gs.GameName,
    gs.Score
FROM GameScores gs
INNER JOIN Families f ON gs.FamilyId = f.Id
ORDER BY f.Name, gs.GameName;

-- 4. Comparer côte à côte
SELECT 
    f.Id as FamilyId,
    f.Name as FamilyName,
    f.Points as PointsInRanking,
    (
        SELECT SUM(gs2.Score) 
        FROM GameScores gs2 
        WHERE gs2.FamilyId = f.Id 
          AND gs2.GameName != 'Total'
    ) - COALESCE(wp.Penalty, 0) as CalculatedTotal,
    f.Points - (
        (
            SELECT SUM(gs2.Score) 
            FROM GameScores gs2 
            WHERE gs2.FamilyId = f.Id 
              AND gs2.GameName != 'Total'
        ) - COALESCE(wp.Penalty, 0)
    ) as Difference
FROM Families f
LEFT JOIN WhistledownPenalties wp ON f.Id = wp.FamilyId
ORDER BY f.Name;
