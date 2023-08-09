using Newtonsoft.Json;

namespace act_Application.Helper
{
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public static SmtpSettings Load()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Config", "smtpconfig.json");
            string jsonConfig = File.ReadAllText(configPath);
            return JsonConvert.DeserializeObject<SmtpSettings>(jsonConfig);
        }
    }
}