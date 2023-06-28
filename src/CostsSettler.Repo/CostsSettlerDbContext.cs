using CostsSettler.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CostsSettler.Repo;
public class CostsSettlerDbContext : DbContext
{
    public DbSet<Circumstance> Circumstances { get; set; } = null!;
    public DbSet<MemberCharge> MemberCharges { get; set; } = null!;

    public CostsSettlerDbContext(DbContextOptions<CostsSettlerDbContext> options) : base(options)
    {   
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Circumstance>()
            .HasMany(circumstance => circumstance.Members)
            .WithOne(member => member.Circumstance)
            .HasForeignKey(member => member.CircumstanceId);

        modelBuilder.Entity<MemberCharge>()
            .Ignore(memberCharge => memberCharge.User);

        modelBuilder.Entity<Circumstance>()
            .Property(circ => circ.TotalAmount)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<MemberCharge>()
            .Property(circ => circ.Amount)
            .HasColumnType("decimal(10,2)");
    }
}
