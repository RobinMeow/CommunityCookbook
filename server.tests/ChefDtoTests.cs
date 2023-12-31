﻿using api.Controllers;
using System.ComponentModel.DataAnnotations;

namespace server.tests;

public sealed class ChefDtoTests : DataAnnotationTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void fails_all_member_validations(string? name, string? email)
    {
        var dto = new ChefDto()
        {
            Name = name!,
            Email = email
        };

        IList<ValidationResult> validationResults = ValidationResults(dto);
        True(HasInvalidMember(validationResults, nameof(ChefDto.Name)));
        True(HasInvalidMember(validationResults, nameof(ChefDto.Email)));
    }

    [Theory]
    [InlineData("Weinberg des Herrn", "be.blessed@nd.loved")]
    [InlineData("Weinberg des Herrn", null)]
    public void successfully_validates_all_members(string name, string? email)
    {
        var dto = new ChefDto()
        {
            Name = name,
            Email = email
        };

        IList<ValidationResult> validationResults = ValidationResults(dto);
        Empty(validationResults);
    }
}
