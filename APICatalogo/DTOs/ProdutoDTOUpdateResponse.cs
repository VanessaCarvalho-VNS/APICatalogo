using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.DTOs
{
    public class ProdutoDTOUpdateResponse
    {
        public int ProdutoId { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public string? ImagemUrl { get; set; }
        public int CategoriaId { get; set; }
    }
}
