using System.Text;
using Domain.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public sealed class IssuerSigningKeyFactory(BearerConfig bearerConfig) : IIssuerSigningKeyFactory
{
    readonly BearerConfig _bearerConfig = bearerConfig;

    public IssuerSigningKeyFactory(IConfiguration configuration)
    : this(configuration.GetSection(nameof(BearerConfig)).Get<BearerConfig>()
            ?? throw new ArgumentException($"Could not find '{nameof(BearerConfig)}' in configuration."))
    {
    }

    public SecurityKey Create()
    {
        byte[] bytes = Encoding.UTF8.GetBytes(_bearerConfig.SigningKey);
        return new SymmetricSecurityKey(bytes);
    }
}
