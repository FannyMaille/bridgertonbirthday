-- Script de synchronisation Lady Whistledown
-- Ce script synchronise les rôles Lady Whistledown entre Players et Families

-- Étape 1: Réinitialiser tous les joueurs (retirer Lady Whistledown de tous)
UPDATE Players 
SET IsLadyWhistledown = 0, 
    Role = CASE 
        WHEN Role = 'Lady Whistledown' AND Role != 'Maîtresse de maison' AND Role != 'Maîtresse de soirée' THEN 'Invité(e)'
        ELSE Role
    END;

-- Étape 2: Mettre à jour les joueurs désignés comme Lady Whistledown dans les familles
UPDATE Players p
INNER JOIN Families f ON p.Id = f.LadyWhistledownId
SET p.IsLadyWhistledown = 1,
    p.Role = 'Lady Whistledown'
WHERE f.LadyWhistledownId IS NOT NULL;

-- Vérification des résultats
SELECT 
    f.Name AS Famille,
    p.Name AS 'Lady Whistledown',
    p.Role AS Role,
    p.IsLadyWhistledown AS 'Est LW'
FROM Families f
LEFT JOIN Players p ON f.LadyWhistledownId = p.Id
ORDER BY f.Name;

-- Afficher tous les Lady Whistledown
SELECT 
    Name,
    Title,
    Role,
    IsLadyWhistledown,
    FamilyId
FROM Players
WHERE IsLadyWhistledown = 1;
