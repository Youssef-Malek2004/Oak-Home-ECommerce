using Microsoft.EntityFrameworkCore;
using Payments.Application.Services.Data;
using Payments.Domain.Entities;

namespace Payments.Infrastructure.Persistence;

public class PaymentDbContext : DbContext, IPaymentDbContext
{
    public PaymentDbContext()
    {
    }

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Payment> Payments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
    }
}