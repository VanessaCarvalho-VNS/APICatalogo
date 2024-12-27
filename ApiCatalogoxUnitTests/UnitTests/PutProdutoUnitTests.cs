using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.Models;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PutProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        // Testes de unidade para PUT

        [Fact]
        public async Task PutProduto_Return_OkResult()
        {
            //Arrange
            var prodId = 3;

            var updatedProdutoDto = new ProdutoDTO
            {
                ProdutoId = prodId,
                Nome = "Produto atualizado - Teste",
                Descricao = "Minha Descrição",
                ImagemUrl = "imagem1.jpg",
                CategoriaId = 2
            };

            //Act
            var result = await _controller.Put(prodId, updatedProdutoDto) as ActionResult<ProdutoDTO>;

            //Assert
            result.Should().NotBeNull(); //Verifica se o resultado não é nulo
            result.Result.Should().BeOfType<OkObjectResult>(); // Verifica se o resultado é OkObjectResult
        }

        [Fact]
        public async Task PutProduto_Return_BadRequest()
        {
            //Arrange
            var ProdId = 1000;

            var meuProduto = new ProdutoDTO
            {
                ProdutoId = 14,
                Nome = "Produto atualizado - Teste",
                Descricao = "Minha Descrição alterada",
                ImagemUrl = "imagem1.jpg",
                CategoriaId = 2
            };

            //Act
            var data = await _controller.Put(ProdId, meuProduto);

            //Assert
            data.Result.Should().BeOfType<BadRequestResult>()
                .Which.StatusCode.Should().Be(400);
            
        }
    }
}
