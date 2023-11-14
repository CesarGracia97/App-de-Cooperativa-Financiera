using System.ComponentModel.DataAnnotations.Schema;
namespace act_Application.Models.BD;
public partial class ActAportacione
{
    public int Id { get; set; }
    public string IdApor { get; set; }
    public int IdUser { get; set; }
    public DateTime FechaAportacion { get; set; }
    public string Cuadrante { get; set; }
    public decimal Valor { get; set; }
    public string NBancoOrigen { get; set; }
    public string CBancoOrigen { get; set; }
    public string NBancoDestino { get; set; }
    public string CBancoDestino { get; set; }
    [Column(TypeName = "longblob")]
    public byte[] CapturaPantalla { get; set; }
    public string Estado { get; set; }
    [NotMapped]
    public string NombreUsuario { get; set; }
    [NotMapped]
    public int NumeroAportaciones { get; set; }
    [NotMapped]
    public List<DetalleAportacion> DetallesAportaciones { get; set; }
}
public class DetalleAportacion
{
    public decimal Valor { get; set; }
    public DateTime FechaAportacion { get; set; }
    public int Cuadrante { get; set; }
}
