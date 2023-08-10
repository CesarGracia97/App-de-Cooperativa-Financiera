using Newtonsoft.Json;
using System.IO;

namespace act_Application.Helper
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