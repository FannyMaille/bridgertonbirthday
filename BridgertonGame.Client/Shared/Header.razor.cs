using Microsoft.AspNetCore.Components;
using BridgertonGame.Client.Services;

namespace BridgertonGame.Client.Shared
{
    public partial class Header
    {
        [Inject] private NotificationService NotificationService { get; set; } = default!;

        private bool isMenuOpen = false;

        private void ToggleMenu()
        {
            isMenuOpen = !isMenuOpen;
        }

        private void CloseMenu()
        {
            isMenuOpen = false;
        }
    }
}
