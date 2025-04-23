using System;
using System.Collections.Generic;
using System.Linq;
using Examen3.Models;

namespace Examen3.Clases
{
    public class clsLogin
    {
        public clsLogin()
        {
            loginRespuesta = new LoginRespuesta();
        }

        public DBExamen3Entities db = new DBExamen3Entities();
        public Login login { get; set; }
        public LoginRespuesta loginRespuesta { get; set; }

        public bool ValidarUsuario()
        {
            try
            {
                var admin = db.AdministradorITMs.FirstOrDefault(a => a.Usuario == login.Usuario);
                if (admin == null)
                {
                    loginRespuesta.Autenticado = false;
                    loginRespuesta.Mensaje = "Usuario no existe";
                    return false;
                }

                if (admin.Clave != login.Clave)
                {
                    loginRespuesta.Autenticado = false;
                    loginRespuesta.Mensaje = "Clave incorrecta";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                loginRespuesta.Autenticado = false;
                loginRespuesta.Mensaje = ex.Message;
                return false;
            }
        }

        public IQueryable<LoginRespuesta> Ingresar()
        {
            if (ValidarUsuario())
            {
                string token = TokenGenerator.GenerateTokenJwt(login.Usuario);
                return (from a in db.AdministradorITMs
                        where a.Usuario == login.Usuario && a.Clave == login.Clave
                        select new LoginRespuesta
                        {
                            Usuario = a.Usuario,
                            Autenticado = true,
                            Token = token,
                            Mensaje = "Inicio de sesión exitoso"
                        }).AsQueryable();
            }
            else
            {
                List<LoginRespuesta> list = new List<LoginRespuesta>();
                list.Add(loginRespuesta);
                return list.AsQueryable();
            }
        }
    }
}