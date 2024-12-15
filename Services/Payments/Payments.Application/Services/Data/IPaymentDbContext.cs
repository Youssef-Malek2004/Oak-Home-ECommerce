using Microsoft.EntityFrameworkCore;
using Payments.Domain.Entities;

namespace Payments.Application.Services.Data;

public interface IPaymentDbContext 
{
    DbSet<Payment> Payments { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));  
}