
using Archi.API.Data;
using Archi.API.Models;
using Archi.Library.Controllers;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Archi.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class TacosController : BaseController<ArchiDbContext, TacosModel>
{
    public TacosController(ArchiDbContext context, ILogger<TacosController> logger) : base(context, logger)
    {
    }

    // 🎉 C'est tout ! Toutes les méthodes CRUD sont héritées de BaseController :
    // - GET /api/v1/tacos          → Get()
    // - GET /api/v1/tacos/{id}     → GetById(id)
    // - POST /api/v1/tacos         → Post(model)
    // - PUT /api/v1/tacos/{id}     → Put(id, model)
    // - DELETE /api/v1/tacos/{id}  → Delete(id)
}