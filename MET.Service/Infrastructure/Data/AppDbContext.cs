using MET.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MET.Service.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(b =>
        {
            b.ToTable("Expenses");
            b.HasKey(e => e.Id);
            b.Property(e => e.Description).HasMaxLength(200);
            b.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });
    }
}