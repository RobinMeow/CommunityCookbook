using api.Controllers.Recipes;
using api.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Recipes;

public sealed class RecipeControllerTests
{
    readonly RecipeController _recipeController;

    public RecipeControllerTests()
    {
        DbContext dbContext = Substitute.For<DbContext>();

        _recipeController = new RecipeController(
            dbContext,
            Substitute.For<ILogger<RecipeController>>()
        );
    }

    [Fact]
    public async Task AddAsync_returns_created_with_valid_request_dto()
    {
        var requestDto = new NewRecipeDto()
        {
            Title = "My Recipe",
        };

        ActionResult<RecipeDto> createdResult = await _recipeController.AddAsync(requestDto);
        IsType<CreatedResult>(createdResult.Result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task AddAsync_returns_BadReuqest_with_invalid_request_dto(string? title)
    {
        var requestDto = new NewRecipeDto()
        {
            Title = title!,
        };

        ActionResult<RecipeDto> result = await _recipeController.AddAsync(requestDto);
        
        IsType<BadRequestObjectResult>(result.Result);
        BadRequestObjectResult objectResult = (BadRequestObjectResult)result.Result;

        IsType<NewRecipeDto>(objectResult.Value);
        NotNull(objectResult.Value);

        NewRecipeDto dto = (NewRecipeDto)objectResult.Value;

        True(dto.HasErrors());
    }

    [Fact]
    public async Task GetAllAsync_returns_OkObject()
    {
        ActionResult<IEnumerable<RecipeDto>> result = await _recipeController.GetAllAsync();
        IsType<OkObjectResult>(result.Result);
    }
}
