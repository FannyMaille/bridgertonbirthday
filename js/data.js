// Data for The Bridgerton Game

const GAME_DATA = {
    // Players data
    players: [
        // Hastings Family
        { id: 'h1', code: 'CELIA2024', name: 'Célia Hastings', title: 'DUCHESSE', imageUrl: 'images/AdminAvatar.png', role: 'Lady Whistledown', familyId: 'hastings', isLadyWhistledown: true },
        { id: 'h2', code: 'FANNY2024', name: 'Fanny Hastings', title: 'DUCHESSE', imageUrl: 'images/AdminAvatar.png', role: 'Invitée', familyId: 'hastings', isLadyWhistledown: false },
        { id: 'h3', code: 'HUGO2024', name: 'Hugo Hastings', title: 'DUC', imageUrl: 'images/AdminAvatar.png', role: 'Invité', familyId: 'hastings', isLadyWhistledown: false },
        
        // Bridgerton Family
        { id: 'b1', code: 'DAPHNE2024', name: 'Daphné Bridgerton', title: 'DUCHESSE', imageUrl: 'images/AdminAvatar.png', role: 'Lady Whistledown', familyId: 'bridgerton', isLadyWhistledown: true },
        { id: 'b2', code: 'SIMON2024', name: 'Simon Bridgerton', title: 'DUC', imageUrl: 'images/AdminAvatar.png', role: 'Invité', familyId: 'bridgerton', isLadyWhistledown: false },
        { id: 'b3', code: 'ELOISE2024', name: 'Eloïse Bridgerton', title: 'LADY', imageUrl: 'images/AdminAvatar.png', role: 'Invitée', familyId: 'bridgerton', isLadyWhistledown: false },
        
        // Featherington Family
        { id: 'f1', code: 'PENELOPE2024', name: 'Penelope Featherington', title: 'LADY', imageUrl: 'images/AdminAvatar.png', role: 'Lady Whistledown', familyId: 'featherington', isLadyWhistledown: true },
        { id: 'f2', code: 'PORTIA2024', name: 'Portia Featherington', title: 'LADY', imageUrl: 'images/AdminAvatar.png', role: 'Invitée', familyId: 'featherington', isLadyWhistledown: false },
        
        // Danbury Family
        { id: 'd1', code: 'AGATHA2024', name: 'Agatha Danbury', title: 'LADY', imageUrl: 'images/AdminAvatar.png', role: 'Maîtresse de soirée', familyId: 'danbury', isLadyWhistledown: false },
        { id: 'd2', code: 'WILL2024', name: 'Will Danbury', title: 'LORD', imageUrl: 'images/AdminAvatar.png', role: 'Invité', familyId: 'danbury', isLadyWhistledown: false },
        
        // Sharma Family
        { id: 's1', code: 'KATE2024', name: 'Kate Sharma', title: 'LADY', imageUrl: 'images/AdminAvatar.png', role: 'Lady Whistledown', familyId: 'sharma', isLadyWhistledown: true },
        { id: 's2', code: 'EDWINA2024', name: 'Edwina Sharma', title: 'LADY', imageUrl: 'images/AdminAvatar.png', role: 'Invitée', familyId: 'sharma', isLadyWhistledown: false }
    ],

    // Families data
    families: [
        { id: 'hastings', name: 'Hastings', points: 230, rank: 1, votingEnabled: false, revealed: false, ladyWhistledownId: 'h1' },
        { id: 'bridgerton', name: 'Bridgerton', points: 210, rank: 2, votingEnabled: false, revealed: false, ladyWhistledownId: 'b1' },
        { id: 'featherington', name: 'Featherington', points: 180, rank: 3, votingEnabled: false, revealed: false, ladyWhistledownId: 'f1' },
        { id: 'danbury', name: 'Danbury', points: 150, rank: 4, votingEnabled: false, revealed: false, ladyWhistledownId: null },
        { id: 'sharma', name: 'Sharma', points: 120, rank: 5, votingEnabled: false, revealed: false, ladyWhistledownId: 's1' }
    ],

    // Articles data
    articles: [
        { id: '1', title: 'Chers amis lecteurs,', content: 'La notation que la personne va écrire', familyId: 'hastings', familyName: 'Hastings', publishedAt: new Date(Date.now() - 2 * 60 * 60 * 1000).toISOString() },
        { id: '2', title: 'Chers amis lecteurs,', content: 'Un événement des plus intéressants s\'est déroulé lors du dernier bal...', familyId: 'bridgerton', familyName: 'Bridgerton', publishedAt: new Date(Date.now() - 4 * 60 * 60 * 1000).toISOString() },
        { id: '3', title: 'Chers amis lecteurs,', content: 'Les rumeurs circulent à propos d\'une certaine famille...', familyId: 'featherington', familyName: 'Featherington', publishedAt: new Date(Date.now() - 6 * 60 * 60 * 1000).toISOString() },
        { id: '4', title: 'Chers amis lecteurs,', content: 'Les secrets de la haute société ne me sont pas étrangers...', familyId: 'hastings', familyName: 'Hastings', publishedAt: new Date(Date.now() - 8 * 60 * 60 * 1000).toISOString() },
        { id: '5', title: 'Chers amis lecteurs,', content: 'Une nouvelle intrigue secoue les salons londoniens...', familyId: 'danbury', familyName: 'Danbury', publishedAt: new Date(Date.now() - 10 * 60 * 60 * 1000).toISOString() }
    ],

    // Game scores
    gameScores: [
        { gameName: 'Total', familyScores: { hastings: 230, bridgerton: 230, featherington: 230, danbury: 230, sharma: 230 } },
        { gameName: 'Jeu 3', familyScores: { hastings: 230, bridgerton: 230, featherington: 230, danbury: 230, sharma: 230 } },
        { gameName: 'Whistledown', familyScores: { hastings: 0, bridgerton: -10, featherington: 0, danbury: -10, sharma: 0 } },
        { gameName: 'Jeu 2', familyScores: { hastings: 230, bridgerton: 230, featherington: 230, danbury: 230, sharma: 230 } },
        { gameName: 'Whistledown', familyScores: { hastings: 0, bridgerton: 0, featherington: -10, danbury: 0, sharma: 0 } },
        { gameName: 'Jeu 1', familyScores: { hastings: 230, bridgerton: 230, featherington: 230, danbury: 230, sharma: 230 } }
    ],

    // Whistledown penalties
    whistledownPenalties: {
        hastings: 0,
        bridgerton: -10,
        featherington: 0,
        danbury: -10,
        sharma: 0
    },

    // Last publication times (for cooldown)
    lastPublicationTimes: {},

    // Admin credentials
    adminCredentials: {
        username: 'admin',
        password: 'bridgerton2024'
    }
};

// Local Storage Keys
const STORAGE_KEYS = {
    CURRENT_PLAYER: 'bridgerton_currentPlayer',
    IS_ADMIN: 'bridgerton_isAdmin',
    ARTICLES: 'bridgerton_articles',
    FAMILIES: 'bridgerton_families',
    GAME_SCORES: 'bridgerton_gameScores',
    LAST_PUBLICATION_TIMES: 'bridgerton_lastPublicationTimes'
};

// Data Service
const DataService = {
    // Initialize data from localStorage or defaults
    init() {
        // Load articles
        const savedArticles = localStorage.getItem(STORAGE_KEYS.ARTICLES);
        if (savedArticles) {
            GAME_DATA.articles = JSON.parse(savedArticles);
        }

        // Load families
        const savedFamilies = localStorage.getItem(STORAGE_KEYS.FAMILIES);
        if (savedFamilies) {
            GAME_DATA.families = JSON.parse(savedFamilies);
        }

        // Load game scores
        const savedGameScores = localStorage.getItem(STORAGE_KEYS.GAME_SCORES);
        if (savedGameScores) {
            GAME_DATA.gameScores = JSON.parse(savedGameScores);
        }

        // Load last publication times
        const savedTimes = localStorage.getItem(STORAGE_KEYS.LAST_PUBLICATION_TIMES);
        if (savedTimes) {
            GAME_DATA.lastPublicationTimes = JSON.parse(savedTimes);
        }
    },

    // Save data to localStorage
    saveArticles() {
        localStorage.setItem(STORAGE_KEYS.ARTICLES, JSON.stringify(GAME_DATA.articles));
    },

    saveFamilies() {
        localStorage.setItem(STORAGE_KEYS.FAMILIES, JSON.stringify(GAME_DATA.families));
    },

    saveGameScores() {
        localStorage.setItem(STORAGE_KEYS.GAME_SCORES, JSON.stringify(GAME_DATA.gameScores));
    },

    saveLastPublicationTimes() {
        localStorage.setItem(STORAGE_KEYS.LAST_PUBLICATION_TIMES, JSON.stringify(GAME_DATA.lastPublicationTimes));
    },

    // Player methods
    getPlayerByCode(code) {
        return GAME_DATA.players.find(p => p.code.toLowerCase() === code.trim().toLowerCase());
    },

    getAllPlayers() {
        return GAME_DATA.players;
    },

    getPlayersByFamily(familyId) {
        return GAME_DATA.players.filter(p => p.familyId === familyId);
    },

    // Family methods
    getAllFamilies() {
        return GAME_DATA.families;
    },

    getFamilyById(id) {
        return GAME_DATA.families.find(f => f.id === id);
    },

    updateFamilyPoints(familyId, points) {
        const family = this.getFamilyById(familyId);
        if (family) {
            family.points = points;
            this.saveFamilies();
        }
    },

    setLadyWhistledown(familyId, playerId) {
        const family = this.getFamilyById(familyId);
        if (family) {
            family.ladyWhistledownId = playerId;
            this.saveFamilies();
        }
    },

    toggleVoting(familyId, enabled) {
        const family = this.getFamilyById(familyId);
        if (family) {
            family.votingEnabled = enabled;
            this.saveFamilies();
        }
    },

    revealLadyWhistledown(familyId) {
        const family = this.getFamilyById(familyId);
        if (family) {
            family.revealed = true;
            this.saveFamilies();
        }
    },

    // Article methods
    getAllArticles() {
        return GAME_DATA.articles.sort((a, b) => new Date(b.publishedAt) - new Date(a.publishedAt));
    },

    getArticlesByFamily(familyId) {
        return GAME_DATA.articles
            .filter(a => a.familyId === familyId)
            .sort((a, b) => new Date(b.publishedAt) - new Date(a.publishedAt));
    },

    canPublish(familyId) {
        const lastTime = GAME_DATA.lastPublicationTimes[familyId];
        if (!lastTime) return true;
        
        const lastDate = new Date(lastTime);
        const now = new Date();
        const diffMinutes = (now - lastDate) / (1000 * 60);
        return diffMinutes >= 30;
    },

    getTimeUntilNextPublication(familyId) {
        const lastTime = GAME_DATA.lastPublicationTimes[familyId];
        if (!lastTime) return null;
        
        const lastDate = new Date(lastTime);
        const nextTime = new Date(lastDate.getTime() + 30 * 60 * 1000);
        const now = new Date();
        const remaining = nextTime - now;
        
        return remaining > 0 ? remaining : null;
    },

    publishArticle(title, content, familyId, familyName) {
        const article = {
            id: Date.now().toString(),
            title: title,
            content: content,
            familyId: familyId,
            familyName: familyName,
            publishedAt: new Date().toISOString()
        };

        GAME_DATA.articles.push(article);
        GAME_DATA.lastPublicationTimes[familyId] = new Date().toISOString();
        
        this.saveArticles();
        this.saveLastPublicationTimes();
        return article;
    },

    deleteArticle(articleId) {
        const index = GAME_DATA.articles.findIndex(a => a.id === articleId);
        if (index !== -1) {
            GAME_DATA.articles.splice(index, 1);
            this.saveArticles();
        }
    },

    // Game score methods
    getAllGameScores() {
        return GAME_DATA.gameScores;
    },

    updateGameScore(gameName, familyId, points) {
        const game = GAME_DATA.gameScores.find(g => g.gameName === gameName);
        if (game) {
            game.familyScores[familyId] = points;
            this.saveGameScores();
        }
    },

    updateWhistledownPenalty(familyId, penalty) {
        GAME_DATA.whistledownPenalties[familyId] = penalty;
    },

    // Auth methods
    getCurrentPlayer() {
        const saved = localStorage.getItem(STORAGE_KEYS.CURRENT_PLAYER);
        return saved ? JSON.parse(saved) : null;
    },

    setCurrentPlayer(player) {
        if (player) {
            localStorage.setItem(STORAGE_KEYS.CURRENT_PLAYER, JSON.stringify(player));
        } else {
            localStorage.removeItem(STORAGE_KEYS.CURRENT_PLAYER);
        }
    },

    isAdminAuthenticated() {
        return localStorage.getItem(STORAGE_KEYS.IS_ADMIN) === 'true';
    },

    setAdminAuthenticated(value) {
        if (value) {
            localStorage.setItem(STORAGE_KEYS.IS_ADMIN, 'true');
        } else {
            localStorage.removeItem(STORAGE_KEYS.IS_ADMIN);
        }
    },

    validateAdmin(username, password) {
        return username === GAME_DATA.adminCredentials.username && 
               password === GAME_DATA.adminCredentials.password;
    }
};

// Initialize data on load
DataService.init();
