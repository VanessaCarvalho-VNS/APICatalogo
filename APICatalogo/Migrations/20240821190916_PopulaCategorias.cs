using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categorias(Nome, ImagemUrl) Values('Bebidas','bebidas.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias(Nome, ImagemUrl) Values('Lanches','lanches.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias(Nome, ImagemUrl) Values('Sobremesas','sobemesas.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categorias");
        }
    }
}
