using System.ComponentModel.DataAnnotations.Schema;

namespace act_Application.Models.BD;
public partial class ActCuota
{
    public int Id { get; set; }
    public string IdCuot { get; set; }
    public int IdUser { get; set; }
    public int IdPrestamo { get; set; }
    public DateTime FechaGeneracion { get; set; }
    public DateTime FechaCuota { get; set; }
    public decimal Valor { get; set; }
    public string Estado { get; set; }
    public string FechaPago { get; set; }
    public string CBancoOrigen { get; set; }
    public string NBancoOrigen { get; set; }
    public string CBancoDestino { get; set; }
    public string NBancoDestino { get; set; }
    public string HistorialValores { get; set; }
    public string CapturaPantalla { get; set; }
    [NotMapped]
    public string NombreDueño { get; set; }
}
