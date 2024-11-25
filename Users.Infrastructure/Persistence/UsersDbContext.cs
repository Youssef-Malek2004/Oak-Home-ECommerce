using Microsoft.EntityFrameworkCore;
using Users.Application.Services;
using Users.Domain.Entities;

namespace Users.Infrastructure.Persistence;

public class UsersDbContext : DbContext, IUsersDbContext
{
    public UsersDbContext()
    {
    }

    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
    }
}