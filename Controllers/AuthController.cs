using ApiGymphony.Helpers;
using ApiGymphony.Models;
using ApiGymphony.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NugetGymphonyAGM.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiGymphony.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryGymphony repo;
        private HelperActionOAuthService helper;

        public AuthController( RepositoryGymphony repo, HelperActionOAuthService helper )
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login( LoginModelDTO model )
        {
            ValidacionUsuario user = await this.repo.LogInUserAsync(model.Email, model.Password);
            if ( user == null )
            {
                // Devolvemos 401 con un mensaje JSON para que el Front-end sepa qué decir
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }
            else
            {
                VistaUsuario usuario = await this.repo.FindVistaUsuarioAsync(user.Id);

                if ( usuario.NombreRol == "Socio" )
                {
                    bool isActive = await this.repo.IsSocioActivoAsync(usuario.IdUsuario);
                    if ( isActive == false )
                    {
                        // Devolvemos 403 Forbidden (Prohibido) porque sus credenciales son válidas, pero su cuenta está desactivada.
                        return StatusCode(403, new { mensaje = "Tu membresía está dada de baja. Por favor, contacta con administración." });
                    }
                }

                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);

                UsuarioTokenDTO usermodel = new UsuarioTokenDTO
                {
                    IdUsuario = usuario.IdUsuario,
                    NombreRol = usuario.NombreRol,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre,
                    Apellidos = usuario.Apellidos,
                    Telefono = usuario.Telefono,
                    FechaNacimiento = usuario.FechaNacimiento,
                    Dni = usuario.Dni,
                    RutaFoto = usuario.RutaFoto
                };

                string jsonUser = JsonConvert.SerializeObject(usermodel);
                string jsonCifrado = HelperCifrado.CifrarString(jsonUser);

                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonCifrado),
                    new Claim(ClaimTypes.Role, usuario.NombreRol),
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
                };

                JwtSecurityToken token = new JwtSecurityToken(
                    claims: informacion,
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(20),
                    notBefore: DateTime.UtcNow
                    );

                return Ok(new
                {
                    response = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
        }
    }
}
