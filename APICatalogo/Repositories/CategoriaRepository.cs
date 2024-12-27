using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;
using X.PagedList.Extensions;


namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    { }

    public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParams)
    {
        var categorias = await GetAllAsync();

        var categoriasOrdenadas = categorias.OrderBy(p => p.CategoriaId).AsQueryable();

        // OrderBy síncrono
        //var resultado = IPagedList<Categoria>.ToPageList(categoriasOrdenadas, 
        //    categoriasParams.PageNumber, categoriasParams.PageSize);

        var resultado = categoriasOrdenadas.ToPagedList(categoriasParams.PageNumber, categoriasParams.PageSize);
        
        return resultado;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParams)
    {
        var categorias = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriasParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParams.Nome));
        }

        //var categoriasFiltradas = PagedList<Categoria>
        //    .ToPagedList(categorias.AsQueryable(), 
        //    categoriasParams.PageNumber, 
        //    categoriasParams.PageSize);

        var categoriasFiltradas = categorias.ToPagedList(categoriasParams.PageNumber, 
            categoriasParams.PageSize);

        return categoriasFiltradas;


    }
    
}

    

