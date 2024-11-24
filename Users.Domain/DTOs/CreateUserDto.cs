using System.ComponentModel.DataAnnotations;

namespace Users.Domain.DTOs;

public class CreateUserDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? PasswordHash { get; set; }
}