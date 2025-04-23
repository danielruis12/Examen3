//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Examen3.Models;
//using System.Web;

//namespace Examen3.Clases
//{
//    public class clsLogin
//    {
//        public clsLogin()
//        {
//            loginRespuesta = new LoginRespuesta();
//        }
//        public DBExamen3Entities dbExamen3 = new DBExamen3Entities();
//        public Login login { get; set; }
//        public LoginRespuesta loginRespuesta { get; set; }
//        public bool ValidarUsuario()
//        {
//            try
//            {
//                clsCypher cifrar = new clsCypher();
//                Usuario usuario = dbExamen3.AdministradorITMs.FirstOrDefault(a => a.Usuario == login.Usuario);
//                if (usuario == null)
//                {
//                    loginRespuesta.Autenticado = false;
//                    loginRespuesta.Mensaje = "Usuario no existe";
//                    return false;
//                }
//                byte[] arrBytesSalt = Convert.FromBase64String(usuario.Salt);
//                string ClaveCifrada = cifrar.HashPassword(login.Clave, arrBytesSalt);
//                login.Clave = ClaveCifrada;
//                return true;
//            }
//            catch (Exception ex)
//            {
//                loginRespuesta.Autenticado = false;
//                loginRespuesta.Mensaje = ex.Message;
//                return false;
//            }
//        }
//        private bool ValidarClave()
//        {
//            try
//            {
//                Usuario usuario = dbExamen3.AdministradorITMs.FirstOrDefault(a => a.Usuario == login.Usuario && a.Clave == login.Clave);
//                if (usuario == null)
//                {
//                    loginRespuesta.Autenticado = false;
//                    loginRespuesta.Mensaje = "La clave no coincide";
//                    return false;
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                loginRespuesta.Autenticado = false;
//                loginRespuesta.Mensaje = ex.Message;
//                return false;
//            }
//        }
//        public IQueryable<LoginRespuesta> Ingresar()
//        {
//            if (ValidarUsuario() && ValidarClave())
//            {
//                string token = TokenGenerator.GenerateTokenJwt(login.Usuario);
//                return from U in dbExamen3.Set<AdministradorITM>()
//                       join UP in dbSuper.Set<Usuario_Perfil>()
//                       on U.id equals UP.idUsuario
//                       join P in dbSuper.Set<Perfil>()
//                       on UP.idPerfil equals P.id
//                       where U.userName == login.Usuario &&
//                               U.Clave == login.Clave
//                       select new LoginRespuesta
//                       {
//                           Usuario = U.userName,
//                           Autenticado = true,
//                           Perfil = P.Nombre,
//                           PaginaInicio = P.PaginaNavegar,
//                           Token = token,
//                           Mensaje = ""
//                       };
//            }
//            else
//            {
//                List<LoginRespuesta> List = new List<LoginRespuesta>();
//                List.Add(loginRespuesta);
//                return List.AsQueryable();
//            }
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using Examen3.Models;
using System.Web;

namespace Examen3.Clases
{
    public class clsLogin
    {
        public clsLogin()
        {
            loginRespuesta = new LoginRespuesta();
        }

        public DBExamen3Entities dbExamen3 = new DBExamen3Entities();
        public Login login { get; set; }
        public LoginRespuesta loginRespuesta { get; set; }

        public bool ValidarUsuario()
        {
            try
            {
                var administrador = dbExamen3.AdministradorITMs
                    .FirstOrDefault(a => a.Usuario == login.Usuario);

                if (administrador == null)
                {
                    loginRespuesta.Autenticado = false;
                    loginRespuesta.Mensaje = "Usuario no existe";
                    return false;
                }

                if (administrador.Clave != login.Clave)
                {
                    loginRespuesta.Autenticado = false;
                    loginRespuesta.Mensaje = "Clave incorrecta";
                    return false;
                }

                loginRespuesta.Autenticado = true;
                loginRespuesta.Usuario = administrador.Usuario;
                loginRespuesta.Nombre = administrador.NombreCompleto;
                loginRespuesta.Token = TokenGenerator.GenerateTokenJwt(administrador.Usuario);
                loginRespuesta.Mensaje = "Autenticado correctamente";
                return true;
            }
            catch (Exception ex)
            {
                loginRespuesta.Autenticado = false;
                loginRespuesta.Mensaje = ex.Message;
                return false;
            }
        }

        public LoginRespuesta Ingresar()
        {
            ValidarUsuario();
            return loginRespuesta;
        }
    }
}