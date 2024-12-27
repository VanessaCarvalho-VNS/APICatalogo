using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests;

public class GetProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public GetProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task GetProdutoById_OKResult()
    {
        // Arrange
        var prodId = 2;

        // Act
        var data = await _controller.Get(prodId);

        // Assert (xUnit)
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        // Assert (fluentassertions)
        data.Result.Should().BeOfType<OkObjectResult>() //verifica se o resultado é do tipo okObjectresult.
            .Which.StatusCode.Should().Be(200); // verificar se p códigp de status do okObjectResult é 200.
    }

    [Fact]
    public async Task GetProdutoById_Return_BadRequest()
    {
        //Arrange
        var prodId = -1;

        //Act
        var data = await _controller.Get( prodId);

        //Assert
        data.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode
            .Should()
            .Be(400);
    }

    [Fact]
    public async Task GetProdutoById_Return_NotFound()
    {
        //Arrange
        var prodId = 999;

        //Act
        var data = await _controller.Get( prodId);

        //Assert
        data.Result.Should().BeOfType<NotFoundObjectResult>()
            .Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetProdutos_Return_ListOfProdutosDTO()
    {
        // Act
        var data = await _controller.Get();

        //Assert
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should()
            .BeAssignableTo<IEnumerable<ProdutoDTO>>()//Verifica se o valor do OkObjectResult é atribuível a IEnumerable<ProdutosDTO>.
            .And.NotBeNull();
    }

    [Fact]
    public async Task GetProduto_Return_BadRequest()
    {
        //Act
        var data = await _controller.Get();

        //Assert
        data.Result.Should().BeOfType<BadRequestResult>();
    }

}
