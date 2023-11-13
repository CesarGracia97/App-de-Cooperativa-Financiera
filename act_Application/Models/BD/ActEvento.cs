using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace act_Application.Models.BD
{
    public partial class ActEvento
    {
        public int Id { get; set; }
        public string IdEven { get; set; }
        public int IdPrestamo { get; set; }
        public int IdUser { get; set; }
        public string ParticipantesId {  get; set; } 
        public string NombresPId { get; set; }
        public DateTime FechaGeneracion {  get; set; }
        public  DateTime FechaInicio {  get; set; }
        public DateTime FechaFinalizacion {  get; set; }
        public string Estado {  get; set; }
        [NotMapped]
        public string NombreUsuario { get; set; }
    }
}
