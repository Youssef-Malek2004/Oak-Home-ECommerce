using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Services.Data;
using Payments.Infrastructure.Persistence;

namespace Payments.Infrastructure;

public static class DependencyInjection
{
    private const string DatabaseConnection = "DatabaseLocal";
    
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IPaymentDbContext, PaymentDbContext>(x =>
            x.UseNpgsql(configuration.GetConnectionString(DatabaseConnection)));

        return services;
    }
}