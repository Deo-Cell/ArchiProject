using Archi.Library.Data;
using Microsoft.AspNetCore.Mvc;

namespace Archi.Library.Controllers
{
    public abstract class BaseController<C, M> : ControllerBase where C : BaseDbContext where M : BaseModel
    {
        protected readonly C _context;

        public BaseController(C context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<M>> Get()
        {
            return Ok(_context.Set<M>().Where(x => !x.IsDeleted).ToList());
        }
    }
}