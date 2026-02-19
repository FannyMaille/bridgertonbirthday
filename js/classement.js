// Classement Page JavaScript

document.addEventListener('DOMContentLoaded', function() {
    loadRanking();
    loadPointsTable();
    loadRevealSection();
});

function loadRanking() {
    const families = DataService.getAllFamilies();
    const rankedFamilies = [...families].sort((a, b) => a.rank - b.rank);
    
    const rankingList = document.getElementById('ranking-list');
    rankingList.innerHTML = rankedFamilies.map(family => `
        <div class="ranking-item ${family.rank === 1 ? 'first' : ''}">
            ${family.rank === 1 ? '<img src="../images/Crown.png" alt="Crown" class="crown-icon" />' : ''}
            <span class="rank-number">${getRankText(family.rank)}</span>
            <span class="family-name">Famille ${family.name}</span>
            <span class="family-points">${family.points} pts</span>
        </div>
    `).join('');
}

function loadPointsTable() {
    const families = DataService.getAllFamilies();
    const gameScores = DataService.getAllGameScores();
    
    // Update table header
    const tableHeader = document.getElementById('table-header');
    tableHeader.innerHTML = '<th>Jeu</th>' +
        families.map(family => `<th>${family.name}</th>`).join('');
    
    // Update table body
    const tableBody = document.getElementById('table-body');
    tableBody.innerHTML = gameScores.map(gameScore => {
        const rowClass = getRowClass(gameScore.gameName);
        return `
            <tr class="${rowClass}">
                <td class="game-name">${gameScore.gameName}</td>
                ${families.map(family => {
                    const score = gameScore.familyScores[family.id] || 0;
                    const scoreClass = score < 0 ? 'negative' : '';
                    return `<td class="score ${scoreClass}">${formatScore(score)}</td>`;
                }).join('')}
            </tr>
        `;
    }).join('');
}

function loadRevealSection() {
    const families = DataService.getAllFamilies();
    const allPlayers = DataService.getAllPlayers();
    
    const revealGrid = document.getElementById('reveal-grid');
    revealGrid.innerHTML = families.map(family => {
        const ladyWhistledown = family.ladyWhistledownId ? 
            allPlayers.find(p => p.id === family.ladyWhistledownId) : null;
        
        return `
            <div class="reveal-card">
                <div class="reveal-frame">
                    <div class="ornament-frame">
                        <img src="../images/OrnementCadre.png" alt="" class="ornament-border" />
                        <div class="silhouette-container">
                            ${family.revealed && ladyWhistledown ? 
                                `<img src="${ladyWhistledown.imageUrl}" alt="${ladyWhistledown.name}" class="revealed-image" />` :
                                `<img src="../images/LadyWithldown.png" alt="Mystery" class="mystery-silhouette" />`
                            }
                        </div>
                    </div>
                </div>
                <div class="reveal-info">
                    <span class="reveal-label">FAMILLE</span>
                    ${family.revealed && ladyWhistledown ? 
                        `<span class="reveal-name">${ladyWhistledown.name}</span>` :
                        `<span class="reveal-mystery">??? ${family.name}</span>`
                    }
                    <span class="reveal-points">xx pts</span>
                </div>
            </div>
        `;
    }).join('');
}

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

function getRowClass(gameName) {
    if (gameName === 'Total') return 'total-row';
    if (gameName === 'Whistledown') return 'whistledown-row';
    return '';
}

function formatScore(score) {
    if (score === 0) return '-';
    return `${score}pts`;
}
