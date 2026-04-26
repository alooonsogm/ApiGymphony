using ApiGymphony.Models;
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
        public async Task<ActionResult> RegistroSocio( SocioDTO nuevoSocio )
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
            try
            {
                await this.repo.DarDeBajaSocioAsync(idSocio);
                return Ok(new { status = "success", mensaje = "El socio ha sido dado de baja correctamente." });
            }
            catch ( Exception )
            {
                return BadRequest(new { status = "error", mensaje = "No se pudo procesar la baja. Verifica que el ID sea correcto." });
            }
        }

        [HttpPost("[action]/{idSocio}")]
        public async Task<ActionResult> DarDeAltaSocio( int idSocio )
        {
            try
            {
                await this.repo.DarDeAltaSocioAsync(idSocio);
                return Ok(new { status = "success", mensaje = "El socio ha sido dado de alta exitosamente." });
            }
            catch ( Exception )
            {
                return BadRequest(new { status = "error", mensaje = "No se pudo procesar el alta. Verifica que el ID sea correcto." });
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RegistroEntrenador( EntrenadorDTO model )
        {
            if ( model == null || model.Usuario == null )
            {
                return BadRequest(new { status = "error", mensaje = "Datos insuficientes." });
            }

            try
            {
                await this.repo.RegistroEntrenadorAsync(
                    model.Usuario.Email,
                    model.Usuario.Password,
                    model.Usuario.Nombre,
                    model.Usuario.Apellidos,
                    model.Usuario.Telefono,
                    model.Usuario.FechaNacimiento,
                    model.Usuario.Dni,
                    model.Usuario.RutaFoto,
                    model.DiasSemana,
                    model.HorasInicio,
                    model.HorasFin
                );

                return Ok(new { status = "success", mensaje = "Entrenador y horario registrados correctamente." });
            }
            catch ( Exception ex )
            {
                return BadRequest(new { status = "error", mensaje = "Error en el registro: " + ex.Message });
            }
        }

        [HttpGet("[action]/{idEntrenador}")]
        public async Task<ActionResult> ValidarBorradoEntrenador( int idEntrenador )
        {
            try
            {
                bool tieneSesiones = await this.repo.EntrenadorTieneSesionesAsync(idEntrenador);
                var sustitutos = await this.repo.GetEntrenadoresSustitutosAsync(idEntrenador);

                var listaSustitutos = sustitutos.Select(s => new {
                    id = s.IdUsuario,
                    nombre = s.Nombre + " " + s.Apellidos
                }).ToList();

                return Ok(new
                {
                    success = true,
                    hasSessions = tieneSesiones,
                    sustitutos = listaSustitutos
                });
            }
            catch ( Exception ex )
            {
                return BadRequest(new { success = false, message = "Error al validar: " + ex.Message });
            }
        }

        [HttpDelete("[action]/{idEntrenadorABorrar}")]
        public async Task<ActionResult> DeleteEntrenadorSustituyendo( int idEntrenadorABorrar, [FromQuery] int? idEntrenadorSustituto )
        {
            if ( idEntrenadorABorrar <= 0 )
            {
                return BadRequest(new { status = "error", mensaje = "ID de entrenador no válido." });
            }

            try
            {
                await this.repo.DeleteEntrenadorSustituyendoAsync(idEntrenadorABorrar, idEntrenadorSustituto);

                return Ok(new { status = "success", mensaje = "El entrenador ha sido eliminado correctamente de la plataforma." });
            }
            catch ( Exception ex )
            {
                return BadRequest(new { status = "error", mensaje = "Ocurrió un error al intentar borrar el entrenador: " + ex.Message });
            }
        }

        [HttpGet("[action]/{idUsuario}")]
        public async Task<ActionResult<VistaUsuario>> FindVistaUsuario( int idUsuario )
        {
            return await this.repo.FindVistaUsuarioAsync(idUsuario);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<DatosEvolucion>>> GetEvolucionSocios()
        {
            try
            {
                List<DatosEvolucion> evolucion = await this.repo.GetEvolucionSociosAsync();
                return Ok(evolucion);
            }
            catch ( Exception ex )
            {
                return BadRequest(new
                {
                    status = "error",
                    mensaje = "Error al generar los datos de evolución de altas y bajas: " + ex.Message
                });
            }
        }

        [HttpGet("[action]/{idSocio}")]
        public async Task<ActionResult<List<string>>> GetDiasAsistenciaSocio( int idSocio )
        {
            try
            {
                List<string> diasAsistidos = await this.repo.GetDiasAsistenciaSocioAsync(idSocio);
                return Ok(diasAsistidos);
            }
            catch ( Exception ex )
            {
                return BadRequest(new
                {
                    status = "error",
                    mensaje = "Error al obtener el historial de asistencia: " + ex.Message
                });
            }
        }
    }
}
