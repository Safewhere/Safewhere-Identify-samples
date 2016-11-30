using System;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.OneTimePasswordConnectionSample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Begin POST One Time Password Connection ");
            PostOneTimePasswordConnection();
            Console.WriteLine("End POST One Time Password Connection ");

            Console.WriteLine("Begin PUT One Time Password Connection ");
            PutOneTimePasswordConnection();
            Console.WriteLine("End PUT One Time Password Connection ");

            Console.WriteLine("Begin DELETE One Time Password Connection ");
            DeleteOneTimePasswordConnection();
            Console.WriteLine("End DELETE One Time Password Connection ");

            Console.WriteLine("Begin GET One Time Password Connection ");
            GetOneTimePasswordConnection();
            Console.WriteLine("End GET One Time Password Connection ");

            Console.WriteLine("All done!");
            Console.ReadLine();
        }

        private static void GetOneTimePasswordConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OneTimePasswordConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise Get One Time Password Connection ");
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

        private static void DeleteOneTimePasswordConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OneTimePasswordConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise DELETE One Time Password Connection ");
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

        private static void PutOneTimePasswordConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OneTimePasswordConnection.json");
                var connectionUpdate = Helper.GetJsonObjectFromFile<Connection>("SampleData/OneTimePasswordConnectionUpdate.json");
                

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise PUT One Time Password Connection ");
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

        private static void PostOneTimePasswordConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OneTimePasswordConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");

                           Console.WriteLine("-> Exercise POST One Time Password Connection ");
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
