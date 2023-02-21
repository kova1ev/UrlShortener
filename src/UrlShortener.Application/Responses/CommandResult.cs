namespace UrlShortener.Application.Responses;

//todo вставить в команды 
public class CommandResult<TEntity> : CommandResult
{
    public virtual TEntity? ReturnedObject { get; set; }
}

public class CommandResult
{
    public virtual bool IsSuccess { get; set; } = true;
    public virtual string ErrorMessage { get; set; } = string.Empty;
}
