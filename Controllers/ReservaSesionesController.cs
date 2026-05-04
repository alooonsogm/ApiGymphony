using ApiGymphony.Helpers;
using ApiGymphony.Models;
using ApiGymphony.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetGymphonyAGM.Models;

namespace ApiGymphony.Controllers
{
    [Authorize(Roles = "Socio")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaSesionesController : ControllerBase
    {
        private RepositoryGymphony repo;
        private HelperUsuarioToken helper;

        public ReservaSesionesController( RepositoryGymphony repo, HelperUsuarioToken helper )
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost("[action]/{sesionId}")]
        public async Task<ActionResult> ReservarPlaza( int sesionId )
        {
            if ( sesionId <= 0 )
            {
                return BadRequest(new { status = "error", mensaje = "ID de sesión no válido." });
            }

            UsuarioTokenDTO usuarioLogueado = this.helper.GetUsuario();

            string resultado = await this.repo.ReservarPlazaAsync(sesionId, usuarioLogueado.IdUsuario);

            if ( resultado == "OK" )
            {
                return Ok(new { status = "success", mensaje = "Reserva realizada con éxito." });
            }
            else
            {
                return BadRequest(new { status = "error", mensaje = resultado });
            }
        }

        [HttpDelete("[action]/{sesionId}")]
        public async Task<ActionResult> AnularReserva( int sesionId )
        {
            if ( sesionId <= 0 )
            {
                return BadRequest(new { status = "error", mensaje = "ID de sesión no válido." });
            }

            UsuarioTokenDTO usuarioLogueado = this.helper.GetUsuario();

            string resultado = await this.repo.AnularReservaAsync(sesionId, usuarioLogueado.IdUsuario);

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
