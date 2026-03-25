using Microsoft.AspNetCore.Components;

namespace BridgertonGame.Client.Shared
{
    public partial class Hero
    {
        [Parameter]
        public string BackgroundImage { get; set; } = "images/hero-bg.jpg";

        [Parameter]
        public string? TitlePrefix { get; set; }

        [Parameter]
        public string Title { get; set; } = "";
    }
}
