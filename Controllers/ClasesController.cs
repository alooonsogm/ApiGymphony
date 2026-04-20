using ApiGymphony.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetGymphonyAGM.Models;

namespace ApiGymphony.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClasesController : ControllerBase
    {
        private RepositoryGymphony repo;

        public ClasesController( RepositoryGymphony repo )
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Clases>>> GetClases()
        {
            return await this.repo.GetTodasClasesAsync();
        }
    }
}
