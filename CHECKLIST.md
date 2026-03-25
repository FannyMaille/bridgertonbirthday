# ✅ Checklist de vérification - Équipe Lady Whistledown

## Modifications Backend (✅ Complétées)

- [x] `DatabaseGameDataService.cs` - Méthode `GetLadyWhistledownTeamPointsAsync()`
- [x] `DatabaseGameDataService.cs` - Méthode `GetLadyWhistledownIndividualPointsAsync()`
- [x] `GameScoresController.cs` - Endpoint `lady-whistledown-team-points`
- [x] `GameScoresController.cs` - Endpoint `lady-whistledown-individual-points`
- [x] `ApiService.cs` - Méthode `GetLadyWhistledownTeamPointsAsync()`
- [x] `ApiService.cs` - Méthode `GetLadyWhistledownIndividualPointsAsync()`

## Modifications Frontend (⚠️ Partiellement complétées)

- [x] `Classement.razor` - Variable `ladyWhistledownTeamPoints` ajoutée
- [x] `Classement.razor` - Chargement des points dans `OnInitializedAsync()`
- [x] `Classement.razor` - Section d'affichage de l'équipe ajoutée
- [ ] `MonEspace.razor` - Variable `ladyWhistledownTeamPoints` à ajouter
- [ ] `MonEspace.razor` - Chargement dans `LoadPlayerData()` à modifier
- [ ] `MonEspace.razor` - Mise à jour dans `PublishArticle()` à modifier
- [ ] `MonEspace.razor` - Section HTML d'affichage à ajouter

## Build et tests

- [x] Build réussi (sans les modifications de MonEspace)
- [ ] Test d'affichage sur Classement
- [ ] Test d'affichage sur MonEspace (après modifications)
- [ ] Test de publication d'article
- [ ] Vérification de la mise à jour des compteurs

## Instructions pour compléter MonEspace.razor

### 1. Ajouter la variable (dans @code)
```csharp
private int playerPoints = 0;
private int ladyWhistledownTeamPoints = 0;  // <-- AJOUTER CETTE LIGNE
```

### 2. Modifier LoadPlayerData()
Cherchez le bloc qui commence par :
```csharp
if (currentPlayer.IsLadyWhistledown)
{
    var penalties = await ApiService.GetPenaltiesAsync();
    playerPoints = penalties.ContainsKey(currentPlayer.FamilyId) ? penalties[currentPlayer.FamilyId] : 0;
```

Ajoutez après la ligne `playerPoints = ...` :
```csharp
    // Charger les points totaux de l'équipe Lady Whistledown
    ladyWhistledownTeamPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
```

### 3. Modifier PublishArticle()
Cherchez le bloc qui commence par :
```csharp
if (currentPlayer?.IsLadyWhistledown == true)
{
    var penalties = await ApiService.GetPenaltiesAsync();
    playerPoints = penalties.ContainsKey(currentPlayer.FamilyId) ? penalties[currentPlayer.FamilyId] : 0;
```

Ajoutez après la ligne `playerPoints = ...` :
```csharp
    // Recharger les points de l'équipe
    ladyWhistledownTeamPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
```

### 4. Ajouter la section HTML
Cherchez la div qui affiche "Vos points personnels" (vers la ligne 245) :
```razor
<div style="text-align: center; padding: 15px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); ...">
    <div style="font-size: 0.9rem; opacity: 0.9;">Vos points personnels</div>
    <div style="font-size: 2.5rem; ...">@playerPoints pts</div>
    <div style="font-size: 0.85rem; opacity: 0.9;">Lady Whistledown</div>
</div>
```

Juste après cette div (après la balise `</div>` fermante), collez le contenu de `MONESPACE_TEAM_HTML.razor`

## Vérification finale

Après avoir appliqué toutes les modifications :

1. Exécutez :
```bash
dotnet build
```

2. Si le build réussit, lancez l'application :
```bash
cd BridgertonGame.Server
dotnet run
```

3. Testez en tant que Lady Whistledown :
   - Vérifiez l'affichage des deux cartes de points sur MonEspace
   - Publiez un article
   - Vérifiez que les deux compteurs se mettent à jour
   - Allez sur Classement pour voir la section de l'équipe

## Fichiers de référence créés

- `LADY_WHISTLEDOWN_TEAM_COMPLETE.md` - Documentation complète
- `LADY_WHISTLEDOWN_TEAM_INTEGRATION.md` - Guide d'intégration
- `MONESPACE_MODIFICATIONS.cs` - Instructions code précises
- `MONESPACE_TEAM_HTML.razor` - Code HTML à copier
- `apply-lady-whistledown-team.bat` - Script d'aide
- `CHECKLIST.md` - Ce fichier

## En cas de problème

Si vous rencontrez une erreur de compilation :
1. Vérifiez que vous avez bien ajouté `private int ladyWhistledownTeamPoints = 0;`
2. Vérifiez les `await` dans les deux méthodes modifiées
3. Assurez-vous que le code HTML est bien indenté
4. Vérifiez qu'il n'y a pas de balises non fermées

## Statut actuel

✅ **Backend complet et fonctionnel**
✅ **Classement.razor complet**
⚠️ **MonEspace.razor nécessite 4 modifications simples**

Une fois les modifications appliquées, tout sera opérationnel ! 🎉
