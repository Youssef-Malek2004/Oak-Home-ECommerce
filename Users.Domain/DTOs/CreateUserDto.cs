using System.ComponentModel.DataAnnotations;

namespace Users.Domain.DTOs;

public class CreateUserDto
{
    public string? Username { get; set; }
    
    [MaxLength(50)]
    public string? Email { get; set; }

    [MaxLength(10)]
    public string? Role { get; set; }
    
    [MaxLength(1000)]
    public string? PasswordHash { get; set; }
}