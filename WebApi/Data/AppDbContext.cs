using Microsoft.EntityFrameworkCore;
using RindusBackend.Models;

namespace RindusBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    public DbSet<UserModel>? User { get; set; }
}