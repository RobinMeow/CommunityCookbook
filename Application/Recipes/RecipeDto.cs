using System.ComponentModel.DataAnnotations;
using Common;
using Domain;

namespace Application.Recipes;

public sealed record RecipeDto : EntityDto
{
    [Required]
    [StringRange(RecipeValidators.TITLE_MIN_LENGTH, RecipeValidators.TITLE_MAX_LENGTH)]
    public string Title { get; set; } = null!;
}