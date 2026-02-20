$filePath = "BridgertonGame.Server\Services\DatabaseGameDataService.cs"
$content = Get-Content $filePath -Raw

$oldCode = @"
    // Auth methods
    public async Task<bool> ValidateAdminAsync(string username, string password)
    {
        var admin = await _context.AdminCredentials
            .FirstOrDefaultAsync(a => a.Username == username && a.Password == password);
        return admin != null;
    }
"@

$newCode = @"
    // Auth methods
    public async Task<bool> ValidateAdminAsync(string username, string password)
    {
        var admin = await _context.AdminCredentials
            .FirstOrDefaultAsync(a => a.Username == username);
        
        if (admin == null)
            return false;

        // Verify the password using BCrypt
        return BCrypt.Net.BCrypt.Verify(password, admin.Password);
    }
"@

$content = $content.Replace($oldCode, $newCode)
Set-Content $filePath -Value $content -NoNewline

Write-Host "✅ DatabaseGameDataService.cs mis à jour avec BCrypt!" -ForegroundColor Green
