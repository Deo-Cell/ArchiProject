using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Archi.Library.Versioning
{
    /// <summary>
    /// Configuration centralisée du versioning d'API.
    /// Utilise le versioning par URL : /api/v1/pizzas, /api/v2/pizzas, etc.
    /// </summary>
    public static class ApiVersionConfig
    {
        /// <summary>
        /// Version par défaut de l'API (v1.0)
        /// </summary>
        public static readonly ApiVersion DefaultVersion = new(1, 0);

        /// <summary>
        /// Configure le versioning dans le service collection.
        /// </summary>
        public static IServiceCollection AddApiVersioningConfig(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // Version par défaut si non spécifiée
                options.DefaultApiVersion = DefaultVersion;

                // Utiliser la version par défaut si le client ne spécifie pas de version
                options.AssumeDefaultVersionWhenUnspecified = true;

                // Inclure les versions supportées dans les headers de réponse
                options.ReportApiVersions = true;

                // Lire la version depuis l'URL (ex: /api/v1/pizzas)
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                // Format du groupe de version : 'v'VVV (ex: v1, v2)
                options.GroupNameFormat = "'v'VVV";

                // Remplacer le {version} dans les routes par la version réelle
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
