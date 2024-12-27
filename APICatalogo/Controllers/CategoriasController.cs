using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;
using X.PagedList;


namespace APICatalogo.Controllers
{
    [EnableCors("OrigensComAcessoPermitido")]
    [Route("[controller]")]
    [ApiController]
    [EnableRateLimiting("fixedwindow")]
    [Produces("application/json")]
    //Inibe a exibição da documentação para os endpoints
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriasController> _logger;
        public CategoriasController(ILogger<CategoriasController> logger, IUnitOfWork uof)
        {
            _logger = logger;
            _uof = uof;
        }

        /// <summary>
        /// Obtem uma lista de objetos Categorias
        /// </summary>
        /// <returns>Uma lista de objetos Categoria</returns>


         
        //[Authorize]
        [HttpGet]
        [DisableRateLimiting]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync();
        
            if (categorias is null)
                return NotFound("Não existem categorias...");

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery]
                               CategoriasParameters categoriasParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriasParameters);

            return ObterCategorias(categorias);
        }

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas(
                                   [FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categoriasFiltradas =  await _uof.CategoriaRepository
                                         .GetCategoriasFiltroNomeAsync(categoriasFiltro);

            return ObterCategorias(categoriasFiltradas);

        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var categoriasDto = categorias.ToCategoriaDTOList();
            return Ok(categoriasDto);
        }

        /// <summary>
        /// Obtem uma Categoria pelo seu id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objetos Categoria</returns>
        //[DisableCors]
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        //[Route("api/v{version:apiVersion}/ObterCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrado...");
            }

            var categoriaDto = categoria.ToCategoriaDTO();

            return Ok(categoriaDto);

        }

        /// <summary>
        /// Inclui uma nova Categorias
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        /// POST api/categorias
        /// {
        ///     "categoriaId": 1,
        ///     "nome": "categoria1",
        ///     "imagemUrl": "http://teste.net/1.jpg"
        /// }
        /// </remarks>
        /// <param name="categoriaDto">objeto Categoria</param>
        /// <returns>O objetos Categoria incluida</returns>
        /// <remarks>Retorna um objeto Categoria incluido</remarks>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos!");
            }
            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            await _uof.CommitAsync();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();
            return new CreatedAtRouteResult("ObterCategoria",
                new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();

            var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
            

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrado...");
            }
            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            await _uof.CommitAsync();

            var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaExcluidaDto);
        }
    }
}








