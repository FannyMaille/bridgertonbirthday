// Personnages Page JavaScript

document.addEventListener('DOMContentLoaded', function() {
    loadCharacters();
});

function loadCharacters() {
    const families = DataService.getAllFamilies();
    const allPlayers = DataService.getAllPlayers();
    
    // Find the host (Maîtresse de soirée)
    const hostPlayer = allPlayers.find(p => p.role === 'Maîtresse de soirée');
    
    // Display host section if found
    if (hostPlayer) {
        const hostSection = document.getElementById('host-section');
        hostSection.style.display = 'flex';
        document.getElementById('host-image').src = hostPlayer.imageUrl;
        document.getElementById('host-image').alt = hostPlayer.name;
        document.getElementById('host-title').textContent = hostPlayer.title;
        document.getElementById('host-name').textContent = hostPlayer.name;
    }
    
    // Display families
    const familiesContainer = document.getElementById('families-container');
    familiesContainer.innerHTML = families.map(family => {
        const familyMembers = allPlayers.filter(p => p.familyId === family.id && p.id !== hostPlayer?.id);
        
        return `
            <div class="family-section">
                <div class="family-divider">
                    <div class="flower-line">
                        <img src="../images/Fleurs.png" alt="" />
                    </div>
                    <h3 class="family-name-title">Famille ${family.name}</h3>
                    <div class="flower-line">
                        <img src="../images/Fleurs.png" alt="" />
                    </div>
                </div>

                <div class="family-members-grid">
                    ${familyMembers.map(member => `
                        <div class="character-card">
                            <div class="ornament-frame character-frame">
                                <img src="../images/OrnementCadre.png" alt="" class="ornament-border" />
                                <div class="character-image-container">
                                    <img src="${member.imageUrl}" alt="${member.name}" class="character-image" />
                                </div>
                            </div>
                            <div class="character-title">${member.title}</div>
                            <div class="character-name">${member.name}</div>
                        </div>
                    `).join('')}
                </div>
            </div>
        `;
    }).join('');
}
