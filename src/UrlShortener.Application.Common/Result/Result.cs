namespace UrlShortener.Application.Common.Result;


public class Result
{
    public virtual bool IsSuccess { get; protected set; }
    public virtual string Message { get; protected set; }
    public virtual IEnumerable<string>? Errors { get; protected set; }

    protected Result(bool isSuccess, string? message, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Message = message == null && isSuccess ? "Success" : "Failure";
        Errors = errors ?? throw new InvalidOperationException(nameof(errors));
    }

    public static Result Success(string? message = null)
    {
        return new Result(true, message, Enumerable.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors, string? message = null)
    {
        if (errors == null)
            throw new ArgumentNullException(nameof(errors));
        return new Result(false, message, errors);
    }
}

public sealed class Result<TEntity> : Result
{
    private readonly TEntity _value;
    private Result(TEntity value, bool isSuccess, string? message, IEnumerable<string> errors)
        : base(isSuccess, message, errors)
    {
        _value = value;
    }

    public TEntity Value
    {
        get
        {
            return IsSuccess ? _value : throw new InvalidOperationException();
        }
    }

    public static Result<TEntity> Success(TEntity entity, string? message = null)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        return new Result<TEntity>(entity, true, message, Enumerable.Empty<string>());
    }

    public new static Result<TEntity> Failure(IEnumerable<string> errors, string? message = null)
    {
        if (errors == null)
            throw new ArgumentNullException(nameof(errors));
        return new Result<TEntity>(default!, false, message, errors);
    }
}