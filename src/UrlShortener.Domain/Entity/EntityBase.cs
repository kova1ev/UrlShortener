using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entity;

public abstract class EntityBase
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; protected set; }
    protected EntityBase()
    {
        Id = Guid.NewGuid();
    }
}
