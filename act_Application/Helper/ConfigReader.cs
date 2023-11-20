using Newtonsoft.Json.Linq;

namespace act_Application.Helper
{
    public class ConfigReader
    {
        public static string GetQuery(int opcion, string queryName)
        {
            string filePath = Path.Combine("Data", "json", "config.json");
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
                case 3:
                    return config["Automatic"][queryName].ToString();
                default:
                    Console.WriteLine("Opcion no reconocida.");
                    break;
            }
            return string.Empty;
        }
    }
}
