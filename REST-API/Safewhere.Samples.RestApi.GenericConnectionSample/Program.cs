using System;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.GenericConnectionSample
{
    class Program
    {
        private const string PostDataFile01 = "SampleData/CON_POST_GENERICPROVIDER_01.json";
        private const string PostDataFile02 = "SampleData/CON_POST_GENERICPROVIDER_02.json";
        private const string PutDataFile = "SampleData/CON_PUT_GENERICPROVIDER.json";

        static void Main()
        {
            Console.WriteLine("Begin POST Generic Connection");
            PostGenericConnection();
            Console.WriteLine("End POST Generic Connection" + Environment.NewLine);

            Console.WriteLine("Begin GET Generic Connection");
            GetGenericConnection();
            Console.WriteLine("End GET Generic Connection" + Environment.NewLine);

            Console.WriteLine("Begin GET All Generic Connection");
            GetAllGenericConnection();
            Console.WriteLine("End GET All Generic Connection" + Environment.NewLine);

            Console.WriteLine("Begin PUT Generic Connection");
            PutGenericConnection();
            Console.WriteLine("End PUT Generic Connection" + Environment.NewLine);

            Console.WriteLine("Begin DELETE Generic Connection");
            DeleteGenericConnection();
            Console.WriteLine("End DELETE Generic Connection" + Environment.NewLine);

            Console.WriteLine("All done!");
        }

        private static void PostGenericConnection()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<Connection>(PostDataFile01);

                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Post Generic connection");
                            var response = request.Post(RequestObject.Connections, postData);
                            return response;
                        },
                        () =>
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.Connections, postData.Name))
                    );
            }
        }
		
        private static void PutGenericConnection()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<Connection>(PostDataFile01);
                var putData = Helper.GetJsonObjectFromFile<Connection>(PutDataFile);

                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Generic connection");
                            request.Post(RequestObject.Connections, postData);

                            Console.WriteLine("Put to update Generic connection");
                            var response = request.Put(RequestObject.Connections, putData);
                            return response;
                        },
                        () =>
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.Connections, postData.Name))
                    );
            }
        }
		
        private static void GetGenericConnection()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<Connection>(PostDataFile01);

                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Generic connection");
                            request.Post(RequestObject.Connections, postData);

                            Console.WriteLine("Get Generic connection");
                            var response =
                                request.Get(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                    RequestObject.Connections, postData.Name));
                            return response;
                        },
                        () =>
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.Connections, postData.Name))
                    );
            }
        }

        private static void GetAllGenericConnection()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData01 = Helper.GetJsonObjectFromFile<Connection>(PostDataFile01);
                var postData02 = Helper.GetJsonObjectFromFile<Connection>(PostDataFile02);

                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Generic connections");
                            request.Post(RequestObject.Connections, postData01);
                            request.Post(RequestObject.Connections, postData02);

                            Console.WriteLine("Get  All Generic connections");
                            var response = request.Get(RequestObject.Connections);
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.Connections, postData01.Name));
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.Connections, postData02.Name));
                        }
                    );
            }
        }
		
        private static void DeleteGenericConnection()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData01 = Helper.GetJsonObjectFromFile<Connection>(PostDataFile01);

                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Generic connections");
                            request.Post(RequestObject.Connections, postData01);

                            Console.WriteLine("Delete Generic connection " + postData01.Name);
                            var response =
                                request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                    RequestObject.Connections, postData01.Name));
                            return response;
                        },
                        () =>
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.Connections, postData01.Name))
                    );
            }
        }
    }
}
