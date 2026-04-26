using ApiGymphony.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiGymphony.Helpers
{
    public class HelperUsuarioToken
    {
        private IHttpContextAccessor contextAccessor;

        public HelperUsuarioToken( IHttpContextAccessor contextAccessor )
        {
            this.contextAccessor = contextAccessor;
        }

        public UsuarioTokenDTO GetUsuario()
        {
            Claim claim = this.contextAccessor.HttpContext.User.FindFirst(z => z.Type == "UserData");
            string jsonCifrado = claim.Value;
            string jsonUsuario = HelperCifrado.DescifrarString(jsonCifrado);
            UsuarioTokenDTO user = JsonConvert.DeserializeObject<UsuarioTokenDTO>(jsonUsuario);
            return user;
        }
    }
}
