using System.ComponentModel.DataAnnotations.Schema;

namespace act_Application.Models.BD;

/// <summary>
/// Aportaciones ec
/// </summary>
public partial class ActAportacione
{
    public int Id { get; set; } //Oculto en la vista

    public int IdUser { get; set; }//Oculto en la vista

    public DateTime FechaAportacion { get; set; }

    public string Nbanco { get; set; }

    public string Cbancaria { get; set; }

    public string Razon { get; set; }

    public decimal Valor { get; set; }

    public byte[] CapturaPantalla { get; set; }

    public string Aprobacion { get; set; }//Oculto en la vista (Inyeccion dentro del sistema)


    public int Cuadrante1 { get; set; }//Oculto en la vista (Inyeccion dentro del sistema)

    public int Cuadrante2 { get; set; }

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