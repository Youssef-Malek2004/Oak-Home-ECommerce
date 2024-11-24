namespace Abstractions.ResultsPattern;

// Generic Result for scenarios where a value (TEntity) needs to be returned.
public class Result<TEntity> : Result
{
    private Result(TEntity? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public TEntity? Value { get; }

    // Factory method for a successful generic result.
    public static Result<TEntity> Success(TEntity value) => new(value, true, Error.None);

    // Factory method for a failure generic result.
    public new static Result<TEntity> Failure(Error error) => new(default, false, error);
}