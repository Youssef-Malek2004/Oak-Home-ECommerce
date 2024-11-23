using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;

namespace Users.Application.Services;

public interface IUsersDbContext 
{
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}