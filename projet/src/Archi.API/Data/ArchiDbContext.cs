using Archi.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Archi.API.Data;

public class ArchiDbContext : DbContext
{
    public ArchiDbContext(DbContextOptions<ArchiDbContext> options)
    : base(options)
    {
    }

    public DbSet<TacosModel> Tacos { get; set; }
}