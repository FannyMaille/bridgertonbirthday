@echo off
echo ========================================
echo Verification Rapide des Votes
echo ========================================
echo.

echo Verification en cours...
echo.

cd BridgertonGame.Server

mysql -h localhost -P 3306 -u root -p -D bridgerton -e "SELECT f.Name as Famille, CASE WHEN f.VotingEnabled = 1 THEN 'OUI' ELSE 'NON' END as VoteActive, CASE WHEN f.Revealed = 1 THEN 'OUI' ELSE 'NON' END as Revele, p.Name as LadyWhistledown, (SELECT COUNT(*) FROM Votes v WHERE v.FamilyId = f.Id) as NbVotes FROM Families f LEFT JOIN Players p ON f.LadyWhistledownId = p.Id WHERE f.Name = 'Bridgerton';"

echo.
echo ========================================
echo.
echo IMPORTANT :
echo - Si Revele = NON : Les votes ne sont PAS encore comptabilises (NORMAL)
echo - Si Revele = OUI : Les votes DOIVENT etre comptabilises
echo.
echo Pour reveler : Admin ^> Onglet Revelations ^> Toggle ON pour Bridgerton
echo.
pause
