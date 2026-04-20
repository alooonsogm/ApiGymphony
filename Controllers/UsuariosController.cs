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

        [HttpGet]
        [Route("[action]/{idSesion}")]
        public async Task<ActionResult<List<Usuario>>> GetUsuariosPorSesion( int idSesion )
        {
            return await this.repo.GetUsuariosPorSesionAsync(idSesion);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Usuario>>> GetEntrenadores()
        {
            return await this.repo.GetTodosEntrenadoresAsync();
        }
    }
}
