using ApiGymphony.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetGymphonyAGM.Models;

namespace ApiGymphony.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private RepositoryGymphony repo;

        public UsuariosController( RepositoryGymphony repo )
        {
            this.repo = repo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> FindUsuario( int id )
        {
            return await this.repo.FindUsuarioAsync(id);
        }

        [HttpGet("[action]/{idSesion}")]
        public async Task<ActionResult<List<Usuario>>> GetUsuariosPorSesion( int idSesion )
        {
            return await this.repo.GetUsuariosPorSesionAsync(idSesion);
        }

        [HttpGet("[action]/{rol}")]
        public async Task<ActionResult<List<VistaUsuario>>> GetUsuariosPorRol( string rol )
        {
            return await this.repo.GetUsuariosPorRolAsync(rol);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<VistaSocio>>> GetSociosConEstado()
        {
            return await this.repo.GetSociosConEstadoAsync();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<Usuario>>> GetEntrenadores()
        {
            return await this.repo.GetTodosEntrenadoresAsync();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RegistroSocio( Usuario nuevoSocio )
        {
            if ( nuevoSocio == null )
            {
                return BadRequest(new { status = "error", mensaje = "Los datos de registro son obligatorios." });
            }
            await this.repo.RegistroSocioAsync(
                nuevoSocio.Email,
                nuevoSocio.Password,
                nuevoSocio.Nombre,
                nuevoSocio.Apellidos,
                nuevoSocio.Telefono,
                nuevoSocio.FechaNacimiento,
                nuevoSocio.Dni,
                nuevoSocio.RutaFoto
            );

            return Ok(new { status = "success", mensaje = "Socio registrado correctamente." });
        }

        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult> DeleteSocio( int id )
        {
            await this.repo.DeleteSocioAsync(id);
            return Ok();
        }

        [HttpPut("[action]/{idSocio}")]
        public async Task<ActionResult> DarDeBajaSocio( int idSocio )
        {
            await this.repo.DarDeBajaSocioAsync(idSocio);
            return Ok(new { status = "success", mensaje = "El socio ha sido dado de baja correctamente." });
        }

        [HttpPost("[action]/{idSocio}")]
        public async Task<ActionResult> DarDeAltaSocio( int idSocio )
        {
            await this.repo.DarDeAltaSocioAsync(idSocio);
            return Ok(new { status = "success", mensaje = "El socio ha sido dado de alta exitosamente." });
        }
    }
}
