using ApiGymphony.Helpers;
using ApiGymphony.Models;
using ApiGymphony.Repositories;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetGymphonyAGM.Models;

namespace ApiGymphony.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private RepositoryGymphony repo;
        private HelperUsuarioToken helper;
        private BlobServiceClient blobServiceClient;

        public UsuariosController( RepositoryGymphony repo, HelperUsuarioToken helper, BlobServiceClient blobServiceClient )
        {
            this.repo = repo;
            this.helper = helper;
            this.blobServiceClient = blobServiceClient;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Usuario>> GetMiPerfil()
        {
            UsuarioTokenDTO usuarioLogueado = this.helper.GetUsuario();
            Usuario user = await this.repo.FindUsuarioAsync(usuarioLogueado.IdUsuario);

            if ( user != null && !string.IsNullOrEmpty(user.RutaFoto) )
            {
                string containerName = "usuariosgymphony";
                BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = containerClient.GetBlobClient(user.RutaFoto);

                if ( await blobClient.ExistsAsync() )
                {
                    BlobSasBuilder sasBuilder = new BlobSasBuilder()
                    {
                        BlobContainerName = containerName,
                        BlobName = user.RutaFoto,
                        Resource = "b",
                        ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                    };

                    sasBuilder.SetPermissions(BlobSasPermissions.Read);

                    Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                    user.RutaFoto = sasUri.ToString();
                }
            }

            return user;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> FindUsuario( int id )
        {
            return await this.repo.FindUsuarioAsync(id);
        }

        [Authorize(Roles = "Administrador, Entrenador")]
        [HttpGet("[action]/{idSesion}")]
        public async Task<ActionResult<List<Usuario>>> GetUsuariosPorSesion( int idSesion )
        {
            return await this.repo.GetUsuariosPorSesionAsync(idSesion);
        }

        [HttpGet("[action]/{rol}")]
        public async Task<ActionResult<List<VistaUsuario>>> GetUsuariosPorRol( string rol )
        {
            return await this.repo.GetUsuariosPorRolAsync(rol);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<VistaSocio>>> GetSociosConEstado()
        {
            return await this.repo.GetSociosConEstadoAsync();
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<Usuario>>> GetEntrenadores()
        {
            return await this.repo.GetTodosEntrenadoresAsync();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> RegistroSocio( [FromForm] SocioDTO nuevoSocio )
        {
            if ( nuevoSocio == null )
            {
                return BadRequest(new { status = "error", mensaje = "Los datos de registro son obligatorios." });
            }

            string nombreBlob = "default.jpg";

            if ( nuevoSocio.RutaFoto != null && nuevoSocio.RutaFoto.Length > 0 )
            {
                string extension = Path.GetExtension(nuevoSocio.RutaFoto.FileName);
                nombreBlob = Guid.NewGuid().ToString() + extension;

                string containerName = "usuariosgymphony";
                BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = containerClient.GetBlobClient(nombreBlob);

                using ( Stream stream = nuevoSocio.RutaFoto.OpenReadStream() )
                {
                    var blobOptions = new Azure.Storage.Blobs.Models.BlobUploadOptions
                    {
                        HttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
                        {
                            ContentType = nuevoSocio.RutaFoto.ContentType
                        }
                    };

                    await blobClient.UploadAsync(stream, blobOptions);
                }
            }

            await this.repo.RegistroSocioAsync(
                nuevoSocio.Email,
                nuevoSocio.Password,
                nuevoSocio.Nombre,
                nuevoSocio.Apellidos,
                nuevoSocio.Telefono,
                nuevoSocio.FechaNacimiento,
                nuevoSocio.Dni,
                nombreBlob
            );

            return Ok(new { status = "success", mensaje = "Socio registrado correctamente." });
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult> DeleteSocio( int id )
        {
            await this.repo.DeleteSocioAsync(id);
            return Ok();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("[action]/{idSocio}")]
        public async Task<ActionResult> DarDeBajaSocio( int idSocio )
        {
            try
            {
                await this.repo.DarDeBajaSocioAsync(idSocio);
                return Ok(new { status = "success", mensaje = "El socio ha sido dado de baja correctamente." });
            }
            catch ( Exception )
            {
                return BadRequest(new { status = "error", mensaje = "No se pudo procesar la baja. Verifica que el ID sea correcto." });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("[action]/{idSocio}")]
        public async Task<ActionResult> DarDeAltaSocio( int idSocio )
        {
            try
            {
                await this.repo.DarDeAltaSocioAsync(idSocio);
                return Ok(new { status = "success", mensaje = "El socio ha sido dado de alta exitosamente." });
            }
            catch ( Exception )
            {
                return BadRequest(new { status = "error", mensaje = "No se pudo procesar el alta. Verifica que el ID sea correcto." });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> RegistroEntrenador( [FromForm] EntrenadorDTO model )
        {
            if ( model == null || model.Usuario == null )
            {
                return BadRequest(new { status = "error", mensaje = "Datos insuficientes." });
            }

            try
            {
                string nombreBlob = "default.jpg";

                if ( model.Usuario.RutaFoto != null && model.Usuario.RutaFoto.Length > 0 )
                {
                    string extension = Path.GetExtension(model.Usuario.RutaFoto.FileName);
                    nombreBlob = Guid.NewGuid().ToString() + extension;

                    string containerName = "usuariosgymphony";
                    BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
                    BlobClient blobClient = containerClient.GetBlobClient(nombreBlob);

                    using ( Stream stream = model.Usuario.RutaFoto.OpenReadStream() )
                    {
                        var blobOptions = new Azure.Storage.Blobs.Models.BlobUploadOptions
                        {
                            HttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
                            {
                                ContentType = model.Usuario.RutaFoto.ContentType
                            }
                        };

                        await blobClient.UploadAsync(stream, blobOptions);
                    }
                }

                await this.repo.RegistroEntrenadorAsync(
                    model.Usuario.Email,
                    model.Usuario.Password,
                    model.Usuario.Nombre,
                    model.Usuario.Apellidos,
                    model.Usuario.Telefono,
                    model.Usuario.FechaNacimiento,
                    model.Usuario.Dni,
                    nombreBlob,
                    model.DiasSemana,
                    model.HorasInicio,
                    model.HorasFin
                );

                return Ok(new { status = "success", mensaje = "Entrenador y horario registrados correctamente." });
            }
            catch ( Exception ex )
            {
                return BadRequest(new { status = "error", mensaje = "Error en el registro: " + ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("[action]/{idEntrenador}")]
        public async Task<ActionResult> ValidarBorradoEntrenador( int idEntrenador )
        {
            try
            {
                bool tieneSesiones = await this.repo.EntrenadorTieneSesionesAsync(idEntrenador);
                var sustitutos = await this.repo.GetEntrenadoresSustitutosAsync(idEntrenador);

                var listaSustitutos = sustitutos.Select(s => new {
                    id = s.IdUsuario,
                    nombre = s.Nombre + " " + s.Apellidos
                }).ToList();

                return Ok(new
                {
                    success = true,
                    hasSessions = tieneSesiones,
                    sustitutos = listaSustitutos
                });
            }
            catch ( Exception ex )
            {
                return BadRequest(new { success = false, message = "Error al validar: " + ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("[action]/{idEntrenadorABorrar}")]
        public async Task<ActionResult> DeleteEntrenadorSustituyendo( int idEntrenadorABorrar, [FromQuery] int? idEntrenadorSustituto )
        {
            if ( idEntrenadorABorrar <= 0 )
            {
                return BadRequest(new { status = "error", mensaje = "ID de entrenador no válido." });
            }

            try
            {
                await this.repo.DeleteEntrenadorSustituyendoAsync(idEntrenadorABorrar, idEntrenadorSustituto);

                return Ok(new { status = "success", mensaje = "El entrenador ha sido eliminado correctamente de la plataforma." });
            }
            catch ( Exception ex )
            {
                return BadRequest(new { status = "error", mensaje = "Ocurrió un error al intentar borrar el entrenador: " + ex.Message });
            }
        }

        [HttpGet("[action]/{idUsuario}")]
        public async Task<ActionResult<VistaUsuario>> FindVistaUsuario( int idUsuario )
        {
            return await this.repo.FindVistaUsuarioAsync(idUsuario);
        }

        [Authorize(Roles = "Administrador, Entrenador")]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<DatosEvolucion>>> GetEvolucionSocios()
        {
            try
            {
                List<DatosEvolucion> evolucion = await this.repo.GetEvolucionSociosAsync();
                return Ok(evolucion);
            }
            catch ( Exception ex )
            {
                return BadRequest(new
                {
                    status = "error",
                    mensaje = "Error al generar los datos de evolución de altas y bajas: " + ex.Message
                });
            }
        }

        [Authorize(Roles = "Socio")]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<string>>> GetMisDiasAsistencia()
        {
            try
            {
                UsuarioTokenDTO usuarioLogueado = this.helper.GetUsuario();
                List<string> diasAsistidos = await this.repo.GetDiasAsistenciaSocioAsync(usuarioLogueado.IdUsuario);
                return Ok(diasAsistidos);
            }
            catch ( Exception ex )
            {
                return BadRequest(new { status = "error", mensaje = "Error al obtener el historial de asistencia: " + ex.Message });
            }
        }
    }
}
