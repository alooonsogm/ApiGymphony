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
        [Route("[action]/{id}")]
        public async Task<ActionResult<List<HorarioEmpleados>>> GetHorarioEntrenador( int id )
        {
            return await this.repo.GetHorarioUsuarioPorIdAsync(id);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<List<HorarioEmpleados>>> GetHorarioEntrenadorOrdenado( int id )
        {
            return await this.repo.GetHorariosEntrenadorAsync(id);
        }
    }
}
