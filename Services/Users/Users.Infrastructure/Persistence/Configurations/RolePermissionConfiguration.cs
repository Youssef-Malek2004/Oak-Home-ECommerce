using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Contracts.Authorization;
using Users.Domain.Entities;
using Users.Infrastructure.Authentication;

namespace Users.Infrastructure.Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        builder.HasData(
            Create(Role.Registered, Permissions.ReadUsers),
            Create(Role.Registered, Permissions.UpdateUsers),
            Create(Role.Admin, Permissions.ReadUsers),
            Create(Role.Admin, Permissions.UpdateUsers),
            Create(Role.Admin, Permissions.SoftDeleteUsers),
            Create(Role.Admin, Permissions.PerformCrud),
            Create(Role.Admin, Permissions.MustBeSameUser),
            Create(Role.Registered, Permissions.MustBeSameUser));
    }

    private static RolePermission Create(
        Role role, Permissions permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = (int)permission.Id
        };
    }
}