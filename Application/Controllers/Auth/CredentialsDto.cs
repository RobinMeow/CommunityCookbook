using System.ComponentModel.DataAnnotations;

namespace Application.Auth;

public record class CredentialsDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
