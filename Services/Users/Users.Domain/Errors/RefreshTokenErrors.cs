using Abstractions.ResultsPattern;

namespace Users.Domain.Errors;

public static class RefreshTokenErrors
{
    public static Error RefreshTokenMissing() =>
        new("Tokens.RefreshTokenMissing", "The refresh token is missing.");

    public static Error RefreshTokenInvalidOrExpired() =>
        new("Tokens.RefreshTokenInvalidOrExpired", "The refresh token is invalid or has expired.");
}