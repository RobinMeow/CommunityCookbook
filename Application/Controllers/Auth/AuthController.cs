using System.ComponentModel.DataAnnotations;
using Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Auth;

[ApiController]
[Route("[controller]")]
public sealed class AuthController(
    IChefRepository chefRepository,
    ILogger<AuthController> logger,
    IPasswordHasher passwordHasher,
    IJwtFactory jwtFactory
    ) : ControllerBase
{
    readonly IChefRepository _chefRepository = chefRepository;
    readonly ILogger<AuthController> _logger = logger;
    readonly IPasswordHasher _passwordHasher = passwordHasher;
    readonly IJwtFactory _jwtFactory = jwtFactory;

    /// <summary>sign up a new account.</summary>
    /// <param name="newChef">the data to create an account from.</param>
    /// <param name="cancellationToken"></param>
    [HttpPost(nameof(RegisterAsync))]
    public async Task<Results<Created<ChefDto>, BadRequest<string>, StatusCodeHttpResult>> RegisterAsync(
        [Required, FromBody] RegisterChefDto newChef,
        CancellationToken cancellationToken = default)
    {
        string chefname = newChef.Name;

        cancellationToken.ThrowIfCancellationRequested();
        Chef? chefWithSameName = await _chefRepository.GetByNameAsync(chefname, cancellationToken);

        if (chefWithSameName != null)
            return TypedResults.BadRequest($"Chefname ist bereits vergeben.");

        if (newChef.Email != null)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Chef? chefWithSameEmail = await _chefRepository.GetByEmailAsync(newChef.Email, cancellationToken);

            if (chefWithSameEmail != null)
                return TypedResults.BadRequest($"Email ist bereits vergeben.");
        }

        var chef = new Chef()
        {
            Id = EntityId.New(),
            Name = chefname,
            Email = newChef.Email
        };
            
        chef.SetPassword(newChef.Password, _passwordHasher);

        cancellationToken.ThrowIfCancellationRequested();
        await _chefRepository.AddAsync(chef, cancellationToken);

        return TypedResults.Created(nameof(RegisterAsync), new ChefDto
        {
            Id = chef.Id.ToString(),
            Name = chef.Name,
            Email = chef.Email,
            CreatedAt = chef.CreatedAt,
        });
    }

    /// <summary>log in using an existing account.</summary>
    /// <param name="credentials">credentials to check against and generate a JWT from.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>a JWT for client side usage to keep the user logged in over a longer period of time.</returns>
    [HttpPost(nameof(LoginAsync))]
    public async Task<Results<Ok<string>, BadRequest<string>>> LoginAsync(
        [Required, FromBody] CredentialsDto credentials,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Chef? chef = await _chefRepository.GetByNameAsync(credentials.Name, cancellationToken);

        if (chef == null)
        {
            return TypedResults.BadRequest("Benutzer existiert nicht.");
        }

        PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(chef.PasswordHash, credentials.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return TypedResults.BadRequest("Falsches Passwort.");
        }

        string token = _jwtFactory.Create(chef);

        return TypedResults.Ok(token);
    }

    /// <summary>delete an existing account.</summary>
    /// <param name="credentials">the credentials to check against which account to delete and if the provided password matches the account.</param>
    /// <param name="cancellationToken"></param>
    [HttpPost(nameof(DeleteAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteAsync([Required] CredentialsDto credentials, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Chef? chef = await _chefRepository.GetByNameAsync(credentials.Name, cancellationToken);

        if (chef == null)
        {
            return BadRequest("User not found.");
        }

        PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(chef.PasswordHash, credentials.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return BadRequest("Invalid password.");
        }

        cancellationToken.ThrowIfCancellationRequested();
        await _chefRepository.RemoveAsync(credentials.Name, cancellationToken);

        return Ok();
    }
}
