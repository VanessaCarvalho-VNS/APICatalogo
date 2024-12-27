using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;


namespace APICatalogo.Repositories;
public interface IProdutoRepository : IRepository<Produto>
{
    Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
    Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams);
    Task<IEnumerable<Produto>> GetProdutosPorCategoriasAsync(int id);
}
