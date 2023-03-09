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

public sealed class Result<TValue> : Result
{
    private readonly TValue _value;
    private Result(TValue value, bool isSuccess, IEnumerable<string> errors)
        : base(isSuccess, errors)
    {
        _value = value;
    }

    public TValue Value
    {
        get
        {
            return IsSuccess ? _value : throw new InvalidOperationException($"{nameof(Value)} is null");
        }
    }

    public bool HasValue => IsSuccess;

    public static Result<TValue> Success(TValue entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        return new Result<TValue>(entity, true, Enumerable.Empty<string>());
    }

    public new static Result<TValue> Failure(IEnumerable<string> errors)
    {
        if (errors == null)
            throw new ArgumentNullException(nameof(errors));
        return new Result<TValue>(default!, false, errors);
    }
}