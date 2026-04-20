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

        [HttpGet("{idSesion}")]
        public async Task<ActionResult<DatosSesion>> FindDatosSesion( int idSesion )
        {
            return await this.repo.FindDatosSesionAsync(idSesion);
        }

        [HttpGet]
        [Route("[action]")]
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
