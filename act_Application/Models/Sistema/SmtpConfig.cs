using Newtonsoft.Json;

namespace act_Application.Models.Sistema
{
    public class SmtpConfig
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public static SmtpConfig LoadConfig(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SmtpConfig>(json);
        }
    }
}