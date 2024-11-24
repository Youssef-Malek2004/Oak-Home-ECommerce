using Users.Domain.Entities;

namespace Users.Application.Services;

public interface IJwtProvider
{
    string Generate(User user);
}