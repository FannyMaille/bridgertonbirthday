using BridgertonGame.Server.Services;
using BridgertonGame.Server.Data;
using BridgertonGame.Server.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR with enhanced configuration for production
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
});

// Add Database - MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Remplacer les placeholders dans la connection string
if (!string.IsNullOrEmpty(connectionString))
{
    connectionString = connectionString
        .Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost")
        .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT") ?? "3306")
        .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "reveensacados")
        .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "root")
        .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "");

    // S'assurer que la connection string contient les paramètres UTF-8
    if (!connectionString.Contains("Charset=") && !connectionString.Contains("charset="))
    {
        connectionString += ";Charset=utf8mb4;";
    }

    Console.WriteLine(Environment.GetEnvironmentVariable("DB_HOST"));
    Console.WriteLine(Environment.GetEnvironmentVariable("DB_PORT"));
    Console.WriteLine(Environment.GetEnvironmentVariable("DB_NAME"));
    Console.WriteLine(Environment.GetEnvironmentVariable("DB_USER"));
    Console.WriteLine(Environment.GetEnvironmentVariable("DB_PASSWORD"));
    Console.WriteLine(connectionString);

    builder.Services.AddDbContext<BridgertonDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}

// Replace GameDataService with DatabaseGameDataService
builder.Services.AddScoped<DatabaseGameDataService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        policy => policy.WithOrigins("https://localhost:7113", "http://localhost:5257", "https://localhost:5001", "http://localhost:5000","https://bridgerton-birthday.fr")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BridgertonDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (MySqlConnector.MySqlException ex) when (ex.Message.Contains("already exists"))
    {
        Console.WriteLine($"Migration skipped: {ex.Message}");
        // La table existe déjà, continuer normalement
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configure WebSockets - MUST be before UseStaticFiles and UseRouting
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(30)
};

// Allow all origins for WebSocket (important for reverse proxy compatibility)
webSocketOptions.AllowedOrigins.Add("https://bridgerton-birthday.fr");
webSocketOptions.AllowedOrigins.Add("http://localhost:5000");
webSocketOptions.AllowedOrigins.Add("https://localhost:5001");

app.UseWebSockets(webSocketOptions);

app.UseBlazorFrameworkFiles();

// Configuration du cache pour les fichiers statiques
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // En développement, désactiver le cache pour les fichiers CSS
        if (app.Environment.IsDevelopment() && ctx.File.Name.EndsWith(".css"))
        {
            ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            ctx.Context.Response.Headers.Append("Pragma", "no-cache");
            ctx.Context.Response.Headers.Append("Expires", "0");
        }
        else if (!app.Environment.IsDevelopment())
        {
            // En production, utiliser un cache avec validation
            const int durationInSeconds = 60 * 60 * 24 * 7; // 7 jours
            ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={durationInSeconds}");
        }
    }
});

app.UseCors("AllowBlazorClient");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<ChatHub>("/chatHub");
app.MapFallbackToFile("index.html");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
