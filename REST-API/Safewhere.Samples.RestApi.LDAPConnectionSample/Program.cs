using System;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.LDAPConnectionSample
{
    class Program
    {
        public const string PostLDAPAuthenticationConnectionSample = "SampleData/PostLDAPAuthenticationConnectionSample.json";
        public const string PutLDAPAuthenticationConnectionSample = "SampleData/PutLDAPAuthenticationConnectionSample.json";
        public const string PatchLDAPAuthenticationConnectionSample = "SampleData/PatchLDAPAuthenticationConnectionSample.json";

        public const string LDAPAuthenticationConnectionName = "LDAP authentication connection";

        public static void Main()
        {
            Console.WriteLine("Begin POST {0}", LDAPAuthenticationConnectionName);
            PostConnection(LDAPAuthenticationConnectionName, PostLDAPAuthenticationConnectionSample);
            Console.WriteLine("End POST {0}\n", LDAPAuthenticationConnectionName);

            Console.WriteLine("Begin PUT {0}", LDAPAuthenticationConnectionName);
            PutConnection(LDAPAuthenticationConnectionName, PostLDAPAuthenticationConnectionSample, PutLDAPAuthenticationConnectionSample);
            Console.WriteLine("End PUT {0}\n", LDAPAuthenticationConnectionName);

            Console.WriteLine("Begin GET {0}", LDAPAuthenticationConnectionName);
            GetConnection(LDAPAuthenticationConnectionName, PostLDAPAuthenticationConnectionSample);
            Console.WriteLine("End GET {0}\n", LDAPAuthenticationConnectionName);

            Console.WriteLine("Begin DELETE {0}", LDAPAuthenticationConnectionName);
            DeleteConnection(LDAPAuthenticationConnectionName, PostLDAPAuthenticationConnectionSample);
            Console.WriteLine("End DELETE {0}\n", LDAPAuthenticationConnectionName);

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
                        var response = request.Get(string.Format(CultureInfo.InstalledUICulture, "{0}/{1}", RequestObject.Connections, postData.Name));
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
                           var response = request.Delete(string.Format(CultureInfo.InstalledUICulture, "{0}/{1}", RequestObject.Connections, postData.Name));
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.InstalledUICulture, "{0}/{1}", RequestObject.Connections, postData.Name));
                       }
                   );
            }
        }
    }
}
