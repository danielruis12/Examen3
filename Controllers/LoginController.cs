using Examen3.Clases;
using Examen3.Models;
using System.Linq;
using System.Web.Http;

namespace Examen3.Controllers
{
    [RoutePrefix("api/login")]
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("Ingresar")]
        public IQueryable<LoginRespuesta> Ingresar([FromBody] Login login)
        {
            clsLogin _login = new clsLogin();
            _login.login = login;
            return _login.Ingresar();
        }
    }
}