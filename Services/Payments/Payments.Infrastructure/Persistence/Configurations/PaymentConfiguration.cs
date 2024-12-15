using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payments.Domain.Entities;

namespace Payments.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.Property(p => p.OrderId)
            .IsRequired();

        builder.Property(p => p.PaymentTime)
            .IsRequired();

        builder.Property(p => p.Amount)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.PaymentMethod)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.TransactionId)
            .HasMaxLength(100);

        builder.Property(p => p.Currency)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(p => p.FailureReason)
            .HasMaxLength(500);
        
        builder.HasIndex(p => p.UserId);
    }
}