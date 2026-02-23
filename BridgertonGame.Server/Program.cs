using BridgertonGame.Server.Services;
using BridgertonGame.Server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
        policy => policy.WithOrigins("https://localhost:7113", "http://localhost:5257", "https://localhost:5001", "http://localhost:5000")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BridgertonDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseCors("AllowBlazorClient");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
