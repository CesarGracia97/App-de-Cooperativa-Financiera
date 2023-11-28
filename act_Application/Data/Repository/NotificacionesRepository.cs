using act_Application.Helper;

namespace act_Application.Data.Repository
{
    public class NotificacionesRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
    }
}
