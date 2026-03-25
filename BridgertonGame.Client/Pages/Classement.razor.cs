using Microsoft.AspNetCore.Components;
using BridgertonGame.Client.Services;
using BridgertonGame.Shared.Models;

namespace BridgertonGame.Client.Pages
{
    public partial class Classement
    {
        [Inject] private ApiService ApiService { get; set; } = default!;

        private List<Family>? families;
        private List<GameScore>? gameScores;
        private List<Player>? players;
        private List<Article>? articles;
        private Dictionary<string, int> penalties = new();
        private List<FamilyVoteResult>? voteResults;
        private bool showModal = false;
        private string selectedImage = "";
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

        private class RankingEntry
        {
            public string Name { get; set; } = string.Empty;
            public int Points { get; set; }
            public int Rank { get; set; }
            public bool IsLadyWhistledownTeam { get; set; }
            public Family? Family { get; set; }
        }

        private List<RankingEntry> GetCompleteRanking()
        {
            if (families == null) return new();

            var entries = new List<RankingEntry>();

            // Ajouter toutes les familles
            foreach (var family in families)
            {
                entries.Add(new RankingEntry
                {
                    Name = family.Name,
                    Points = family.Points,
                    IsLadyWhistledownTeam = false,
                    Family = family
                });
            }

            // Ajouter l'équipe Lady Whistledown
            entries.Add(new RankingEntry
            {
                Name = "Équipe Lady Whistledown",
                Points = ladyWhistledownTeamPoints,
                IsLadyWhistledownTeam = true
            });

            // Trier par points décroissants puis par nom
            var sorted = entries
                .OrderByDescending(e => e.Points)
                .ThenBy(e => e.Name)
                .ToList();

            // Assigner les rangs en mode "dense" (1, 1, 2, 3...)
            int currentRank = 1;
            int? previousPoints = null;

            for (int i = 0; i < sorted.Count; i++)
            {
                if (previousPoints.HasValue && sorted[i].Points == previousPoints.Value)
                {
                    // Même score que le précédent, même rang
                    sorted[i].Rank = currentRank;
                }
                else
                {
                    // Nouveau score, incrémenter seulement de 1
                    if (i > 0)
                    {
                        currentRank++;
                    }
                    sorted[i].Rank = currentRank;
                }
                previousPoints = sorted[i].Points;
            }

            return sorted;
        }

        private int GetPlayerPoints(string familyId)
        {
            return penalties.ContainsKey(familyId) ? penalties[familyId] : 0;
        }

        private List<(Family Family, int Rank)> GetRankedWhistledowns()
        {
            if (families == null) return new();

            // Trier par points décroissants puis par nom
            var sorted = families
                .Select(f => new { Family = f, Points = GetPlayerPoints(f.Id) })
                .OrderByDescending(x => x.Points)
                .ThenBy(x => x.Family.Name)
                .ToList();

            // Assigner les rangs en mode "dense" (1, 1, 2, 3...)
            var ranked = new List<(Family Family, int Rank)>();
            int currentRank = 1;
            int? previousPoints = null;

            for (int i = 0; i < sorted.Count; i++)
            {
                if (previousPoints.HasValue && sorted[i].Points == previousPoints.Value)
                {
                    // Même score que le précédent, même rang
                    ranked.Add((sorted[i].Family, currentRank));
                }
                else
                {
                    // Nouveau score, incrémenter seulement de 1
                    if (i > 0)
                    {
                        currentRank++;
                    }
                    ranked.Add((sorted[i].Family, currentRank));
                }
                previousPoints = sorted[i].Points;
            }

            return ranked;
        }

        private string GetWhistledownRankClass(int rank) => rank switch
        {
            1 => "whistledown-rank-first",
            2 => "whistledown-rank-second",
            3 => "whistledown-rank-third",
            _ => ""
        };

        private string GetWhistledownRankBadgeClass(int rank) => rank switch
        {
            1 => "rank-badge-gold",
            2 => "rank-badge-silver",
            3 => "rank-badge-bronze",
            _ => "rank-badge-default"
        };

        private string GetRankText(int rank) => rank switch
        {
            1 => "1er",
            2 => "2eme",
            3 => "3eme",
            4 => "4eme",
            5 => "5eme",
            _ => $"{rank}eme"
        };

        private string GetRankingCardClass(int rank) => rank switch
        {
            1 => "rank-first",
            2 => "rank-second",
            3 => "rank-third",
            _ => ""
        };

        private string GetRowClass(string gameName) => gameName switch
        {
            "Total" => "total-row",
            "Pénalités Whistledown" => "whistledown-row",
            _ => ""
        };

        private string FormatScore(int score) => score == 0 ? "-" : $"{score}pts";

        private void ShowImageModal(string imageUrl)
        {
            selectedImage = imageUrl;
            showModal = true;
        }

        private void CloseImageModal()
        {
            showModal = false;
            selectedImage = "";
        }
    }
}
