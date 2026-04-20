using ApiGymphony.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetGymphonyAGM.Models;

namespace ApiGymphony.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController : ControllerBase
    {
        private RepositoryGymphony repo;

        public SalasController( RepositoryGymphony repo )
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Salas>>> GetSalas()
        {
            return await this.repo.GetTodasSalasAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Salas>> FindSala( int id )
        {
            return await this.repo.FindSalasAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post( Salas sala )
        {
            await this.repo.CreateSalaAsync(sala.Nombre, sala.CapacidadMaxima);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put( Salas sala )
        {
            await this.repo.UpdateSalaAsync(sala.IdSalas, sala.Nombre, sala.CapacidadMaxima);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete( int id )
        {
            await this.repo.DeleteSalasAsync(id);
            return Ok();
        }
    }
}
