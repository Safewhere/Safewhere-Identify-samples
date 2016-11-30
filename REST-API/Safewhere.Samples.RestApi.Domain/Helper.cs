using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace Safewhere.Samples.RestApi.Domain
{
    public static class Helper
    {
        public static string GetJsonFromFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            string fileContent;

            var assemblyPath = Directory.GetCurrentDirectory();
            var testDataFile = new FileInfo(Path.Combine(assemblyPath, path));

            using (var reader = new StreamReader(testDataFile.FullName))
            {
                fileContent = reader.ReadToEnd();
            }

            return fileContent;
        }

        public static T GetJsonObjectFromFile<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
                return default(T);

            string fileContent;

            var assemblyPath = Directory.GetCurrentDirectory();
            var testDataFile = new FileInfo(Path.Combine(assemblyPath, path));

            using (var reader = new StreamReader(testDataFile.FullName))
            {
                fileContent = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(fileContent);
        }

        public static string ReadResponseContentAsString(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException("response");
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
