using ApiGymphony.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetGymphonyAGM.Models;

namespace ApiGymphony.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaSesionesController : ControllerBase
    {
        private RepositoryGymphony repo;

        public ReservaSesionesController( RepositoryGymphony repo )
        {
            this.repo = repo;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ReservarPlaza( ReservaSesiones reserva )
        {
            if ( reserva == null )
            {
                return BadRequest(new { status = "error", mensaje = "Datos de la reserva incorrectos." });
            }

            string resultado = await this.repo.ReservarPlazaAsync(reserva.SesionId, reserva.ClienteId);

            if ( resultado == "OK" )
            {
                return Ok(new { status = "success", mensaje = "Reserva realizada con éxito." });
            }
            else
            {
                return BadRequest(new { status = "error", mensaje = resultado });
            }
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> AnularReserva( ReservaSesiones reserva )
        {
            if ( reserva == null )
            {
                return BadRequest(new { status = "error", mensaje = "Datos de la reserva incorrectos." });
            }

            string resultado = await this.repo.AnularReservaAsync(reserva.SesionId, reserva.ClienteId);

            if ( resultado == "OK_ANULADA" )
            {
                return Ok(new { status = "success", mensaje = "Reserva cancelada con éxito." });
            }
            else
            {
                return BadRequest(new { status = "error", mensaje = resultado });
            }
        }
    }
}
