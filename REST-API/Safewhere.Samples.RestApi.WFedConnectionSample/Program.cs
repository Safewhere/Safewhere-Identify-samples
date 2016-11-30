using System;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.WSFedConnectionSample
{
    class Program
    {
        public const string PostWSFedProtocolConnectionSample = "SampleData/PostWSFedProtocolConnectionSample.json";
        public const string PutWSFedProtocolConnectionSample = "SampleData/PutWSFedProtocolConnectionSample.json";
        public const string PatchWSFedProtocolConnectionSample = "SampleData/PatchWSFedProtocolConnectionSample.json";

        public const string PostWSFedAuthenticationConnectionSample = "SampleData/PostWSFedAuthenticationConnectionSample.json";
        public const string PutWSFedAuthenticationConnectionSample = "SampleData/PutWSFedAuthenticationConnectionSample.json";
        public const string PatchWSFedAuthenticationConnectionSample = "SampleData/PatchWSFedAuthenticationConnectionSample.json";

        public const string WSFedProtocolConnectionName = "WSFederation protocol connection";
        public const string WSFedAuthenticationConnectionName = "WSFederation authentication connection";

        public static void Main()
        {
            Console.WriteLine("Begin POST {0}", WSFedProtocolConnectionName);
            PostConnection(WSFedProtocolConnectionName, PostWSFedProtocolConnectionSample);
            Console.WriteLine("End POST {0}\n", WSFedProtocolConnectionName);

            Console.WriteLine("Begin PUT {0}", WSFedProtocolConnectionName);
            PutConnection(WSFedProtocolConnectionName, PostWSFedProtocolConnectionSample, PutWSFedProtocolConnectionSample);
            Console.WriteLine("End PUT {0}\n", WSFedProtocolConnectionName);

            Console.WriteLine("Begin PUT {0}", WSFedProtocolConnectionName);
            PatchConnection(WSFedProtocolConnectionName, PostWSFedProtocolConnectionSample, PatchWSFedProtocolConnectionSample);
            Console.WriteLine("End PUT {0}\n", WSFedProtocolConnectionName);

            Console.WriteLine("Begin GET {0}", WSFedProtocolConnectionName);
            GetConnection(WSFedProtocolConnectionName, PostWSFedProtocolConnectionSample);
            Console.WriteLine("End GET {0}\n", WSFedProtocolConnectionName);

            Console.WriteLine("Begin DELETE {0}", WSFedProtocolConnectionName);
            DeleteConnection(WSFedProtocolConnectionName, PostWSFedProtocolConnectionSample);
            Console.WriteLine("End DELETE {0}\n", WSFedProtocolConnectionName);

            Console.WriteLine("Begin POST {0}", WSFedAuthenticationConnectionName);
            PostConnection(WSFedAuthenticationConnectionName, PostWSFedAuthenticationConnectionSample);
            Console.WriteLine("End POST {0}\n", WSFedAuthenticationConnectionName);

            Console.WriteLine("Begin PUT {0}", WSFedAuthenticationConnectionName);
            PutConnection(WSFedAuthenticationConnectionName, PostWSFedAuthenticationConnectionSample, PutWSFedAuthenticationConnectionSample);
            Console.WriteLine("End PUT {0}\n", WSFedAuthenticationConnectionName);

            Console.WriteLine("Begin PUT {0}", WSFedAuthenticationConnectionName);
            PatchConnection(WSFedAuthenticationConnectionName, PostWSFedAuthenticationConnectionSample, PatchWSFedAuthenticationConnectionSample);
            Console.WriteLine("End PUT {0}\n", WSFedAuthenticationConnectionName);

            Console.WriteLine("Begin GET {0}", WSFedAuthenticationConnectionName);
            GetConnection(WSFedAuthenticationConnectionName, PostWSFedAuthenticationConnectionSample);
            Console.WriteLine("End GET {0}\n", WSFedAuthenticationConnectionName);

            Console.WriteLine("Begin DELETE {0}", WSFedAuthenticationConnectionName);
            DeleteConnection(WSFedAuthenticationConnectionName, PostWSFedAuthenticationConnectionSample);
            Console.WriteLine("End DELETE {0}\n", WSFedAuthenticationConnectionName);

            Console.WriteLine("All done!");
        }


        private static void PostConnection(string connectionName, string postDataFilePath)
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("-> Prepare data");
                var postData = Helper.GetJsonObjectFromFile<Connection>(postDataFilePath);

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Post {0}", connectionName);
                           var response = request.Post(RequestObject.Connections, postData);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.InstalledUICulture, "{0}/{1}", RequestObject.Connections, postData.Name));
                       }
                   );
            }
        }

        private static void PutConnection(string connectionName, string postDataFilePath, string putDataFilePath)
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("-> Prepare data");
                var postData = Helper.GetJsonObjectFromFile<Connection>(postDataFilePath);
                var putData = Helper.GetJsonObjectFromFile<JObject>(putDataFilePath);

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create new {0}", connectionName);
                           var response = request.Post(RequestObject.Connections, postData);

                           Console.WriteLine("-> Put to update {0}", connectionName);
                           response = request.Put(RequestObject.Connections, putData);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.InstalledUICulture, "{0}/{1}", RequestObject.Connections, postData.Name));
                       }
                   );
            }
        }

        private static void GetConnection(string connectionName, string postDataFilePath)
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("-> Prepare data");
                var postData = Helper.GetJsonObjectFromFile<Connection>(postDataFilePath);

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create new {0}", connectionName);
                           request.Post(RequestObject.Connections, postData);

                           Console.WriteLine("-> Get {0}", connectionName);
                           return request.Get(string.Format(CultureInfo.InstalledUICulture, "{0}/{1}", RequestObject.Connections, postData.Name));
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.InstalledUICulture, "{0}/{1}", RequestObject.Connections, postData.Name));
                       }
                   );
            }
        }

        private static void PatchConnection(string connectionName, string postDataFilePath, string patchDataFilePath)
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("-> Prepare data");
                var postData = Helper.GetJsonObjectFromFile<Connection>(postDataFilePath);
                var patchData = Helper.GetJsonObjectFromFile<JArray>(patchDataFilePath);

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create new {0}", connectionName);
                           var response = request.Post(RequestObject.Connections, postData);

                           Console.WriteLine("-> Put to update {0}", connectionName);
                           var path = string.Join("/", RequestObject.Connections, postData.Name);
                           response = request.Patch(path, patchData);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.InstalledUICulture, "{0}/{1}", RequestObject.Connections, postData.Name));
                       }
                   );
            }
        }

        private static void DeleteConnection(string connectionName, string postDataFilePath)
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("-> Prepare data");
                var postData = Helper.GetJsonObjectFromFile<Connection>(postDataFilePath);

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create new {0}", connectionName);
                           request.Post(RequestObject.Connections, postData);

                           Console.WriteLine("-> Delete {0}", connectionName);
                           return request.Delete(string.Format(CultureInfo.InstalledUICulture, "{0}/{1}", RequestObject.Connections, postData.Name));
                       },
                       () =>
                       {
                       }
                   );
            }
        }
    }
}
