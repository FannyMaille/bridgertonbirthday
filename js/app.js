// App JavaScript for The Bridgerton Game

document.addEventListener('DOMContentLoaded', function() {
    // Load and display articles on home page
    loadArticles();
});

function loadArticles() {
    const articlesList = document.getElementById('articles-list');
    if (!articlesList) return;

    const articles = DataService.getAllArticles();
    
    articlesList.innerHTML = articles.map(article => `
        <div class="article-card">
            <div class="article-border">
                <div class="article-content">
                    <p class="article-greeting">${article.title}</p>
                    <p class="article-text">"${article.content}"</p>
                    <div class="article-footer">
                        <span class="signature">Lady Whistledown</span>
                        <span class="family-name">Famille<br/>${article.familyName}</span>
                    </div>
                </div>
            </div>
        </div>
    `).join('');
}

// Utility function to format date
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('fr-FR', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

// Utility function to format time remaining
function formatTimeRemaining(milliseconds) {
    const minutes = Math.floor(milliseconds / (1000 * 60));
    const seconds = Math.floor((milliseconds % (1000 * 60)) / 1000);
    return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
}

// Get rank text
function getRankText(rank) {
    const ranks = {
        1: '1er',
        2: '2eme',
        3: '3eme',
        4: '4eme',
        5: '5eme'
    };
    return ranks[rank] || `${rank}eme`;
}
