using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    
    public partial class PopulaProdutos : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " 
                + "Values('Coca-Cola Diet','Refrigerante de cola 350ml',5.45,'coca-cola.jpg',50,now(),1)");

            migrationBuilder.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) "
                + "Values('Lanche de Atum','Lanche de atum com maionese',8.50,'atum.jpg',10,now(),2)");

            migrationBuilder.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) "
                + "Values('Pudim 100 g','Pudim de leite condensado',6.75,'pudim.jpg',20,now(),3)");
        }

         
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Produtos");
        }
    }
}
