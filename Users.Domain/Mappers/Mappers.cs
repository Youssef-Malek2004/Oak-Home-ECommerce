using Users.Domain.DTOs;
using Users.Domain.Entities;

namespace Users.Domain.Mappers;

public static class Mappers
{
    public static User CreateUserToUser(CreateUserDto createUserDto)
    {
        return new User
        {
            Email = createUserDto.Email,
            Username = createUserDto.Username,
            PasswordHash = createUserDto.PasswordHash,
        };
    }
}