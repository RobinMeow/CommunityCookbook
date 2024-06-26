﻿using Domain.Auth;
using Infrastructure.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure_specs;

public sealed class IssuerSigningKeyFactoryTests
{
    [Fact]
    public void creates_security_key()
    {
        var bearerConfig = new BearerConfig()
        {
            SigningKey = "not empty key"
        };

        var issuerSigningKeyFactory = new IssuerSigningKeyFactory(bearerConfig);
        SecurityKey securityKey = issuerSigningKeyFactory.Create();
        NotNull(securityKey);
    }

    [Fact]
    public void throws_with_invalid_bearer_config()
    {
        var bearerConfig = new BearerConfig()
        {
            SigningKey = ""
        };

        var issuerSigningKeyFactory = new IssuerSigningKeyFactory(bearerConfig);
        ThrowsAny<Exception>(issuerSigningKeyFactory.Create);
    }
}
