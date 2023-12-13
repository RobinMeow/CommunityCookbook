﻿using api.Controllers.Auth;

namespace server.tests.Auth;

public sealed class DeleteChefDtoTests : DataAnnotationTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void fails_all_member_validations(string? name, string? password)
    {
        var dto = new DeleteChefDto()
        {
            Name = name!,
            Password = password!,
        };

        Equal(2, ValidateAmountFailed(dto));
    }

    [Fact]
    public void successfully_validates_all_members()
    {
        var dto = new DeleteChefDto()
        {
            Name = "meow",
            Password = "meow",
        };

        Equal(0, ValidateAmountFailed(dto));
    }
}
