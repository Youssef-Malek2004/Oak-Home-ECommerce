using Cart.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.Infrastructure.Persistence.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => ci.ProductId); 

        builder.Property(ci => ci.ProductId)
            .IsRequired()
            .HasMaxLength(50); 

        builder.Property(ci => ci.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.Property(ci => ci.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Ignore(ci => ci.TotalPrice);

        builder.HasIndex(ci => ci.ProductName);
    }
}