using ApiGymphony.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetGymphonyAGM.Models;

namespace ApiGymphony.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private RepositoryGymphony repo;

        public RolController( RepositoryGymphony repo )
        {
            this.repo = repo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> FindRol( int id )
        {
            return await this.repo.FindRolPorIdRolAsync(id);
        }
    }
}
