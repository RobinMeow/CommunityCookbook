using System.ComponentModel.DataAnnotations;
using api.Domain;

namespace api.Controllers;

public abstract class EntityDto
{
    [Required]
    public required string Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
}
