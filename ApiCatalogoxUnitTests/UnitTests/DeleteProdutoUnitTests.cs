using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests;

public class DeleteProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public DeleteProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    // Testes para o Delete 

    [Fact]
    public async Task DeleteProdutoById_Return_OkResult()
    {
        // Arrange
        var prodId = 2;

        //Act
        var result = await _controller.Delete(prodId) as ActionResult<ProdutoDTO>;

        //Assert
        result.Should().NotBeNull(); // Verifica se o resultadoé nulo
        result.Result.Should().BeOfType<OkObjectResult>(); // Verifica se o resultado é okResult
    }

    [Fact]
    public async Task DeleteProdutoById_Return_NotFound()
    {
        //Arrange
        var prodId = 999;

        //Act
        var result = await _controller.Delete(prodId) as ActionResult<ProdutoDTO>;

        // Assert
        result.Should().NotBeNull(); // Verifica se o resultadoé nulo
        result.Result.Should().BeOfType<NotFoundObjectResult>(); // Verifica se o resultado é NotFound
    }
}
