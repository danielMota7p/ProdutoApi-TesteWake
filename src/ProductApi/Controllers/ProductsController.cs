using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProductApplication.DTOs;
using ProductApplication.Services;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protege todos por padrão (ajuste as exceções com [AllowAnonymous])
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista produtos com paginação, ordenação e busca por nome.
        /// Ex.: GET /api/products?nome=bola&sortby=valor&order=desc&page=1&pagesize=10
        /// </summary>
        [HttpGet]
        [AllowAnonymous] // deixe público para facilitar testes; remova depois se quiser tudo protegido
        [ProducesResponseType(typeof(PagedResult<ProductResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductResponseDto>>> GetAsync([FromQuery] ProductQueryParams query, CancellationToken ct)
        {
            var result = await _service.GetAsync(query, ct);
            return Ok(result);
        }

        /// <summary>
        /// Obtém um produto por ID.
        /// </summary>
        [HttpGet("{id:int}", Name = "GetProductById")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductResponseDto>> GetByIdAsync([FromRoute] int id, CancellationToken ct)
        {
            var product = await _service.GetByIdAsync(id, ct);
            if (product is null)
                return NotFound(Problem(title: "Produto não encontrado", statusCode: StatusCodes.Status404NotFound));

            return Ok(product);
        }


        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> CreateAsync([FromBody] ProductRequestDto dto, CancellationToken ct)
        {
            try
            {
                var created = await _service.CreateAsync(dto, ct);
                return CreatedAtRoute("GetProductById", new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(Problem(title: "Falha de validação de negócio", detail: ex.Message, statusCode: StatusCodes.Status400BadRequest));
            }
        }


        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductResponseDto>> UpdateAsync([FromRoute] int id, [FromBody] ProductRequestDto dto, CancellationToken ct)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto, ct);
                if (updated is null)
                    return NotFound(Problem(title: "Produto não encontrado", statusCode: StatusCodes.Status404NotFound));

                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(Problem(title: "Falha de validação de negócio", detail: ex.Message, statusCode: StatusCodes.Status400BadRequest));
            }
        }

        /// <summary>
        /// Exclui um produto.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken ct)
        {
            var ok = await _service.DeleteAsync(id, ct);
            if (!ok)
                return NotFound(Problem(title: "Produto não encontrado", statusCode: StatusCodes.Status404NotFound));

            return NoContent();
        }
    }
}
