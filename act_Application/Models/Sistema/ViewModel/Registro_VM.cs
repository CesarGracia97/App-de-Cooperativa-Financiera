using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;

namespace act_Application.Models.Sistema.ViewModel
{
    public class Registro_VM
    {
        public Registro_VM()
        {
            User = new ActUser();
        }
        public List<UserList> ListaDeUsuarios { get; set; }
        public ActUser User { get; set; }
    }
}
