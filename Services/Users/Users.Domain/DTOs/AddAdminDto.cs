namespace Users.Domain.DTOs;

public class AddAdminDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}