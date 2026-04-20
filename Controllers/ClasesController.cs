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

        [HttpGet("{id}")]
        public async Task<ActionResult<Clases>> FindClase( int id )
        {
            return await this.repo.FindClasesAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post( Clases clase )
        {
            await this.repo.CreateClasesAsync(clase.Nombre, clase.Descripcion);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put( Clases clase )
        {
            await this.repo.UpdateClasesAsync(clase.IdClases, clase.Nombre, clase.Descripcion);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete( int id )
        {
            await this.repo.DeleteClasesAsync(id);
            return Ok();
        }
    }
}
