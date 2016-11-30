using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using Safewhere.Samples.RestApi.Domain;

namespace Safewhere.Samples.RestApi.CertificateSample
{
    class Program
    {
        public const string PostCertificateSample = "SampleData/PostCertificateSample.json";
        public const string PostManyCertificateSample = "SampleData/PostManyCertificateSample.json";

        public const string ResourceName = "Certificate";

        private static void Main()
        {
            Console.WriteLine("Begin POST {0}", ResourceName);
            PostCertificate(ResourceName, PostCertificateSample);
            Console.WriteLine("End POST {0}\n", ResourceName);

            Console.WriteLine("Begin POST many {0}", ResourceName);
            PostManyCertificate(ResourceName, PostManyCertificateSample);
            Console.WriteLine("End POST {0}\n", ResourceName);

            Console.WriteLine("Begin GET {0}", ResourceName);
            GetCertificate(ResourceName, PostCertificateSample);
            Console.WriteLine("End GET {0}\n", ResourceName);

            Console.WriteLine("Begin DELETE {0}", ResourceName);
            DeleteCertificate(ResourceName, PostCertificateSample);
            Console.WriteLine("End DELETE {0}\n", ResourceName);

            Console.WriteLine("Begin DELETE {0}", ResourceName);
            DeleteManyCertificate(ResourceName, PostManyCertificateSample);
            Console.WriteLine("End DELETE {0}\n", ResourceName);

            Console.WriteLine("All done!");
        }

        private static void PostCertificate(string resourceName, string postDataFilePath)
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("-> Prepare data");
                var postData = Helper.GetJsonObjectFromFile<JObject>(postDataFilePath);
                var rawData = (string)postData["RawData"];

                var cert = new X509Certificate2(Convert.FromBase64String(rawData));
                RestApiCaller.CallAndHandleError
                (
                    () =>
                    {
                        Console.WriteLine("-> Post {0}", resourceName);
                        var response = request.Post(RequestObject.Certificate, postData);
                        return response;
                    },
                    () => request.Delete(string.Join("/", RequestObject.Certificate, string.Format(CultureInfo.InvariantCulture, "{0}?forceDeleteInUse=true", cert.Thumbprint)))
                );
            }
        }

        private static void PostManyCertificate(string resourceName, string postDataFilePath)
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("-> Prepare data");
                var thumprints = new List<string>();
                var postData = Helper.GetJsonObjectFromFile<JArray>(postDataFilePath);

                foreach (var data in postData)
                {
                    var rawData = (string)data["RawData"];
                    var cert = new X509Certificate2(Convert.FromBase64String(rawData));
                    thumprints.Add(cert.Thumbprint);
                }

                RestApiCaller.CallAndHandleError
                (
                    () =>
                    {
                        Console.WriteLine("-> Post {0}", resourceName);
                        var response = request.Post(string.Join("/", RequestObject.Certificate, "many"), postData);
                        return response;
                    },
                    () =>
                        request.Delete(string.Join("/", RequestObject.Certificate, "many?forceDeleteInUse=true"),
                            thumprints)
                );
            }
        }

        private static void GetCertificate(string resourceName, string postDataFilePath)
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("-> Prepare data");
                var postData = Helper.GetJsonObjectFromFile<JObject>(postDataFilePath);
                var rawData = (string)postData["RawData"];

                var cert = new X509Certificate2(Convert.FromBase64String(rawData));
                RestApiCaller.CallAndHandleError
                (
                    () =>
                    {
                        Console.WriteLine("-> Create new {0}", resourceName);
                        var response = request.Post(RequestObject.Certificate, postData);

                        Console.WriteLine("-> Get {0}", resourceName);
                        response = request.Get(string.Join("/", RequestObject.Certificate, cert.Thumbprint));
                        return response;
                    },
                    () =>
                        request.Delete(string.Join("/", RequestObject.Certificate,
                            string.Format(CultureInfo.InvariantCulture, "{0}?forceDeleteInUse=true", cert.Thumbprint)))
                );
            }
        }

        private static void DeleteCertificate(string resourceName, string postDataFilePath)
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("-> Prepare data");
                var postData = Helper.GetJsonObjectFromFile<JObject>(postDataFilePath);
                var rawData = (string)postData["RawData"];

                var cert = new X509Certificate2(Convert.FromBase64String(rawData));
                RestApiCaller.CallAndHandleError
                (
                    () =>
                    {
                        Console.WriteLine("-> Create new {0}", resourceName);
                        var response = request.Post(RequestObject.Certificate, postData);

                        Console.WriteLine("-> Delete {0}", resourceName);
                        response =
                            request.Delete(string.Join("/", RequestObject.Certificate,
                                string.Format(CultureInfo.InvariantCulture, "{0}?forceDeleteInUse=true",
                                    cert.Thumbprint)));
                        return response;
                    },
                    () => { }
                );
            }
        }

        private static void DeleteManyCertificate(string resourceName, string postDataFilePath)
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("-> Prepare data");
                var thumprints = new List<string>();
                var postData = Helper.GetJsonObjectFromFile<JArray>(postDataFilePath);

                foreach (var data in postData)
                {
                    var rawData = (string)data["RawData"];
                    var cert = new X509Certificate2(Convert.FromBase64String(rawData));
                    thumprints.Add(cert.Thumbprint);
                }

                RestApiCaller.CallAndHandleError
                (
                    () =>
                    {
                        Console.WriteLine("-> Post {0}", resourceName);
                        var response = request.Post(string.Join("/", RequestObject.Certificate, "many"), postData);

                        Console.WriteLine("-> Delete {0}", resourceName);
                        response = request.Delete(string.Join("/", RequestObject.Certificate, "many?forceDeleteInUse=true"), thumprints);
                        Console.WriteLine("-> Got response:");
                        Console.WriteLine("StatusCode: {0}, Content: {1}", response.StatusCode, Helper.ReadResponseContentAsString(response));

                        return response;
                    },
                    () => { }
                );
            }
        }
    }
}
