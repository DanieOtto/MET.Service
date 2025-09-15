using MET.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MET.Service.Infrastructure.Data;

// C#
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(b =>
        {
            b.ToTable("Expenses");
            b.HasKey(e => e.Id);
            b.Property(e => e.Description).HasMaxLength(200);
            b.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        });
    }
}