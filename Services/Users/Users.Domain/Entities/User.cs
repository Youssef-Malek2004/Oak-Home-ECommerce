using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users.Domain.Entities;

public class User 
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [MaxLength(50)]
    public string? Username { get; set; }
    
    [MaxLength(50)]
    public string? Email { get; set; }

    public ICollection<Role>? Roles { get; set; }
    
    [MaxLength(1000)]
    public string? PasswordHash { get; set; }

    public bool IsDeleted { get; set; } = false;
}