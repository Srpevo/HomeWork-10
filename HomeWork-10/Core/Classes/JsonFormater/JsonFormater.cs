using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeWork_10.Core.Classes.JsonFormater
{
    internal static class JsonTool
    {
        public static void ToJson(object obj, string filePath)
        {
           
            string Json = JsonSerializer.Serialize(obj);


            using (StreamWriter wr  = new StreamWriter(filePath))
            {
                wr.WriteLine(Json);
            }
        }
        public static IEnumerable<T> ToList<T>(string filePath)
        {
            string jsonStr = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<IEnumerable<T>>(jsonStr)!;
        } 
    }
}
