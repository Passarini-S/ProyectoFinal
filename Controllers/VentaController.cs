using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaDeGestion.Models;
using SistemaDeGestion.Repositories;

namespace SistemaDeGestion.Controllers
{
        [ApiController]
        [Route("api/v1/[controller]")]
        public class VentaController : Controller
        {
            private VentaRepository repository = new VentaRepository();

            [HttpGet]
            public ActionResult<List<Venta>> Get()
            {
                try
                {
                    List<Venta> lista = repository.listarVenta();
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
                bool seElimino = repository.eliminarVenta(id);
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
        public ActionResult Post([FromBody] Venta venta)
        {
            try
            {
                repository.crearVenta(venta);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
        [HttpPut("{id}")]
        public ActionResult<Venta> Put(long id, [FromBody] Venta VentaAActualizar)
        {
            try
            {
                Venta? ventaActualizado = repository.actualizarVenta(id, VentaAActualizar);
                if (ventaActualizado != null)
                {
                    return Ok(ventaActualizado);
                }
                else
                {
                    return NotFound("Venta no encontrado...");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


    }

}
