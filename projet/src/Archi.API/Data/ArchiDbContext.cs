using Microsoft.EntityFrameworkCore;

public class ArchiDbContext : DbContext
{
    public ArchiDbContext(DbContextOptions<ArchiDbContext> options)
    : base(options)
    {
    }


}