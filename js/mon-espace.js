// Mon Espace Page JavaScript

let currentPlayer = null;
let currentFamily = null;
let familyMembers = [];
let showRole = false;
let selectedMemberId = '';
let articleContent = '';

// Check if user is already logged in
document.addEventListener('DOMContentLoaded', function() {
    currentPlayer = DataService.getCurrentPlayer();
    if (currentPlayer) {
        showProfileSection();
    }

    // Setup character counter
    const articleEditor = document.getElementById('article-content');
    if (articleEditor) {
        articleEditor.addEventListener('input', function() {
            articleContent = this.value;
            document.getElementById('char-count').textContent = `${articleContent.length}/300 charactères`;
            updatePublishButton();
        });
    }

    // Setup member select
    const memberSelect = document.getElementById('member-select');
    if (memberSelect) {
        memberSelect.addEventListener('change', function() {
            selectedMemberId = this.value;
            document.getElementById('confirm-vote-btn').disabled = !selectedMemberId;
        });
    }

    // Setup enter key on code input
    const codeInput = document.getElementById('player-code');
    if (codeInput) {
        codeInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                validateCode();
            }
        });
    }
});

function validateCode() {
    const codeInput = document.getElementById('player-code');
    const errorMessage = document.getElementById('error-message');
    const code = codeInput.value.trim();

    if (!code) {
        errorMessage.textContent = 'Veuillez entrer un code';
        return;
    }

    const player = DataService.getPlayerByCode(code);
    if (player) {
        currentPlayer = player;
        DataService.setCurrentPlayer(player);
        errorMessage.textContent = '';
        showProfileSection();
    } else {
        errorMessage.textContent = 'Code invalide. Veuillez réessayer.';
    }
}

function showProfileSection() {
    document.getElementById('login-section').style.display = 'none';
    document.getElementById('profile-section').style.display = 'block';

    // Load player data
    document.getElementById('player-title').textContent = currentPlayer.title;
    document.getElementById('player-name').textContent = currentPlayer.name;
    document.getElementById('player-image').src = currentPlayer.imageUrl;
    document.getElementById('player-image').alt = currentPlayer.name;

    // Load family data
    currentFamily = DataService.getFamilyById(currentPlayer.familyId);
    familyMembers = DataService.getPlayersByFamily(currentPlayer.familyId);

    // Update family info
    document.getElementById('family-subtitle').textContent = `Les points et le classement de ma Famille ${currentFamily.name}`;
    document.getElementById('family-rank').textContent = `${currentFamily.rank}°`;
    document.getElementById('family-points').textContent = currentFamily.points;

    // Update members title
    document.getElementById('members-title').innerHTML = `Les Membres de<br/>Ma Famille ${currentFamily.name}`;

    // Load family members
    loadFamilyMembers();

    // Setup voting section
    setupVotingSection();

    // Setup publication section (Lady Whistledown only)
    setupPublicationSection();
}

function loadFamilyMembers() {
    const membersGrid = document.getElementById('members-grid');
    membersGrid.innerHTML = familyMembers.map(member => `
        <div class="member-card">
            <div class="ornament-frame member-frame">
                <img src="../images/OrnementCadre.png" alt="" class="ornament-border" />
                <div class="member-image-container">
                    <img src="${member.imageUrl}" alt="${member.name}" class="member-image" />
                </div>
            </div>
            <div class="member-title">${member.title}</div>
            <div class="member-name">${member.name}</div>
        </div>
    `).join('');
}

function setupVotingSection() {
    const votingSection = document.getElementById('voting-section');
    
    if (currentFamily.votingEnabled) {
        votingSection.style.display = 'block';
        
        // Populate member select
        const memberSelect = document.getElementById('member-select');
        memberSelect.innerHTML = '<option value="">Selectionne un nom</option>' +
            familyMembers.map(member => `
                <option value="${member.id}">${member.name}</option>
            `).join('');
    } else {
        votingSection.style.display = 'none';
    }
}

function setupPublicationSection() {
    const publicationSection = document.getElementById('publication-section');
    
    if (currentPlayer.isLadyWhistledown) {
        publicationSection.style.display = 'block';
        document.getElementById('editor-family-name').innerHTML = `Famille<br/>${currentFamily.name}`;
        updatePublishButton();
        checkCooldown();
    } else {
        publicationSection.style.display = 'none';
    }
}

function toggleRoleVisibility() {
    showRole = !showRole;
    const roleText = document.getElementById('role-text');
    const eyeIcon = document.getElementById('eye-icon');
    
    if (showRole) {
        roleText.textContent = currentPlayer.role;
        eyeIcon.textContent = '🙈';
    } else {
        roleText.textContent = '*******';
        eyeIcon.textContent = '👁️';
    }
}

function confirmVote() {
    if (selectedMemberId) {
        DataService.setLadyWhistledown(currentFamily.id, selectedMemberId);
        alert('Votre vote a été enregistré !');
        document.getElementById('confirm-vote-btn').disabled = true;
    }
}

function updatePublishButton() {
    const canPublish = DataService.canPublish(currentFamily.id);
    const hasContent = articleContent.trim().length > 0;
    document.getElementById('publish-btn').disabled = !canPublish || !hasContent;
}

function checkCooldown() {
    const cooldownMessage = document.getElementById('cooldown-message');
    const timeRemaining = DataService.getTimeUntilNextPublication(currentFamily.id);
    
    if (timeRemaining) {
        cooldownMessage.style.display = 'block';
        cooldownMessage.textContent = `Prochaine publication possible dans: ${formatTimeRemaining(timeRemaining)}`;
        updatePublishButton();
        
        // Update countdown every second
        setTimeout(checkCooldown, 1000);
    } else {
        cooldownMessage.style.display = 'none';
        updatePublishButton();
    }
}

function publishArticle() {
    if (articleContent.trim() && DataService.canPublish(currentFamily.id)) {
        DataService.publishArticle('Chers amis lecteurs,', articleContent, currentFamily.id, currentFamily.name);
        alert('Votre article a été publié !');
        articleContent = '';
        document.getElementById('article-content').value = '';
        document.getElementById('char-count').textContent = '0/300 charactères';
        checkCooldown();
    }
}

function logout() {
    DataService.setCurrentPlayer(null);
    currentPlayer = null;
    currentFamily = null;
    showRole = false;
    selectedMemberId = '';
    articleContent = '';
    
    document.getElementById('login-section').style.display = 'block';
    document.getElementById('profile-section').style.display = 'none';
    document.getElementById('player-code').value = '';
    document.getElementById('error-message').textContent = '';
}
