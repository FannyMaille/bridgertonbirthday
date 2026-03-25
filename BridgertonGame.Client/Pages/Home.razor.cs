using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BridgertonGame.Client.Services;
using BridgertonGame.Shared.Models;

namespace BridgertonGame.Client.Pages
{
    public partial class Home : IDisposable
    {
        [Inject] private ApiService ApiService { get; set; } = default!;
        [Inject] private NotificationService NotificationService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;

        private List<Article>? articles;
        private string? highlightedArticleId;

        protected override async Task OnInitializedAsync()
        {
            articles = await ApiService.GetAllArticlesAsync();
            
            // S'abonner aux notifications pour rafraîchir automatiquement
            NotificationService.OnNotificationReceived += HandleNewNotification;
            NotificationService.OnArticleDeleted += HandleArticleDeleted;
            
            // Vérifier si on doit scroller vers un article spécifique
            var uri = new Uri(Navigation.Uri);
            if (uri.Fragment.StartsWith("#article-"))
            {
                highlightedArticleId = uri.Fragment.Replace("#article-", "");
                await ScrollToArticle(highlightedArticleId);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender && highlightedArticleId != null)
            {
                await ScrollToArticle(highlightedArticleId);
                highlightedArticleId = null;
            }
        }

        private async Task HandleNewNotification(Notification notification)
        {
            // Recharger les articles uniquement si c'est une notification d'article
            if (notification.Type == "article" && !string.IsNullOrEmpty(notification.ArticleId))
            {
                await InvokeAsync(async () =>
                {
                    articles = await ApiService.GetAllArticlesAsync();
                    highlightedArticleId = notification.ArticleId;
                    StateHasChanged();
                });
            }
        }

        private async Task HandleArticleDeleted(string articleId)
        {
            await InvokeAsync(async () =>
            {
                articles = await ApiService.GetAllArticlesAsync();
                StateHasChanged();
            });
        }

        private async Task ScrollToArticle(string articleId)
        {
            try
            {
                await Task.Delay(100); // Petit délai pour s'assurer que le DOM est rendu
                await JS.InvokeVoidAsync("scrollToElement", $"article-{articleId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur scroll: {ex.Message}");
            }
        }

        private string GetArticleClass(Article article)
        {
            return article.Id == highlightedArticleId ? "article-highlight" : "";
        }

        public void Dispose()
        {
            NotificationService.OnNotificationReceived -= HandleNewNotification;
            NotificationService.OnArticleDeleted -= HandleArticleDeleted;
        }
    }
}
