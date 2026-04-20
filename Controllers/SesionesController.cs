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
    }
}
