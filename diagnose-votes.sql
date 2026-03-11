-- Script de diagnostic pour les votes
-- Vérifier l'état des votes et de la révélation

USE bridgerton;

-- 1. Vérifier la configuration de la famille Bridgerton
SELECT 
    f.Id as FamilyId,
    f.Name as FamilyName,
    f.VotingEnabled,
    f.Revealed,
    f.LadyWhistledownId,
    p.Name as LadyWhistledownName
FROM Families f
LEFT JOIN Players p ON f.LadyWhistledownId = p.Id
WHERE f.Name = 'Bridgerton';

-- 2. Vérifier tous les votes pour la famille Bridgerton
SELECT 
    v.Id,
    v.FamilyId,
    voter.Name as VoterName,
    votedFor.Name as VotedForName,
    v.VotedAt
FROM Votes v
INNER JOIN Players voter ON v.VoterId = voter.Id
INNER JOIN Players votedFor ON v.VotedForId = votedFor.Id
WHERE v.FamilyId = 'bridgerton';

-- 3. Vérifier les résultats de votes (si révélé)
SELECT 
    vr.Id,
    vr.FamilyId,
    vr.CorrectVotes,
    vr.IncorrectVotes,
    vr.PointsAwarded,
    vr.RevealedAt
FROM VoteResults vr
WHERE vr.FamilyId = 'bridgerton';

-- 4. Vérifier les scores de votes dans GameScores
SELECT 
    gs.Id,
    gs.GameName,
    gs.FamilyId,
    gs.Score
FROM GameScores gs
WHERE gs.GameName = 'Votes Lady Whistledown' 
  AND gs.FamilyId = 'bridgerton';

-- 5. Vérifier tous les joueurs de la famille Bridgerton
SELECT 
    p.Id,
    p.Name,
    p.IsLadyWhistledown,
    p.FamilyId
FROM Players p
WHERE p.FamilyId = 'bridgerton';
