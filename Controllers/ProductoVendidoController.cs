using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaDeGestion.Models;
using SistemaDeGestion.Repositories;

namespace SistemaDeGestion.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductoVendidoController : Controller
    {
        private ProductoVendidoRepository repository = new ProductoVendidoRepository();

        [HttpGet]
        public ActionResult<List<ProductoVendido>> Get()
        {
            try
            {
                List<ProductoVendido> lista = repository.listarProductoVendido();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult Delete([FromBody] int id)
        {
            try
            {
                bool seElimino = repository.eliminarProductoVendido(id);
                if (seElimino)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [HttpPost]
        public ActionResult Post([FromBody] ProductoVendido productoVendido)
        {
            try
            {
                repository.crearProductoVendido(productoVendido);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
        [HttpPut("{id}")]
        public ActionResult<ProductoVendido> Put(long id, [FromBody] ProductoVendido ProductoVendidoAActualizar)
        {
            try
            {
                ProductoVendido? productoVendidoActualizado = repository.actualizarProductoVendido(id, ProductoVendidoAActualizar);
                if (productoVendidoActualizado != null)
                {
                    return Ok(productoVendidoActualizado);
                }
                else
                {
                    return NotFound("ProductoVendido no encontrado...");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        
    }
}
