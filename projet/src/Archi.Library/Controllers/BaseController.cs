using Archi.Library.Data;
using Archi.Library.Pagination;
using Archi.Library.Sorting;
using Archi.Library.Filters;
using Archi.Library.Search;
using Archi.Library.PartialResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Archi.Library.Controllers
{
    public abstract class BaseController<C, M>(C context, ILogger logger) : ControllerBase where C : BaseDbContext where M : BaseModel
    {
        protected readonly C _context = context;
        protected readonly ILogger _logger = logger;

        /// <summary>
        /// Récupère toutes les entités avec pagination, tri, filtrage, recherche et réponses partielles.
        /// </summary>
        /// <param name="range">Plage de pagination (ex: 0-25)</param>
        /// <param name="asc">Champ(s) pour tri ascendant (ex: name, price)</param>
        /// <param name="desc">Champ(s) pour tri descendant (ex: price)</param>
        /// <returns>Liste d'entités paginée avec headers Content-Range</returns>
        /// <response code="200">Retourne la liste des entités</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual IActionResult Get(
            [FromQuery] string? range,
            [FromQuery] string? asc,
            [FromQuery] string? desc)
        {
            _logger.LogInformation("GET {EntityType} — range={Range} asc={Asc} desc={Desc}",
                typeof(M).Name, range, asc, desc);

            var query = _context.Set<M>().Where(x => !x.IsDeleted);

            // Appliquer la recherche
            var searches = SearchParser.ParseSearch(Request.Query);
            if (searches.Any())
            {
                _logger.LogDebug("Recherche appliquée : {SearchCount} critères", searches.Count);
            }
            query = query.ApplySearch(searches);

            // Appliquer le filtrage
            var filters = QueryFilterParser.ParseFilters(Request.Query);
            if (filters.Any())
            {
                _logger.LogDebug("Filtres appliqués : {FilterCount} filtres", filters.Count);
            }
            query = query.ApplyFilters(filters);

            // Appliquer le tri AVANT la pagination
            var sortings = SortingParser.ParseSorting(asc, desc);
            query = query.ApplySorting(sortings);

            // Compter le total AVANT pagination
            int total = query.Count();

            // Parser le range
            var (start, end) = PaginationHelper.ParseRange(range);

            // Ajuster end si nécessaire
            int actualEnd = Math.Min(end, total - 1);

            // Appliquer la pagination
            var results = PaginationHelper.ApplyPagination(query, start, end).ToList();

            _logger.LogInformation("GET {EntityType} — {Count} résultats sur {Total} (range {Start}-{End})",
                typeof(M).Name, results.Count, total, start, actualEnd);

            // Ajouter les headers de pagination
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            PaginationHeaders.AddPaginationHeaders(
                Response,
                start,
                actualEnd >= 0 ? actualEnd : 0,
                total,
                baseUrl
            );

            // Appliquer la projection dynamique
            var fields = FieldSelector.ParseFields(Request.Query);
            if (fields.Any())
            {
                _logger.LogDebug("Projection partielle : {Fields}", string.Join(", ", fields));
                var projectedResults = results.ApplyProjection(fields);
                return Ok(projectedResults);
            }

            return Ok(results);
        }

        /// <summary>
        /// Récupère une entité par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'entité</param>
        /// <returns>L'entité correspondante</returns>
        /// <response code="200">Retourne l'entité</response>
        /// <response code="404">Entité non trouvée</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual ActionResult<M> GetById([FromRoute] int id)
        {
            _logger.LogInformation("GET {EntityType} id={Id}", typeof(M).Name, id);

            var entity = _context.Set<M>().Find(id);
            if (entity == null || entity.IsDeleted)
            {
                _logger.LogWarning("GET {EntityType} id={Id} — NOT FOUND", typeof(M).Name, id);
                return NotFound();
            }
            return Ok(entity);
        }

        /// <summary>
        /// Crée une nouvelle entité.
        /// </summary>
        /// <param name="model">L'entité à créer</param>
        /// <returns>L'entité créée avec son ID</returns>
        /// <response code="201">Entité créée avec succès</response>
        /// <response code="400">Données invalides</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual ActionResult<M> Post([FromBody] M model)
        {
            _logger.LogInformation("POST {EntityType}", typeof(M).Name);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("POST {EntityType} — Validation échouée : {Errors}",
                    typeof(M).Name,
                    string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            _context.Set<M>().Add(model);
            _context.SaveChanges();

            _logger.LogInformation("POST {EntityType} — Créé avec id={Id}", typeof(M).Name, model.Id);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        /// <summary>
        /// Met à jour une entité existante.
        /// </summary>
        /// <param name="id">Identifiant de l'entité</param>
        /// <param name="model">Les nouvelles données</param>
        /// <response code="204">Mis à jour avec succès</response>
        /// <response code="400">ID mismatch ou données invalides</response>
        /// <response code="404">Entité non trouvée</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual ActionResult Put([FromRoute] int id, [FromBody] M model)
        {
            _logger.LogInformation("PUT {EntityType} id={Id}", typeof(M).Name, id);

            if (id != model.Id)
            {
                _logger.LogWarning("PUT {EntityType} — ID mismatch (route={RouteId}, body={BodyId})",
                    typeof(M).Name, id, model.Id);
                return BadRequest("ID mismatch");
            }

            var existing = _context.Set<M>().Find(id);
            if (existing == null || existing.IsDeleted)
            {
                _logger.LogWarning("PUT {EntityType} id={Id} — NOT FOUND", typeof(M).Name, id);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("PUT {EntityType} id={Id} — Validation échouée", typeof(M).Name, id);
                return BadRequest(ModelState);
            }

            _context.Entry(existing).CurrentValues.SetValues(model);
            _context.SaveChanges();

            _logger.LogInformation("PUT {EntityType} id={Id} — Mis à jour avec succès", typeof(M).Name, id);
            return NoContent();
        }

        /// <summary>
        /// Supprime une entité (suppression logique).
        /// </summary>
        /// <param name="id">Identifiant de l'entité à supprimer</param>
        /// <response code="204">Supprimé avec succès</response>
        /// <response code="404">Entité non trouvée</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual ActionResult Delete([FromRoute] int id)
        {
            _logger.LogInformation("DELETE {EntityType} id={Id}", typeof(M).Name, id);

            var entity = _context.Set<M>().Find(id);
            if (entity == null || entity.IsDeleted)
            {
                _logger.LogWarning("DELETE {EntityType} id={Id} — NOT FOUND", typeof(M).Name, id);
                return NotFound();
            }

            _context.Set<M>().Remove(entity);
            _context.SaveChanges();

            _logger.LogInformation("DELETE {EntityType} id={Id} — Supprimé avec succès", typeof(M).Name, id);
            return NoContent();
        }
    }
}