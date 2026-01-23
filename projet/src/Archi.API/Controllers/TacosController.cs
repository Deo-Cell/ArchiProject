

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

    [HttpGet("{id}")]
    public ActionResult<TacosModel> GetById([FromRoute] int id)
    {
        var tacos = _context.Tacos.Find(id);
        if (tacos == null)
        {
            return NotFound();
        }
        return Ok(tacos);
    }

    [HttpPost]
    public ActionResult<TacosModel> Post([FromBody] TacosModel tacos)
    {
        if (ModelState.IsValid)
        {
            _context.Tacos.Add(tacos);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = tacos.Id }, tacos);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{id}")]
    public ActionResult Put([FromRoute] int id, [FromBody] TacosModel tacos)
    {
        if (id != tacos.Id)
        {
            return BadRequest();
        }
        var existingTacos = _context.Tacos.Find(id);
        if (existingTacos == null)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            _context.Entry(existingTacos).CurrentValues.SetValues(tacos);
            _context.SaveChanges();
            return NoContent();
        }
        return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        var tacos = _context.Tacos.Find(id);
        if (tacos == null)
        {
            return NotFound();
        }
        _context.Tacos.Remove(tacos);
        _context.SaveChanges();
        return NoContent();
    }
}