// Admin Page JavaScript

let currentTab = 'scores';

// Check if admin is already logged in
document.addEventListener('DOMContentLoaded', function() {
    if (DataService.isAdminAuthenticated()) {
        showDashboard();
    }

    // Setup enter key on inputs
    const passwordInput = document.getElementById('password');
    if (passwordInput) {
        passwordInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                validateLogin();
            }
        });
    }
});

function validateLogin() {
    const username = document.getElementById('username').value.trim();
    const password = document.getElementById('password').value;
    const errorMessage = document.getElementById('error-message');

    if (DataService.validateAdmin(username, password)) {
        DataService.setAdminAuthenticated(true);
        errorMessage.textContent = '';
        showDashboard();
    } else {
        errorMessage.textContent = 'Login ou mot de passe incorrect';
    }
}

function showDashboard() {
    document.getElementById('login-section').style.display = 'none';
    document.getElementById('dashboard-section').style.display = 'block';
    loadScoresTab();
    loadArticlesTab();
    loadVotingTab();
    loadRevealTab();
}

function setTab(tab) {
    currentTab = tab;
    
    // Update tab buttons
    document.querySelectorAll('.tab-btn').forEach(btn => {
        btn.classList.remove('active');
        if (btn.dataset.tab === tab) {
            btn.classList.add('active');
        }
    });
    
    // Update tab panes
    document.querySelectorAll('.tab-pane').forEach(pane => {
        pane.style.display = 'none';
    });
    document.getElementById(`${tab}-tab`).style.display = 'block';
}

function loadScoresTab() {
    const families = DataService.getAllFamilies();
    const gameScores = DataService.getAllGameScores();
    
    // Update table header
    const scoresHeader = document.getElementById('scores-header');
    scoresHeader.innerHTML = '<th>Jeu</th>' +
        families.map(family => `<th>${family.name}</th>`).join('');
    
    // Update table body
    const scoresBody = document.getElementById('scores-body');
    scoresBody.innerHTML = gameScores
        .filter(gameScore => gameScore.gameName !== 'Total')
        .map(gameScore => `
            <tr>
                <td class="game-name">${gameScore.gameName}</td>
                ${families.map(family => `
                    <td>
                        <input type="number" 
                               class="score-input"
                               value="${gameScore.familyScores[family.id] || 0}"
                               onchange="updateScore('${gameScore.gameName}', '${family.id}', this.value)" />
                    </td>
                `).join('')}
            </tr>
        `).join('');
    
    // Load penalties
    const penaltiesGrid = document.getElementById('penalties-grid');
    const penalties = DataService.whistledownPenalties;
    penaltiesGrid.innerHTML = families.map(family => `
        <div class="penalty-item">
            <label>${family.name}</label>
            <input type="number" 
                   class="score-input"
                   value="${penalties[family.id] || 0}"
                   onchange="updatePenalty('${family.id}', this.value)" />
        </div>
    `).join('');
}

function loadArticlesTab() {
    const articles = DataService.getAllArticles();
    const articlesList = document.getElementById('admin-articles-list');
    
    articlesList.innerHTML = articles.map(article => `
        <div class="admin-article-card">
            <div class="article-info">
                <span class="article-family">${article.familyName}</span>
                <span class="article-date">${formatDate(article.publishedAt)}</span>
                <p class="article-preview">${article.content.substring(0, 100)}...</p>
            </div>
            <button class="delete-btn" onclick="deleteArticle('${article.id}')">
                Supprimer
            </button>
        </div>
    `).join('');
}

function loadVotingTab() {
    const families = DataService.getAllFamilies();
    const votingGrid = document.getElementById('voting-grid');
    
    votingGrid.innerHTML = families.map(family => `
        <div class="voting-item">
            <span class="family-label">${family.name}</span>
            <label class="toggle-switch">
                <input type="checkbox" 
                       ${family.votingEnabled ? 'checked' : ''}
                       onchange="toggleVoting('${family.id}', this.checked)" />
                <span class="toggle-slider"></span>
            </label>
        </div>
    `).join('');
}

function loadRevealTab() {
    const families = DataService.getAllFamilies();
    const allPlayers = DataService.getAllPlayers();
    
    const revealGrid = document.getElementById('admin-reveal-grid');
    revealGrid.innerHTML = families.map(family => {
        const ladyWhistledown = family.ladyWhistledownId ? 
            allPlayers.find(p => p.id === family.ladyWhistledownId) : null;
        
        return `
            <div class="reveal-item">
                <span class="family-label">${family.name}</span>
                ${family.revealed ? 
                    '<span class="revealed-badge">Révélé</span>' :
                    `<button class="reveal-btn" onclick="revealWhistledown('${family.id}')">
                        Révéler
                    </button>`
                }
            </div>
        `;
    }).join('');
}

function updateScore(gameName, familyId, value) {
    const points = parseInt(value) || 0;
    DataService.updateGameScore(gameName, familyId, points);
}

function updatePenalty(familyId, value) {
    const penalty = parseInt(value) || 0;
    DataService.updateWhistledownPenalty(familyId, penalty);
}

function deleteArticle(articleId) {
    if (confirm('Êtes-vous sûr de vouloir supprimer cet article ?')) {
        DataService.deleteArticle(articleId);
        loadArticlesTab();
    }
}

function toggleVoting(familyId, enabled) {
    DataService.toggleVoting(familyId, enabled);
}

function revealWhistledown(familyId) {
    DataService.revealLadyWhistledown(familyId);
    loadRevealTab();
}

function revealAll() {
    if (confirm('Êtes-vous sûr de vouloir révéler tous les Lady Whistledown ?')) {
        const families = DataService.getAllFamilies();
        families.forEach(family => {
            DataService.revealLadyWhistledown(family.id);
        });
        loadRevealTab();
    }
}

function logout() {
    DataService.setAdminAuthenticated(false);
    document.getElementById('login-section').style.display = 'block';
    document.getElementById('dashboard-section').style.display = 'none';
    document.getElementById('username').value = '';
    document.getElementById('password').value = '';
    document.getElementById('error-message').textContent = '';
}
