using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entity;

public abstract class EntityBase
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public EntityBase()
    {
        Id = Guid.NewGuid();
    }
}
