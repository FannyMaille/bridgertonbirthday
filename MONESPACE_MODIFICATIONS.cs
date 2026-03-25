// ============================================================
// MODIFICATIONS À APPORTER DANS MonEspace.razor
// ============================================================

// 1. Dans le @code, ajouter après "private int playerPoints = 0;" :
private int ladyWhistledownTeamPoints = 0;

// 2. Dans la méthode LoadPlayerData(), remplacer la section de chargement des points Lady Whistledown par :

// Charger les points de Lady Whistledown depuis les pénalités
if (currentPlayer.IsLadyWhistledown)
{
    var penalties = await ApiService.GetPenaltiesAsync();
    playerPoints = penalties.ContainsKey(currentPlayer.FamilyId) ? penalties[currentPlayer.FamilyId] : 0;
    
    // Charger les points totaux de l'équipe Lady Whistledown
    ladyWhistledownTeamPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
    
    await CheckCooldown();
}

// 3. Dans PublishArticle(), remplacer la section de rechargement des points par :

// Recharger les pénalités pour obtenir les points mis à jour
if (currentPlayer?.IsLadyWhistledown == true)
{
    var penalties = await ApiService.GetPenaltiesAsync();
    playerPoints = penalties.ContainsKey(currentPlayer.FamilyId) ? penalties[currentPlayer.FamilyId] : 0;
    
    // Recharger les points de l'équipe
    ladyWhistledownTeamPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
}

// 4. Dans le HTML, après la div affichant "Vos points personnels", ajouter :

<div style="text-align: center; padding: 15px; background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); color: white; border-radius: 10px; margin-bottom: 15px; box-shadow: 0 4px 15px rgba(240, 147, 251, 0.3);">
    <div style="font-size: 0.9rem; opacity: 0.9;">Équipe Lady Whistledown</div>
    <div style="font-size: 2.5rem; font-weight: bold; font-family: 'Libre Baskerville', serif;">@ladyWhistledownTeamPoints pts</div>
    <div style="font-size: 0.85rem; opacity: 0.9;">Total des publications</div>
    <div style="margin-top: 10px; padding-top: 10px; border-top: 1px solid rgba(255,255,255,0.3); font-size: 0.85rem; opacity: 0.95;">
        📰 Points cumulés par toutes les Lady Whistledown
    </div>
</div>
