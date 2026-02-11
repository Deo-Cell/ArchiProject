using Archi.API.Models;
using Archi.Library.Data;
using Microsoft.EntityFrameworkCore;

namespace Archi.API.Data;

public class ArchiDbContext : BaseDbContext
{
    public ArchiDbContext(DbContextOptions<ArchiDbContext> options)
    : base(options)
    {
    }

    public DbSet<TacosModel> Tacos { get; set; }
    public DbSet<PizzaModel> Pizzas { get; set; }
}