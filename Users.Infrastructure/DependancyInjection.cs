using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Application.Services;
using Users.Domain;
using Users.Domain.Repositories;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Repositories;

namespace Users.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IUsersDbContext, UsersDbContext>(x =>
            x.UseNpgsql(configuration.GetConnectionString("Database")));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}