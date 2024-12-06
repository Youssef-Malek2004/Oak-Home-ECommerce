using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Domain.Entities;

public class Order
{
    [Key]
    public Guid OrderId { get; set; } = Guid.NewGuid();

    [Required] public string UserId { get; set; } = string.Empty;

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    [Required] [MaxLength(50)] public string ShippingId { get; set; } = string.Empty;

    [Required] [MaxLength(50)] public string PaymentId { get; set; } = string.Empty; 

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}