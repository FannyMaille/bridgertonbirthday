using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BridgertonGame.Client.Services;
using BridgertonGame.Shared.Models;

namespace BridgertonGame.Client.Pages
{
    public partial class Personnages
    {
        [Inject] private ApiService ApiService { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private List<Family>? families;
        private List<Player>? players;
        private Player? hostPlayer;
        private bool showModal = false;
        private string selectedImage = "";

        protected override async Task OnInitializedAsync()
        {
            families = await ApiService.GetAllFamiliesAsync();
            players = await ApiService.GetAllPlayersAsync();
            // Chercher la Maîtresse de maison (peut avoir différents noms de rôle)
            hostPlayer = players?.FirstOrDefault(p => 
                p.Role == "Maîtresse de maison" || 
                p.Role == "Maîtresse de soirée");
        }

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
