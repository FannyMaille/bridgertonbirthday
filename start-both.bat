@echo off
echo ===========================================
echo Starting BridgertonGame Application
echo ===========================================
:: Ensure script runs from repository root
pushd "%~dp0"
echo Starting Server in a new window...
start "BridgertonGame Server" cmd /k "cd /d "%~dp0BridgertonGame.Server" && dotnet dev-certs https --trust >nul 2>&1 && dotnet run --urls "https://localhost:7191;http://localhost:5062""
echo.
echo Waiting for server to initialize (10s)...
timeout /t 10 /nobreak >nul
echo Starting Client in a new window...
start "BridgertonGame Client" cmd /k "cd /d "%~dp0BridgertonGame.Client" && dotnet run"
echo.
echo Waiting for the client to be reachable on https://localhost:7113 ...
echo (will wait up to 30 seconds)
powershell -NoProfile -Command "$max=30; for ($i=0; $i -lt $max; $i++) { try { $null = Invoke-WebRequest -Uri 'https://localhost:7113' -UseBasicParsing -TimeoutSec 1 -ErrorAction SilentlyContinue; Start-Process 'https://localhost:7113'; exit 0 } catch { } Start-Sleep -Seconds 1 }; Start-Process 'https://localhost:7113'"
echo.
echo ===========================================
echo Both applications have been started. The browser should have opened to the client.
echo Server: https://localhost:7191
echo Client: https://localhost:7113
echo.
popd
echo Press any key to close this window (apps will continue running)...
pause >nul
