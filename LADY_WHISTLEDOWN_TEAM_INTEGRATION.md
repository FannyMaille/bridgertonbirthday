# Équipe Lady Whistledown - Guide d'intégration

Ce guide explique comment intégrer la fonctionnalité d'équipe Lady Whistledown qui cumule uniquement les points de publication d'articles.

## Modifications à apporter

### 1. Dans `MonEspace.razor` - Section Publication

Après la section affichant les points personnels, ajoutez une nouvelle carte pour afficher les points de l'équipe Lady Whistledown :

```razor
<!-- Après la div affichant les points personnels -->
<div style="text-align: center; padding: 15px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; border-radius: 10px; margin-bottom: 15px;">
    <div style="font-size: 0.9rem; opacity: 0.9;">Vos points personnels</div>
    <div style="font-size: 2.5rem; font-weight: bold; font-family: 'Libre Baskerville', serif;">@playerPoints pts</div>
    <div style="font-size: 0.85rem; opacity: 0.9;">Lady Whistledown</div>
</div>

<!-- AJOUTER CETTE NOUVELLE SECTION -->
<div style="text-align: center; padding: 15px; background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); color: white; border-radius: 10px; margin-bottom: 15px; box-shadow: 0 4px 15px rgba(240, 147, 251, 0.3);">
    <div style="font-size: 0.9rem; opacity: 0.9;">Équipe Lady Whistledown</div>
    <div style="font-size: 2.5rem; font-weight: bold; font-family: 'Libre Baskerville', serif;">@ladyWhistledownTeamPoints pts</div>
    <div style="font-size: 0.85rem; opacity: 0.9;">Total des publications</div>
    <div style="margin-top: 10px; padding-top: 10px; border-top: 1px solid rgba(255,255,255,0.3); font-size: 0.85rem; opacity: 0.95;">
        📰 Points cumulés par toutes les Lady Whistledown
    </div>
</div>
```

### 2. Dans le code @code de `MonEspace.razor`

Ajoutez cette variable après `private int playerPoints = 0;` :

```csharp
private int playerPoints = 0;
private int ladyWhistledownTeamPoints = 0;
```

Puis, dans la méthode `LoadPlayerData()`, après le chargement des points individuels, ajoutez :

```csharp
// Charger les points de Lady Whistledown depuis les pénalités
if (currentPlayer.IsLadyWhistledown)
{
    var penalties = await ApiService.GetPenaltiesAsync();
    playerPoints = penalties.ContainsKey(currentPlayer.FamilyId) ? penalties[currentPlayer.FamilyId] : 0;
    
    // Charger les points totaux de l'équipe Lady Whistledown
    ladyWhistledownTeamPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
    
    await CheckCooldown();
}
```

Et dans `PublishArticle()`, après la mise à jour des points personnels :

```csharp
// Recharger les pénalités pour obtenir les points mis à jour
if (currentPlayer?.IsLadyWhistledown == true)
{
    var penalties = await ApiService.GetPenaltiesAsync();
    playerPoints = penalties.ContainsKey(currentPlayer.FamilyId) ? penalties[currentPlayer.FamilyId] : 0;
    
    // Recharger les points de l'équipe
    ladyWhistledownTeamPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
}
```

### 3. Dans `Classement.razor`

Ajoutez une nouvelle section pour afficher l'équipe Lady Whistledown après le tableau des points :

```razor
<!-- Après la section "Tableau des Points" -->
<div class="section-divider">
    <div class="flower-line">
        <img src="images/Ornement.png" alt="" />
    </div>
    <h3>Équipe<br/>Lady Whistledown</h3>
    <div class="flower-line">
        <img src="images/Ornement.png" alt="" />
    </div>
</div>

<div class="modern-ranking-section" style="max-width: 600px; margin: 0 auto;">
    <div class="ranking-card rank-first" style="background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); color: white;">
        <div class="ranking-card-inner" style="text-align: center; padding: 30px;">
            <div style="font-size: 3rem; margin-bottom: 15px;">👥</div>
            <h3 style="font-family: 'Libre Baskerville', serif; font-size: 1.8rem; margin-bottom: 10px;">Équipe Lady Whistledown</h3>
            <div style="font-size: 0.95rem; opacity: 0.9; margin-bottom: 20px;">
                Toutes les Lady Whistledown réunies
            </div>
            <div class="points-badge" style="background: rgba(255,255,255,0.2); padding: 20px; border-radius: 15px;">
                <span class="points-value" style="font-size: 3rem; color: white;">@ladyWhistledownTeamPoints</span>
                <span class="points-label" style="color: white; opacity: 0.9;">pts</span>
            </div>
            <div style="margin-top: 20px; padding-top: 20px; border-top: 1px solid rgba(255,255,255,0.3); font-size: 0.9rem; opacity: 0.95;">
                📰 Points cumulés par publications d'articles uniquement
            </div>
        </div>
    </div>
</div>
```

Dans le code @code de `Classement.razor`, ajoutez :

```csharp
private int ladyWhistledownTeamPoints = 0;

protected override async Task OnInitializedAsync()
{
    families = await ApiService.GetAllFamiliesAsync();
    gameScores = await ApiService.GetAllGameScoresAsync();
    players = await ApiService.GetAllPlayersAsync();
    articles = await ApiService.GetAllArticlesAsync();
    penalties = await ApiService.GetPenaltiesAsync();
    voteResults = await ApiService.GetAllVoteResultsAsync();
    
    // Charger les points de l'équipe Lady Whistledown
    ladyWhistledownTeamPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
}
```

## Points importants

1. **Les points de l'équipe Lady Whistledown** sont calculés en additionnant tous les points de publication (WhistledownPenalties) de toutes les familles
2. **Chaque publication d'article** ajoute 10 points à l'équipe globale
3. **Les points individuels** de chaque Lady Whistledown sont toujours affichés séparément
4. **L'équipe ne participe pas au classement** des familles, c'est une équipe à part

## Visualisation

- **MonEspace** : Affiche les points personnels ET les points de l'équipe pour chaque Lady Whistledown
- **Classement** : Affiche une section dédiée avec le total des points de l'équipe Lady Whistledown

## Exemple de calcul

Si nous avons :
- Lady Whistledown Famille A : 30 points (3 articles)
- Lady Whistledown Famille B : 50 points (5 articles)
- Lady Whistledown Famille C : 20 points (2 articles)

**Total de l'équipe** = 30 + 50 + 20 = **100 points**
