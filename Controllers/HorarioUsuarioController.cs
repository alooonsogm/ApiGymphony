using ApiGymphony.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetGymphonyAGM.Models;

namespace ApiGymphony.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioUsuarioController : ControllerBase
    {
        private RepositoryGymphony repo;

        public HorarioUsuarioController( RepositoryGymphony repo )
        {
            this.repo = repo;
        }

        [HttpGet]
        [Route("[action]/{idUsuario}")]
        public async Task<ActionResult<List<HorarioEmpleados>>> GetHorarioUsuario( int idUsuario )
        {
            return await this.repo.GetHorarioUsuarioPorIdAsync(idUsuario);
        }
    }
}
