# Ajout des Stats de l'Équipe Whistledown dans Mon Espace

## 1. Modifications dans le code @code

Ajouter ces variables après `private int playerPoints = 0;` :

```csharp
// Team Whistledown stats
private int teamWhistledownPoints = 0;
private int teamWhistledownRank = 0;
```

## 2. Modifier la méthode LoadPlayerData

Dans la section Lady Whistledown, ajouter après le chargement des pénalités :

```csharp
// Charger les points de Lady Whistledown depuis les pénalités
if (currentPlayer.IsLadyWhistledown)
{
    var penalties = await ApiService.GetPenaltiesAsync();
    playerPoints = penalties.ContainsKey(currentPlayer.FamilyId) ? penalties[currentPlayer.FamilyId] : 0;
    
    // Charger les stats de l'équipe Whistledown
    teamWhistledownPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
    teamWhistledownRank = await CalculateTeamWhistledownRank();
    
    await CheckCooldown();
}
```

## 3. Ajouter la méthode CalculateTeamWhistledownRank

Ajouter cette nouvelle méthode dans la section @code :

```csharp
private async Task<int> CalculateTeamWhistledownRank()
{
    try
    {
        var families = await ApiService.GetAllFamiliesAsync();
        var teamPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
        
        // Compter combien de familles ont plus de points
        var rank = 1;
        foreach (var family in families)
        {
            if (family.Points > teamPoints)
            {
                rank++;
            }
        }
        
        return rank;
    }
    catch
    {
        return 0;
    }
}
```

## 4. Ajouter l'affichage HTML

Juste AVANT la section "Les Autres Lady Whistledown" (avant le `@if (otherLadyWhistledowns.Any())`), ajouter :

```razor
<!-- Team Whistledown Stats Section -->
<div class="section-divider" style="margin-top: 40px;">
    <div class="flower-line"><img src="images/Ornement.png" alt="" /></div>
    <h3>Équipe<br/>Lady Whistledown</h3>
    <div class="flower-line"><img src="images/Ornement.png" alt="" /></div>
</div>

<div class="stats-grid" style="margin-bottom: 40px;">
    <div class="stat-card stat-rank" style="background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); color: white;">
        <div class="stat-icon">🏆</div>
        <div class="stat-content">
            <span class="stat-label" style="color: rgba(255,255,255,0.9);">Classement</span>
            <span class="stat-value">@GetRankText(teamWhistledownRank)</span>
        </div>
    </div>
    <div class="stat-card stat-points" style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white;">
        <div class="stat-icon">⭐</div>
        <div class="stat-content">
            <span class="stat-label" style="color: rgba(255,255,255,0.9);">Points</span>
            <span class="stat-value">@teamWhistledownPoints</span>
        </div>
    </div>
</div>
```

## Résumé des changements

1. **Variables ajoutées** : `teamWhistledownPoints` et `teamWhistledownRank`
2. **Méthode ajoutée** : `CalculateTeamWhistledownRank()` pour calculer le classement
3. **Chargement des données** : Dans `LoadPlayerData()` pour les Lady Whistledown
4. **Affichage** : Une section "stats-grid" identique à celle de la famille mais avec un style dégradé rose/violet pour l'équipe Whistledown
