namespace Users.Domain.DTOs;

public class SignUpDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public required string PasswordHash { get; set; }
}