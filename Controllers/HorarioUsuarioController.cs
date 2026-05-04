using ApiGymphony.Helpers;
using ApiGymphony.Models;
using ApiGymphony.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetGymphonyAGM.Models;

namespace ApiGymphony.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioUsuarioController : ControllerBase
    {
        private RepositoryGymphony repo;
        private HelperUsuarioToken helper;

        public HorarioUsuarioController( RepositoryGymphony repo, HelperUsuarioToken helper )
        {
            this.repo = repo;
            this.helper = helper;
        }

        [Authorize(Roles = "Entrenador")]
        [HttpGet]
        [Route("[action]")] 
        public async Task<ActionResult<List<HorarioEmpleados>>> GetHorariosDeEntrenadorPerfil()
        {
            UsuarioTokenDTO usuarioLogueado = this.helper.GetUsuario();
            return await this.repo.GetHorarioUsuarioPorIdAsync(usuarioLogueado.IdUsuario);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<List<HorarioEmpleados>>> GetHorarioEntrenadorOrdenadoAdmin( int id )
        {
            return await this.repo.GetHorariosEntrenadorAsync(id);
        }
    }
}
