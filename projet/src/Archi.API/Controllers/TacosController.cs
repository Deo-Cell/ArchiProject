

using Archi.API.Data;
using Archi.API.Models;
using Archi.Library.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Archi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TacosController : BaseController<ArchiDbContext, TacosModel>
{
    public TacosController(ArchiDbContext context) : base(context)
    {
    }

    // 🎉 C'est tout ! Toutes les méthodes CRUD sont héritées de BaseController :
    // - GET /api/tacos          → Get()
    // - GET /api/tacos/{id}     → GetById(id)
    // - POST /api/tacos         → Post(model)
    // - PUT /api/tacos/{id}     → Put(id, model)
    // - DELETE /api/tacos/{id}  → Delete(id)
}