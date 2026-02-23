@echo off
echo ========================================
echo Mise à jour de DatabaseGameDataService avec BCrypt
echo ========================================
echo.

powershell -NoProfile -Command ^
"$file = 'BridgertonGame.Server\Services\DatabaseGameDataService.cs'; ^
$old = '        var admin = await _context.AdminCredentials\r\n            .FirstOrDefaultAsync(a => a.Username == username ^&^& a.Password == password);\r\n        return admin != null;'; ^
$new = '        var admin = await _context.AdminCredentials\r\n            .FirstOrDefaultAsync(a => a.Username == username);\r\n        \r\n        if (admin == null)\r\n            return false;\r\n\r\n        // Verify the password using BCrypt\r\n        return BCrypt.Net.BCrypt.Verify(password, admin.Password);'; ^
(Get-Content $file -Raw).Replace($old, $new) | Set-Content $file -NoNewline; ^
Write-Host 'DatabaseGameDataService.cs mis a jour avec BCrypt!' -ForegroundColor Green"

echo.
echo ✅ Fichier mis à jour !
echo.
pause
