using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users.Domain.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string? Username { get; set; }
    
    [MaxLength(50)]
    public string? Email { get; set; }

    [MaxLength(10)]
    public string? Role { get; set; }
}