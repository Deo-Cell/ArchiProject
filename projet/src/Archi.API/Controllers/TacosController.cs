

using Archi.API.Data;
using Archi.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Archi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TacosController : ControllerBase
{
    private readonly ArchiDbContext _context;

    public TacosController(ArchiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TacosModel>> Get()
    {
        return Ok(_context.Tacos.ToList());
    }
}