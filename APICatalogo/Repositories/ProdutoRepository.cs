using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;
using X.PagedList.Extensions;



namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    { }

    public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
    {
        var produtos = await GetAllAsync();

        var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();

        var resultado = produtosOrdenados.ToPagedList(produtosParams.PageNumber,
                                                      produtosParams.PageSize);

        return resultado;
    }
    public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
    {
        var produtos = await GetAllAsync();

        if (produtosFiltroParams.Preco.HasValue && !
            string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
        {
            if (produtosFiltroParams.PrecoCriterio.Equals("maior",
                               StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value)
                                                                    .OrderBy(p => p.Preco);
            }
            else if (produtosFiltroParams.PrecoCriterio.Equals("menor",
                                    StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value)
                                                                   .OrderBy(p => p.Preco);
            }
            else if (produtosFiltroParams.PrecoCriterio.Equals("igual",
                                     StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value)
                                                                     .OrderBy(p => p.Preco);
            }
        }
        var produtosFiltrados = produtos.ToPagedList((int)produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);

        return produtosFiltrados;
    }
    public async Task<IEnumerable<Produto>> GetProdutosPorCategoriasAsync(int id)
    {
        var produtos = await GetAllAsync();
        var produtosCategorias = produtos.Where(p => p.CategoriaId == id);
        return produtosCategorias;
    }
}
