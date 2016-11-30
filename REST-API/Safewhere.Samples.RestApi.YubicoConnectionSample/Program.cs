using System;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.YubicoConnectionSample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Begin POST Yubico Connection");
            PostYubicoConnection();
            Console.WriteLine("End POST Yubico Connection");

            Console.WriteLine("Begin PUT Yubico Connection");
            PutYubicoConnection();
            Console.WriteLine("End PUT Yubico Connection");

            Console.WriteLine("Begin DELETE Yubico Connection");
            DeleteYubicoConnection();
            Console.WriteLine("End DELETE Yubico Connection");

            Console.WriteLine("Begin GET Yubico Connection");
            GetYubicoConnection();
            Console.WriteLine("End GET Yubico Connection");

            Console.WriteLine("All done!");
            Console.ReadLine();
        }

        private static void GetYubicoConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/YubicoConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise Get Yubico Connection");
                           var response = request.Get(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                       }
                   );
            }
        }

        private static void DeleteYubicoConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/YubicoConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise DELETE Yubico Connection");
                           var response = request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                       }
                   );
            }
        }

        private static void PutYubicoConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/YubicoConnection.json");
                var connectionUpdate = Helper.GetJsonObjectFromFile<Connection>("SampleData/YubicoConnectionUpdate.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise PUT Yubico Connection");
                           var response = request.Put(RequestObject.Connections, connectionUpdate);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                       }
                   );
            }
        }

        private static void PostYubicoConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/YubicoConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");

                           Console.WriteLine("-> Exercise POST Yubico Connection");
                           var response = request.Post(RequestObject.Connections, connection);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                       }
                   );
            }
        }
    }
}
