using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Responses;

//todo вставить в команды 
public class CommandResultResponse<TEntity> where TEntity : EntityBase
{
    public virtual TEntity? ObjectResult { get; set; }
    public bool Success { get; set; }
    public ICollection<string>? Errors { get; set; }
}
