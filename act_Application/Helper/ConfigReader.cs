using Newtonsoft.Json.Linq;

namespace act_Application.Helper
{
    public class ConfigReader
    {
        private readonly static string filePath = Path.Combine("Data", "json", "config.json");
        private readonly static string json = File.ReadAllText(filePath);
        public static string GetQuery(int Opcion, string Categoria, string queryName)
        {
            JObject config = JObject.Parse(json);
            switch (Opcion)
            {
                case 1:
                    switch (Categoria.ToUpper())
                    {
                        case "USER":
                            return config["Queries"]["DB_Queries"]["USER"][queryName].ToString();
                        case "ROLE":
                            return config["Queries"]["DB_Queries"]["ROLE"][queryName].ToString();
                        case "APOR":
                            return config["Queries"]["DB_Queries"]["APOR"][queryName].ToString();
                        case "MULT":
                            return config["Queries"]["DB_Queries"]["MULT"][queryName].ToString();
                        case "EVEN":
                            return config["Queries"]["DB_Queries"]["EVEN"][queryName].ToString();
                        case "PRES":
                            return config["Queries"]["DB_Queries"]["PRES"][queryName].ToString();
                        case "CUOT":
                            return config["Queries"]["DB_Queries"]["CUOT"][queryName].ToString();
                        case "DEST":
                            return config["Queries"]["DB_Queries"]["DEST"][queryName].ToString();
                        case "NOTI":
                            return config["Queries"]["DB_Queries"]["NOTI"][queryName].ToString();
                        default:
                            Console.WriteLine("Categoria no reconocida.");
                            break;
                    }
                    break;
                case 2:
                    return config["Queries"]["Assistant_Queries"][queryName].ToString();
                case 3:
                    return config["Queries"]["Automatic_Queries"][queryName].ToString();
                default:
                    Console.WriteLine("Opcion no reconocida.");
                    break;
            }
            return string.Empty;
        }
    }
}
