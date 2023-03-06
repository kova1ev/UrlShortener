namespace UrlShortener.Application.Common.Result;


public class Result
{
    public virtual bool IsSuccess { get; protected set; }
    public virtual IEnumerable<string>? Errors { get; protected set; }

    protected Result(bool isSuccess, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors ?? throw new InvalidOperationException(nameof(errors));
    }

    public static Result Success()
    {
        return new Result(true, Enumerable.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        if (errors == null)
            throw new ArgumentNullException(nameof(errors));
        return new Result(false, errors);
    }
}

public sealed class Result<TEntity> : Result
{
    private readonly TEntity _value;
    private Result(TEntity value, bool isSuccess, IEnumerable<string> errors)
        : base(isSuccess, errors)
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

    public static Result<TEntity> Success(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        return new Result<TEntity>(entity, true, Enumerable.Empty<string>());
    }

    public new static Result<TEntity> Failure(IEnumerable<string> errors)
    {
        if (errors == null)
            throw new ArgumentNullException(nameof(errors));
        return new Result<TEntity>(default!, false, errors);
    }
}