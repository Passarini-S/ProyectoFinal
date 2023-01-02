using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaDeGestion.Models;
using SistemaDeGestion.Repositories;

namespace SistemaDeGestion.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsuarioController : Controller
    {
        private UsuarioRepository repository = new UsuarioRepository();

        [HttpGet]
        public ActionResult<List<Usuario>> Get()
        {
            try
            {
                List<Usuario> lista = repository.listarUsuario();
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
                bool seElimino = repository.eliminarUsuario(id);
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
        public ActionResult Post([FromBody] Usuario usuario)
        {
            try
            {
                repository.crearUsuario(usuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
        [HttpPut("{id}")]
        public ActionResult<Usuario> Put(long id, [FromBody] Usuario UsuarioAActualizar)
        {
            try
            {
                Usuario? usuarioActualizado = repository.actualizarUsuario(id, UsuarioAActualizar);
                if (usuarioActualizado != null)
                {
                    return Ok(usuarioActualizado);
                }
                else
                {
                    return NotFound("Usuario no encontrado...");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
