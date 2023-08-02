namespace act_Application.Helper;
using Microsoft.Extensions.Configuration;

public static class AppSettingsHelper
{
    public static string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        return configuration.GetConnectionString("Cadena");
    }
}
