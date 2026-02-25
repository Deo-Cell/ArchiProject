
using Archi.API.Data;
using Archi.API.Models;
using Archi.Library.Controllers;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Archi.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class PizzasController : BaseController<ArchiDbContext, PizzaModel>
{
    public PizzasController(ArchiDbContext context, ILogger<PizzasController> logger) : base(context, logger)
    {
    }

    // 🎉 C'est tout ! Toutes les méthodes CRUD sont héritées de BaseController :
    // - GET /api/v1/pizzas          → Get()
    // - GET /api/v1/pizzas/{id}     → GetById(id)
    // - POST /api/v1/pizzas         → Post(model)
    // - PUT /api/v1/pizzas/{id}     → Put(id, model)
    // - DELETE /api/v1/pizzas/{id}  → Delete(id)

    // Tu peux ajouter des méthodes custom ici si besoin, par exemple :
    // [HttpGet("special")]
    // public ActionResult GetSpecialPizzas() { ... }
}