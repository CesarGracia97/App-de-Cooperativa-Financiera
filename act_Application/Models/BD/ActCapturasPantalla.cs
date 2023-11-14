using System.ComponentModel.DataAnnotations.Schema;

namespace act_Application.Models.BD;

public partial class ActCapturasPantalla
{
    public int Id { get; set; }
    public int IdUser {  get; set; }
    public string Origen {  get; set; }
    public int IdOrigenCaptura { get; set; }
    [Column(TypeName = "longblob")]
    public byte[] CapturaPantalla { get; set; }
}
