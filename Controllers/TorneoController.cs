using System;
using System.Linq;
using System.Web.Http;
using Examen3.Models;

namespace Examen3.Controllers
{
    [Authorize]
    [RoutePrefix("api/Torneos")]
    public class TorneoController : ApiController
    {
        private DBExamen3Entities dbExamen3 = new DBExamen3Entities();

        [HttpGet]
        [Route("Consultar")]
        public IHttpActionResult GetTorneos(string tipo = null, string nombre = null, string fecha = null)
        {
            var torneos = dbExamen3.Torneos.AsQueryable();

            if (!string.IsNullOrEmpty(tipo))
                torneos = torneos.Where(t => t.TipoTorneo.Contains(tipo));
            if (!string.IsNullOrEmpty(nombre))
                torneos = torneos.Where(t => t.NombreTorneo.Contains(nombre));
            if (!string.IsNullOrEmpty(fecha) && DateTime.TryParse(fecha, out var fechaParsed))
                torneos = torneos.Where(t => t.FechaTorneo == fechaParsed);

            return Ok(torneos.ToList());
        }

        [HttpPost]
        [Route("Registrar")]
        public IHttpActionResult PostTorneo([FromBody] Torneo torneo)
        {
            dbExamen3.Torneos.Add(torneo);
            dbExamen3.SaveChanges();
            return Ok("Torneo registrado correctamente");
        }

        [HttpPut]
        [Route("Actualizar")]
        public IHttpActionResult PutTorneo([FromBody] Torneo torneo)
        {
            var actual = dbExamen3.Torneos.Find(torneo.idTorneos);
            if (actual == null) return NotFound();

            actual.TipoTorneo = torneo.TipoTorneo;
            actual.NombreTorneo = torneo.NombreTorneo;
            actual.NombreEquipo = torneo.NombreEquipo;
            actual.ValorInscripcion = torneo.ValorInscripcion;
            actual.FechaTorneo = torneo.FechaTorneo;
            actual.Integrantes = torneo.Integrantes;

            dbExamen3.SaveChanges();
            return Ok("Torneo actualizado correctamente");
        }

        [HttpDelete]
        [Route("Eliminar")]
        public IHttpActionResult DeleteTorneo(int id)
        {
            var torneo = dbExamen3.Torneos.Find(id);
            if (torneo == null) return NotFound();
            dbExamen3.Torneos.Remove(torneo);
            dbExamen3.SaveChanges();
            return Ok("Torneo eliminado correctamente");
        }
    }
}