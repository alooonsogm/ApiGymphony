using ApiGymphony.Models;
using ApiGymphony.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetGymphonyAGM.Models;

namespace ApiGymphony.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionesController : ControllerBase
    {
        private RepositoryGymphony repo;

        public SesionesController(RepositoryGymphony repo )
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<DatosSesion>>> GetSesiones()
        {
            return await this.repo.GetTodasSesionesAsync();
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Sesion>> FindSesion( int id )
        {
            return await this.repo.FindSesionAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post( Sesion sesion )
        {
            await this.repo.CreateSesionesAsync(sesion.ClaseId, sesion.EntrenadorId, sesion.SalaId, sesion.Fecha, sesion.HoraInicio, sesion.HoraFin);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put( Sesion sesion )
        {
            await this.repo.UpdateSesionAsync(sesion.IdSesion, sesion.ClaseId, sesion.EntrenadorId, sesion.SalaId, sesion.Fecha, sesion.HoraInicio, sesion.HoraFin);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete( int id )
        {
            await this.repo.DeleteSesionAsync(id);
            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<DatosSesion>> FindDatosSesion( int id )
        {
            return await this.repo.FindDatosSesionAsync(id);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<DatosSesion>>> GetSesionesNuevas()
        {
            return await this.repo.GetSesionesNuevasAsync();
        }

        [HttpGet]
        [Route("[action]/{idCliente}")]
        public async Task<ActionResult<List<DatosSesion>>> GetMisFuturasSesiones( int idCliente )
        {
            return await this.repo.GetMisFuturasSesionesCompletasAsync(idCliente);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ValidarSesion( ValidarSesionDTO model )
        {
            if ( model == null )
            {
                return BadRequest(new { status = "error", mensaje = "Faltan datos para validar." });
            }

            try
            {
                string resultado = await this.repo.ValidarSesionAsync(model.Fecha, model.HoraInicio, model.HoraFin, model.IdEntrenador, model.IdSala, model.IdSesionActual);

                if ( resultado == "OK" )
                {
                    return Ok(new { esValida = true, mensaje = "La sesión se puede programar correctamente." });
                }
                else
                {
                    return Ok(new { esValida = false, mensaje = resultado });
                }
            }
            catch ( Exception ex )
            {
                return BadRequest(new { status = "error", mensaje = "Error interno al validar: " + ex.Message });
            }
        }
    }
}
