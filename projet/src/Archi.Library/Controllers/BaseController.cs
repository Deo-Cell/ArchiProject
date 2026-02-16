using Archi.Library.Data;
using Archi.Library.Pagination;
using Archi.Library.Sorting;
using Microsoft.AspNetCore.Mvc;

namespace Archi.Library.Controllers
{
    public abstract class BaseController<C, M>(C context) : ControllerBase where C : BaseDbContext where M : BaseModel
    {
        protected readonly C _context = context;

        /// <summary>
        /// Get all non-deleted entities with pagination and sorting support
        /// </summary>
        /// <param name="range">Range parameter (ex: "0-25" for items 0 to 25)</param>
        /// <param name="asc">Ascending sort (ex: "name,price")</param>
        /// <param name="desc">Descending sort (ex: "createdAt")</param>
        /// <returns>List of entities with pagination headers</returns>
        [HttpGet]
        public virtual ActionResult<IEnumerable<M>> Get(
            [FromQuery] string? range,
            [FromQuery] string? asc,
            [FromQuery] string? desc)
        {
            var query = _context.Set<M>().Where(x => !x.IsDeleted);

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

            // Ajouter les headers de pagination
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            PaginationHeaders.AddPaginationHeaders(
                Response,
                start,
                actualEnd >= 0 ? actualEnd : 0,
                total,
                baseUrl
            );

            return Ok(results);
        }

        /// <summary>
        /// Get a single entity by ID
        /// </summary>
        [HttpGet("{id}")]
        public virtual ActionResult<M> GetById([FromRoute] int id)
        {
            var entity = _context.Set<M>().Find(id);
            if (entity == null || entity.IsDeleted)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        /// <summary>
        /// Create a new entity
        /// </summary>
        [HttpPost]
        public virtual ActionResult<M> Post([FromBody] M model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Set<M>().Add(model);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        /// <summary>
        /// Update an existing entity
        /// </summary>
        [HttpPut("{id}")]
        public virtual ActionResult Put([FromRoute] int id, [FromBody] M model)
        {
            if (id != model.Id)
            {
                return BadRequest("ID mismatch");
            }

            var existing = _context.Set<M>().Find(id);
            if (existing == null || existing.IsDeleted)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(existing).CurrentValues.SetValues(model);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Delete an entity (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public virtual ActionResult Delete([FromRoute] int id)
        {
            var entity = _context.Set<M>().Find(id);
            if (entity == null || entity.IsDeleted)
            {
                return NotFound();
            }

            _context.Set<M>().Remove(entity);
            _context.SaveChanges();
            return NoContent();
        }
    }
}