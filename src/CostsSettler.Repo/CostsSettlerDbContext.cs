using CostsSettler.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CostsSettler.Repo;
public class CostsSettlerDbContext : DbContext
{
    public DbSet<Circumstance> Circumstances { get; set; } = null!;

    public CostsSettlerDbContext(DbContextOptions<CostsSettlerDbContext> options) : base(options)
    {   
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
