namespace Abstractions.ResultsPattern;

// Non-generic Result for scenarios where no value needs to be returned.
public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error state", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    // Factory method for a successful non-generic result.
    public static Result Success() => new(true, Error.None);

    // Factory method for a failure non-generic result.
    public static Result Failure(Error error) => new(false, error);
}