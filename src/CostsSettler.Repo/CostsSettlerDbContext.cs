using CostsSettler.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CostsSettler.Repo;

/// <summary>
/// DbContext used in CostsSettler application.
/// </summary>
public class CostsSettlerDbContext : DbContext
{
    /// <summary>
    /// DbSet of circumstances.
    /// </summary>
    public DbSet<Circumstance> Circumstances { get; set; } = null!;
    
    /// <summary>
    /// DbSet of charges.
    /// </summary>
    public DbSet<Charge> Charges { get; set; } = null!;

    /// <summary>
    /// Creates new CostsSettlerDbContext instance.
    /// </summary>
    /// <param name="options">DbContext options.</param>
    public CostsSettlerDbContext(DbContextOptions<CostsSettlerDbContext> options) : base(options)
    {   
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Circumstance>()
            .HasMany(circumstance => circumstance.Charges)
            .WithOne(charge => charge.Circumstance)
            .HasForeignKey(charge => charge.CircumstanceId);

        modelBuilder.Entity<Charge>()
            .Ignore(charge => charge.Creditor)
            .Ignore(charge => charge.Debtor)
            .Ignore(charge => charge.DateTime);

        modelBuilder.Entity<Circumstance>()
            .Ignore(circumstance => circumstance.Creditor)
            .Ignore(circumstance => circumstance.Debtors)
            .Ignore(circumstance => circumstance.Members);

        modelBuilder.Entity<Circumstance>()
            .Property(circ => circ.TotalAmount)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<Charge>()
            .Property(circ => circ.Amount)
            .HasColumnType("decimal(10,2)");
    }
}
