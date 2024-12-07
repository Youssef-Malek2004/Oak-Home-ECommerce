using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Contracts.Authorization;
using Users.Domain.Entities;
using Users.Infrastructure.Authentication;

namespace Users.Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        
        builder.HasKey(x => x.Id);

        var permissions = Permissions.GetValues()
            .Select(p => new Permission()
            {
                Id = (int)p.Id,
                Name = p.Name.ToString()
            });

        builder.HasData(permissions);
    }
}