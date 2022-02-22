using Newtonsoft.Json;
using System.IO;

namespace RGManager.Model
{
    public class GameRepository
    {
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                string value = string.Empty;
                using (var sr = new StreamReader(filePath))
                {
                    value = sr.ReadToEnd();
                }

                string contentsToWriteToFile;

                if (string.IsNullOrEmpty(value))
                    contentsToWriteToFile = $"[{JsonConvert.SerializeObject(objectToWrite)}]";
                else
                    contentsToWriteToFile = $"{value.Remove(value.Length - 1)},{JsonConvert.SerializeObject(objectToWrite)}]";

                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
