using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleJoin>
{
    public void Configure(EntityTypeBuilder<UserRoleJoin> builder)
    {
        builder.HasKey(x => new { x.UserId, x.RoleId });
    }
}