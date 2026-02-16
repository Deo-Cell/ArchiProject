

using Archi.API.Data;
using Archi.API.Models;
using Archi.Library.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Archi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzasController : BaseController<ArchiDbContext, PizzaModel>
{
    public PizzasController(ArchiDbContext context) : base(context)
    {
    }

    // 🎉 C'est tout ! Toutes les méthodes CRUD sont héritées de BaseController :
    // - GET /api/pizzas          → Get()
    // - GET /api/pizzas/{id}     → GetById(id)
    // - POST /api/pizzas         → Post(model)
    // - PUT /api/pizzas/{id}     → Put(id, model)
    // - DELETE /api/pizzas/{id}  → Delete(id)
    
    // Tu peux ajouter des méthodes custom ici si besoin, par exemple :
    // [HttpGet("special")]
    // public ActionResult GetSpecialPizzas() { ... }
}