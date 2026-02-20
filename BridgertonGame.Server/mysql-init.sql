-- Migration MySQL pour Bridgerton Game
-- Créer la base de données si elle n'existe pas
CREATE DATABASE IF NOT EXISTS bridgerton CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE bridgerton;

-- Table Players
CREATE TABLE IF NOT EXISTS `Players` (
    `Id` VARCHAR(255) NOT NULL PRIMARY KEY,
    `Code` VARCHAR(255) NOT NULL,
    `Name` VARCHAR(255) NOT NULL,
    `Title` VARCHAR(255) NOT NULL,
    `ImageUrl` VARCHAR(500) NOT NULL,
    `Role` VARCHAR(255) NOT NULL,
    `FamilyId` VARCHAR(255) NOT NULL,
    `IsLadyWhistledown` BOOLEAN NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Table Families
CREATE TABLE IF NOT EXISTS `Families` (
    `Id` VARCHAR(255) NOT NULL PRIMARY KEY,
    `Name` VARCHAR(255) NOT NULL,
    `Points` INT NOT NULL DEFAULT 0,
    `Rank` INT NOT NULL DEFAULT 0,
    `VotingEnabled` BOOLEAN NOT NULL DEFAULT FALSE,
    `Revealed` BOOLEAN NOT NULL DEFAULT FALSE,
    `LadyWhistledownId` VARCHAR(255) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Table Articles
CREATE TABLE IF NOT EXISTS `Articles` (
    `Id` VARCHAR(255) NOT NULL PRIMARY KEY,
    `Title` VARCHAR(500) NOT NULL,
    `Content` TEXT NOT NULL,
    `FamilyId` VARCHAR(255) NOT NULL,
    `FamilyName` VARCHAR(255) NOT NULL,
    `PublishedAt` DATETIME(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Table GameScores
CREATE TABLE IF NOT EXISTS `GameScores` (
    `Id` INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `GameName` VARCHAR(255) NOT NULL,
    `FamilyId` VARCHAR(255) NOT NULL,
    `Score` INT NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Table PublicationCooldowns
CREATE TABLE IF NOT EXISTS `PublicationCooldowns` (
    `FamilyId` VARCHAR(255) NOT NULL PRIMARY KEY,
    `LastPublicationTime` DATETIME(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Table WhistledownPenalties
CREATE TABLE IF NOT EXISTS `WhistledownPenalties` (
    `FamilyId` VARCHAR(255) NOT NULL PRIMARY KEY,
    `Penalty` INT NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Table AdminCredentials
CREATE TABLE IF NOT EXISTS `AdminCredentials` (
    `Id` INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `Username` VARCHAR(255) NOT NULL,
    `Password` VARCHAR(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Seed Players
INSERT IGNORE INTO `Players` VALUES
('h1', 'CELIA2024', 'Célia Hastings', 'DUCHESSE', 'images/AdminAvatar.png', 'Lady Whistledown', 'hastings', TRUE),
('h2', 'FANNY2024', 'Fanny Hastings', 'DUCHESSE', 'images/AdminAvatar.png', 'Invitée', 'hastings', FALSE),
('h3', 'HUGO2024', 'Hugo Hastings', 'DUC', 'images/AdminAvatar.png', 'Invité', 'hastings', FALSE),
('b1', 'DAPHNE2024', 'Daphné Bridgerton', 'DUCHESSE', 'images/AdminAvatar.png', 'Lady Whistledown', 'bridgerton', TRUE),
('b2', 'SIMON2024', 'Simon Bridgerton', 'DUC', 'images/AdminAvatar.png', 'Invité', 'bridgerton', FALSE),
('b3', 'ELOISE2024', 'Eloïse Bridgerton', 'LADY', 'images/AdminAvatar.png', 'Invitée', 'bridgerton', FALSE),
('f1', 'PENELOPE2024', 'Penelope Featherington', 'LADY', 'images/AdminAvatar.png', 'Lady Whistledown', 'featherington', TRUE),
('f2', 'PORTIA2024', 'Portia Featherington', 'LADY', 'images/AdminAvatar.png', 'Invitée', 'featherington', FALSE),
('d1', 'AGATHA2024', 'Agatha Danbury', 'LADY', 'images/AdminAvatar.png', 'Maîtresse de soirée', 'danbury', FALSE),
('d2', 'WILL2024', 'Will Danbury', 'LORD', 'images/AdminAvatar.png', 'Invité', 'danbury', FALSE),
('s1', 'KATE2024', 'Kate Sharma', 'LADY', 'images/AdminAvatar.png', 'Lady Whistledown', 'sharma', TRUE),
('s2', 'EDWINA2024', 'Edwina Sharma', 'LADY', 'images/AdminAvatar.png', 'Invitée', 'sharma', FALSE);

-- Seed Families
INSERT IGNORE INTO `Families` VALUES
('hastings', 'Hastings', 230, 1, FALSE, FALSE, 'h1'),
('bridgerton', 'Bridgerton', 210, 2, FALSE, FALSE, 'b1'),
('featherington', 'Featherington', 180, 3, FALSE, FALSE, 'f1'),
('danbury', 'Danbury', 150, 4, FALSE, FALSE, NULL),
('sharma', 'Sharma', 120, 5, FALSE, FALSE, 's1');

-- Seed Articles (exemples)
INSERT IGNORE INTO `Articles` VALUES
('1', 'Chers amis lecteurs,', 'La notation que la personne va écrire', 'hastings', 'Hastings', UTC_TIMESTAMP() - INTERVAL 10 HOUR),
('2', 'Chers amis lecteurs,', 'Un événement des plus intéressants s\'est déroulé lors du dernier bal...', 'bridgerton', 'Bridgerton', UTC_TIMESTAMP() - INTERVAL 8 HOUR),
('3', 'Chers amis lecteurs,', 'Les rumeurs circulent à propos d\'une certaine famille...', 'featherington', 'Featherington', UTC_TIMESTAMP() - INTERVAL 6 HOUR),
('4', 'Chers amis lecteurs,', 'Les secrets de la haute société ne me sont pas étrangers...', 'hastings', 'Hastings', UTC_TIMESTAMP() - INTERVAL 4 HOUR),
('5', 'Chers amis lecteurs,', 'Une nouvelle intrigue secoue les salons londoniens...', 'danbury', 'Danbury', UTC_TIMESTAMP() - INTERVAL 2 HOUR);

-- Seed GameScores
INSERT IGNORE INTO `GameScores` (`Id`, `GameName`, `FamilyId`, `Score`) VALUES
(1, 'Total', 'hastings', 230),
(2, 'Total', 'bridgerton', 230),
(3, 'Total', 'featherington', 230),
(4, 'Total', 'danbury', 230),
(5, 'Total', 'sharma', 230),
(6, 'Jeu 1', 'hastings', 230),
(7, 'Jeu 1', 'bridgerton', 230),
(8, 'Jeu 1', 'featherington', 230),
(9, 'Jeu 1', 'danbury', 230),
(10, 'Jeu 1', 'sharma', 230),
(11, 'Jeu 2', 'hastings', 230),
(12, 'Jeu 2', 'bridgerton', 230),
(13, 'Jeu 2', 'featherington', 230),
(14, 'Jeu 2', 'danbury', 230),
(15, 'Jeu 2', 'sharma', 230),
(16, 'Jeu 3', 'hastings', 230),
(17, 'Jeu 3', 'bridgerton', 230),
(18, 'Jeu 3', 'featherington', 230),
(19, 'Jeu 3', 'danbury', 230),
(20, 'Jeu 3', 'sharma', 230),
(21, 'Whistledown', 'hastings', 0),
(22, 'Whistledown', 'bridgerton', -10),
(23, 'Whistledown', 'featherington', 0),
(24, 'Whistledown', 'danbury', -10),
(25, 'Whistledown', 'sharma', 0);

-- Seed WhistledownPenalties
INSERT IGNORE INTO `WhistledownPenalties` VALUES
('hastings', 0),
('bridgerton', -10),
('featherington', 0),
('danbury', -10),
('sharma', 0);

-- Seed AdminCredentials
INSERT IGNORE INTO `AdminCredentials` (`Id`, `Username`, `Password`) VALUES
(1, 'admin', 'bridgerton2024');

-- Créer les index pour améliorer les performances
CREATE INDEX idx_articles_familyid ON `Articles`(`FamilyId`);
CREATE INDEX idx_articles_published ON `Articles`(`PublishedAt`);
CREATE INDEX idx_gamescores_game_family ON `GameScores`(`GameName`, `FamilyId`);
CREATE INDEX idx_players_familyid ON `Players`(`FamilyId`);
CREATE INDEX idx_players_code ON `Players`(`Code`);
