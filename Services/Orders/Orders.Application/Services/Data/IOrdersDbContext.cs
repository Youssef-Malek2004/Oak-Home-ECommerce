using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;

namespace Orders.Application.Services.Data;

public interface IOrdersDbContext
{
    DbSet<Order> Orders { get; set; }
    
    DbSet<OrderItem> OrderItems { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));   
}