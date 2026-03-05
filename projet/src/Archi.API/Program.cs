using Archi.API.Data;
using Archi.Library.Logging;
using Archi.Library.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

// Configure SeriLog AVANT tout le reste
SerilogConfig.ConfigureSerilog();

try
{
    Log.Information("=== Démarrage de l'application Archi.API ===");

    var builder = WebApplication.CreateBuilder(args);

    // Remplacer le logger par défaut par SeriLog
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddControllers();

    // Configurer le versioning d'API (v1, v2, ...)
    builder.Services.AddApiVersioningConfig();

    // Configurer Swagger / OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Archi.API",
            Description = "API REST pour la gestion de produits alimentaires (Pizzas, Tacos, Burgers). " +
                          "Supporte la pagination, le tri, le filtrage, la recherche et les réponses partielles.",
           
        });
        // Inclure les commentaires XML pour la documentation
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);

        // Inclure aussi les XML de la Library (BaseController)
        var libXmlPath = Path.Combine(AppContext.BaseDirectory, "Archi.Library.xml");
        if (File.Exists(libXmlPath))
            options.IncludeXmlComments(libXmlPath);
    });

    // Configuration de la base de données SQLite (locale)
    builder.Services.AddDbContext<ArchiDbContext>(options =>
        options.UseSqlite(
            builder.Configuration.GetConnectionString("archilog_db")));

    var app = builder.Build();

    // Créer la base de données automatiquement au démarrage
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ArchiDbContext>();
        dbContext.Database.EnsureCreated();
    }

    // Middleware SeriLog pour logger les requêtes HTTP automatiquement
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    });

    // Activer Swagger UI (accessible en dev ET en prod)
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Archi.API v1");
        options.DocumentTitle = "Archi.API - Documentation Swagger";
    });

    app.UseHttpsRedirection();

    app.MapControllers();

    Log.Information("=== Application Archi.API démarrée avec succès ===");
    Log.Information("📖 Swagger UI disponible sur : http://localhost:5033/swagger");

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
