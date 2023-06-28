namespace UrlShortener.Entity;

public abstract class EntityBase
{
    public Guid Id { get; init; }
    protected EntityBase()
    {
        Id = Guid.NewGuid();
    }
}
