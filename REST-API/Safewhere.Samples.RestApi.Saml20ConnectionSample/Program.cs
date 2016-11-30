using System;
using System.Globalization;
using Safewhere.SCIMModel.Connections;
using Newtonsoft.Json.Linq;
using Safewhere.Samples.RestApi.Domain;

namespace Safewhere.Samples.RestApi.Saml20ConnectionSample
{
    class Program
    {
        public const string PostSaml20ProtocolConnectionSample = "SampleData/PostSaml20ProtocolConnectionSample.json";
        public const string PutSaml20ProtocolConnectionSample = "SampleData/PutSaml20ProtocolConnectionSample.json";
        public const string PatchSaml20ProtocolConnectionSample = "SampleData/PatchSaml20ProtocolConnectionSample.json";

        public const string PostSaml20AuthenticationConnectionSample = "SampleData/PostSaml20AuthenticationConnectionSample.json";
        public const string PutSaml20AuthenticationConnectionSample = "SampleData/PutSaml20AuthenticationConnectionSample.json";
        public const string PatchSaml20AuthenticationConnectionSample = "SampleData/PatchSaml20AuthenticationConnectionSample.json";

        public const string SamlProtocolConnectionName = "Saml20 protocol connection";
        public const string SamlAuthenticationConnectionName = "Saml20 authentication connection";

        private static void Main()
        {
            Console.WriteLine( "Begin POST {0}", SamlProtocolConnectionName);
            PostConnection(SamlProtocolConnectionName, PostSaml20ProtocolConnectionSample);
            Console.WriteLine( "End POST {0}\n", SamlProtocolConnectionName);

            Console.WriteLine( "Begin PUT {0}", SamlProtocolConnectionName);
            PutConnection(SamlProtocolConnectionName, PostSaml20ProtocolConnectionSample, PutSaml20ProtocolConnectionSample);
            Console.WriteLine( "End PUT {0}\n", SamlProtocolConnectionName);

            Console.WriteLine( "Begin PUT {0}", SamlProtocolConnectionName);
            PatchConnection(SamlProtocolConnectionName, PostSaml20ProtocolConnectionSample, PatchSaml20ProtocolConnectionSample);
            Console.WriteLine( "End PUT {0}\n", SamlProtocolConnectionName);

            Console.WriteLine( "Begin GET {0}", SamlProtocolConnectionName);
            GetConnection(SamlProtocolConnectionName, PostSaml20ProtocolConnectionSample);
            Console.WriteLine( "End GET {0}\n", SamlProtocolConnectionName);

            Console.WriteLine( "Begin DELETE {0}", SamlProtocolConnectionName);
            DeleteConnection(SamlProtocolConnectionName, PostSaml20ProtocolConnectionSample);
            Console.WriteLine( "End DELETE {0}\n", SamlProtocolConnectionName);

            Console.WriteLine( "Begin POST {0}", SamlAuthenticationConnectionName);
            PostConnection(SamlAuthenticationConnectionName, PostSaml20AuthenticationConnectionSample);
            Console.WriteLine( "End POST {0}\n", SamlAuthenticationConnectionName);

            Console.WriteLine( "Begin PUT {0}", SamlAuthenticationConnectionName);
            PutConnection(SamlAuthenticationConnectionName, PostSaml20AuthenticationConnectionSample, PutSaml20AuthenticationConnectionSample);
            Console.WriteLine( "End PUT {0}\n", SamlAuthenticationConnectionName);

            Console.WriteLine( "Begin PUT {0}", SamlAuthenticationConnectionName);
            PatchConnection(SamlAuthenticationConnectionName, PostSaml20AuthenticationConnectionSample, PatchSaml20AuthenticationConnectionSample);
            Console.WriteLine( "End PUT {0}\n", SamlAuthenticationConnectionName);

            Console.WriteLine( "Begin GET {0}", SamlAuthenticationConnectionName);
            GetConnection(SamlAuthenticationConnectionName, PostSaml20AuthenticationConnectionSample);
            Console.WriteLine( "End GET {0}\n", SamlAuthenticationConnectionName);

            Console.WriteLine( "Begin DELETE {0}", SamlAuthenticationConnectionName);
            DeleteConnection(SamlAuthenticationConnectionName, PostSaml20AuthenticationConnectionSample);
            Console.WriteLine( "End DELETE {0}\n", SamlAuthenticationConnectionName);

            //Console.WriteLine("All done!");
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
                            Console.WriteLine( "-> Post {0}", connectionName);
                            var response = request.Post(RequestObject.Connections, postData);
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections, postData.Name));
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
                        Console.WriteLine( "-> Post {0}", connectionName);
                        var response = request.Post(RequestObject.Connections, postData);

                        Console.WriteLine( "-> Put to update {0}", connectionName);
                        response = request.Put(RequestObject.Connections, putData);
                        return response;
                    },
                    () =>
                    {
                        request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections, postData.Name));
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
                        Console.WriteLine( "-> Create new {0}", connectionName);
                        request.Post(RequestObject.Connections, postData);

                        Console.WriteLine( "-> Get {0}", connectionName);
                        var response = request.Get(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections, postData.Name));
                        return response;
                    },
                    () =>
                    {
                        request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections, postData.Name));
                    }
                );
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")] 
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
                        Console.WriteLine( "-> Create new {0}", connectionName);
                        var response = request.Post(RequestObject.Connections, postData);

                        Console.WriteLine( "-> Put to update {0}", connectionName);
                        var path = string.Join("/", RequestObject.Connections, postData.Name);
                        response = request.Patch(path, patchData);

                        return response;
                    },
                    () =>
                    {
                        request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections, postData.Name));
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
                        Console.WriteLine( "-> Create new {0}", connectionName);
                        request.Post(RequestObject.Connections, postData);

                        Console.WriteLine( "-> Delete {0}", connectionName);
                        var response =
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections,
                                postData.Name));
                        return response;
                    },
                    () =>
                    {
                        request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections,
                                postData.Name));
                    }
                );

            }
        }
    }
}
