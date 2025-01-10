using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.Infrastructure.Persistence.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Domain.Entities.Cart>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Cart> builder)
    {
        builder.HasKey(c => c.CartId);

        builder.Property(c => c.TotalPrice)
            .HasPrecision(18, 2) 
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.HasIndex(c => c.UserId);
    }
}