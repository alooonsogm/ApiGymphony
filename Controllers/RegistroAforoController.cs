using ApiGymphony.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetGymphonyAGM.Models;

namespace ApiGymphony.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroAforoController : ControllerBase
    {
        private RepositoryGymphony repo;

        public RegistroAforoController( RepositoryGymphony repo )
        {
            this.repo = repo;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<int>> GetAforoActual()
        {
            try
            {
                int aforo = await this.repo.GetAforoActualAsync();
                return Ok(aforo);
            }
            catch ( Exception ex )
            {
                return BadRequest(new { status = "error", mensaje = "Error al obtener el aforo actual: " + ex.Message });
            }
        }

        [HttpPost("[action]/{idSocio}")]
        public async Task<ActionResult> RegistrarAcceso( int idSocio )
        {
            try
            {
                string accionRealizada = await this.repo.RegistrarAccesoAsync(idSocio);

                if ( accionRealizada == "ENTRADA" )
                {
                    return Ok(new
                    {
                        status = "success",
                        tipo = "ENTRADA",
                        mensaje = "Acceso permitido. ¡Bienvenido a Gymphony!"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = "success",
                        tipo = "SALIDA",
                        mensaje = "Salida registrada. ¡Hasta pronto!"
                    });
                }
            }
            catch ( Exception ex )
            {
                return BadRequest(new
                {
                    status = "error",
                    mensaje = "Error al procesar el acceso."
                });
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<DatosHoraPico>>> GetHorasPico()
        {
            try
            {
                List<DatosHoraPico> horasPico = await this.repo.GetHorasPicoAsync();

                if ( horasPico == null || horasPico.Count == 0 )
                {
                    return Ok(new List<DatosHoraPico>());
                }

                return Ok(horasPico);
            }
            catch ( Exception ex )
            {
                return BadRequest(new { status = "error", mensaje = "Error al calcular las estadísticas de horas pico: " + ex.Message });
            }
        }
    }
}
