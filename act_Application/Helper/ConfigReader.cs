using Newtonsoft.Json.Linq;

namespace act_Application.Helper
{
    public class ConfigReader
    {
        public static string GetQuery(int opcion, string queryName)
        {
            string filePath = Path.Combine("Data", "Config", "config.json");
            string json = File.ReadAllText(filePath);
            JObject config = JObject.Parse(json);
            switch (opcion)
            {
                case 1:
                    //Queries para consulta de datos
                    return config["Queries"][queryName].ToString();
                case 2:
                    //Asistentes
                    return config["Assistant"][queryName].ToString();
                default:
                    Console.WriteLine("Opcion no reconocida.");
                    break;
            }
            return string.Empty;
        }
    }
}
