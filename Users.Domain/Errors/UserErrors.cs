using Abstractions.ResultsPattern;

namespace Users.Domain.Errors;

public static class UserErrors
{
    public static readonly Error DoesntExist = new("Users.DoesntExist", "User with this Email does not exist");
    public static Error UserNotFoundId(Guid id) => new("Users.UserNotFound", $"User with ID {id} was not found.");   
    public static Error UserNotFoundEmail(string email) => new("Users.UserNotFound", $"User with Email {email} was not found.");
    public static Error UserAddFailed(string exceptionMessage) => new("Users.UserAddFailed", $"Failed to add user: {exceptionMessage}");
    public static Error UserRemoveFailed(string exceptionMessage) => new("Users.UserRemoveFailed", $"Failed to remove user: {exceptionMessage}");
    public static Error UserEditFailed(string exceptionMessage) => new("Users.UserEditFailed", $"Failed to edit user: {exceptionMessage}");
    public static Error UserAlreadyExists(string email) => new("Users.UserAlreadyExists", $"A user with email '{email}' already exists.");
    
    public static readonly Error InvalidCredentials = new("Users.InvalidCredentials", $"Unable to login in due to invalid credentials.");
}