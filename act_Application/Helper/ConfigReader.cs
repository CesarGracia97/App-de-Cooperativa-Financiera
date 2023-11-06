using Newtonsoft.Json.Linq;

namespace act_Application.Helper
{
    public class ConfigReader
    {
        public static string GetQuery(string queryName)
        {
            string filePath = Path.Combine("Data", "Config", "config.json");
            string json = File.ReadAllText(filePath);
            JObject config = JObject.Parse(json);
            return config["Queries"][queryName].ToString();
        }
    }
}
