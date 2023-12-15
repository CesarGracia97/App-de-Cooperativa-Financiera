using System.ComponentModel.DataAnnotations.Schema;

namespace act_Application.Models.BD;
public partial class ActPrestamo
{
    public int Id { get; set; }
    public string IdPres { get; set; }
    public int IdUser { get; set; }
    public decimal Valor { get; set; }
    public DateTime FechaGeneracion { get; set; }
    public DateTime FechaEntregaDinero { get; set; }
    public DateTime FechaInicioPagoCuotas { get; set; }
    public DateTime FechaPagoTotalPrestamo { get; set; }
    public string TipoCuota { get; set; }
    public string Estado { get; set; }
    [NotMapped]
    public string DuenoPrestamo { get; set; }
}