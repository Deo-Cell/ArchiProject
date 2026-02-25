using Archi.API.Data;
using Archi.Library.Logging;
using Archi.Library.Versioning;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Configure SeriLog AVANT tout le reste
SerilogConfig.ConfigureSerilog();

try
{
    Log.Information("=== Démarrage de l'application Archi.API ===");

    var builder = WebApplication.CreateBuilder(args);

    // Remplacer le logger par défaut par SeriLog
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddOpenApi();
    builder.Services.AddControllers();

    // Configurer le versioning d'API (v1, v2, ...)
    builder.Services.AddApiVersioningConfig();

    // Configuration de la base de données SQLite (locale)
    builder.Services.AddDbContext<ArchiDbContext>(options =>
        options.UseSqlite(
            builder.Configuration.GetConnectionString("archilog_db")));

    var app = builder.Build();

    // Créer la base de données automatiquement au démarrage
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ArchiDbContext>();
        dbContext.Database.EnsureCreated(); // Crée la DB si elle n'existe pas
    }

    // Middleware SeriLog pour logger les requêtes HTTP automatiquement
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

    Log.Information("=== Application Archi.API démarrée avec succès ===");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "L'application a échoué au démarrage");
}
finally
{
    Log.CloseAndFlush();
}
