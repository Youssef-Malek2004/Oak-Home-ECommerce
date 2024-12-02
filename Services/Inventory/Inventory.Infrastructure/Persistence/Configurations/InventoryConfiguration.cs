using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Configurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventories>
{
    public void Configure(EntityTypeBuilder<Inventories> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.ProductId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.VendorId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.WarehouseId)
            .IsRequired();

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.Property(i => i.IsAvailable)
            .IsRequired();

        builder.Property(i => i.IsDeleted)
            .IsRequired();

        builder.Property(i => i.LastUpdated)
            .IsRequired();

        builder.Property(i => i.UpdatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Price)
            .IsRequired();
    }
}