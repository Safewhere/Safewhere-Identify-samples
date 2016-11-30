using System;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.ClaimTransformations;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.OpenIdConnectionSample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Begin POST Open Id Connection");
            PostOpenIdConnection();
            Console.WriteLine("End POST Open Id Connection");

            Console.WriteLine("Begin PUT Open Id Connection");
            PutOpenIdConnection();
            Console.WriteLine("End PUT Open Id Connection");

            Console.WriteLine("Begin DELETE Open Id Connection");
            DeleteOpenIdConnection();
            Console.WriteLine("End DELETE Open Id Connection");

            Console.WriteLine("Begin GET Open Id Connection");
            GetOpenIdConnection();
            Console.WriteLine("End GET Open Id Connection");

            Console.WriteLine("Begin Put claim transformation Open Id Connection");
            PutClaimTransformationOpenIdConnection();
            Console.WriteLine("End Put claim transformation Open Id Connection");

            Console.WriteLine("Begin Delete claim transformation Open Id Connection");
            DeleteClaimTransformationOpenIdConnection();
            Console.WriteLine("End Delete claim transformation Open Id Connection");

            Console.WriteLine("Begin Put many claim transformation Open Id Connection");
            PutClaimTransformationsOpenIdConnection();
            Console.WriteLine("End Put many claim transformation Open Id Connection");

            Console.WriteLine("Begin Delete many claim transformation Open Id Connection");
            DeleteClaimTransformationsOpenIdConnection();
            Console.WriteLine("End Delete many claim transformation Open Id Connection");

            Console.WriteLine("All Done!");
            Console.ReadLine();
        }

        private static void DeleteClaimTransformationsOpenIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OpenIdConnectionWithClaimTransformation.json");
                var claimTransformation = Helper.GetJsonObjectFromFile<CreateExcludeTransformationRequest>("SampleData/ClaimTransformation.json");
                var claimTransformation1 = Helper.GetJsonObjectFromFile<CreateExcludeTransformationRequest>("SampleData/ClaimTransformation1.json");
                var putClaimTransformations = Helper.GetJsonObjectFromFile<object>("SampleData/PutClaimTransformations.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.ClaimTransformationExcludeIdentify, claimTransformation);
                           request.Post(RequestObject.ClaimTransformationExcludeIdentify, claimTransformation1);
                           request.Post(RequestObject.Connections, connection);
                           request.Put(RequestObject.ConnectionsClaimTransformations, putClaimTransformations);


                           Console.WriteLine("-> Exercise Delete many claim transformation Open Id Connection");
                           var response = request.Delete(RequestObject.ConnectionsClaimTransformations, putClaimTransformations);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.ClaimTransformationExcludeIdentify, claimTransformation.TransformationName));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.ClaimTransformationExcludeIdentify, claimTransformation1.TransformationName));
                       }
                   );
            }
        }

        private static void PutClaimTransformationsOpenIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OpenIdConnectionWithClaimTransformation.json");
                var claimTransformation = Helper.GetJsonObjectFromFile<CreateExcludeTransformationRequest>("SampleData/ClaimTransformation.json");
                var claimTransformation1 = Helper.GetJsonObjectFromFile<CreateExcludeTransformationRequest>("SampleData/ClaimTransformation1.json");
                var putClaimTransformations = Helper.GetJsonObjectFromFile<object>("SampleData/PutClaimTransformations.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.ClaimTransformationExcludeIdentify, claimTransformation);
                           request.Post(RequestObject.ClaimTransformationExcludeIdentify, claimTransformation1);
                           request.Post(RequestObject.Connections, connection);


                           Console.WriteLine("-> Exercise Put many claim transformation Open Id Connection");
                           var response = request.Put(RequestObject.ConnectionsClaimTransformations, putClaimTransformations);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.ClaimTransformationExcludeIdentify, claimTransformation.TransformationName));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.ClaimTransformationExcludeIdentify, claimTransformation.TransformationName));
                       }
                   );
            }
        }

        private static void DeleteClaimTransformationOpenIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OpenIdConnectionWithClaimTransformation.json");
                var claimTransformation = Helper.GetJsonObjectFromFile<CreateExcludeTransformationRequest>("SampleData/ClaimTransformation.json");
                var putClaimTransformation = Helper.GetJsonObjectFromFile<ConnectionClaimTransformation>("SampleData/PutClaimTransformation.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.ClaimTransformationExcludeIdentify, claimTransformation);
                           request.Post(RequestObject.Connections, connection);
                           request.Put(RequestObject.ConnectionsClaimTransformation, putClaimTransformation);


                           Console.WriteLine("-> Exercise Delete claim transformation Open Id Connection");
                           var response = request.Delete(RequestObject.ConnectionsClaimTransformation, putClaimTransformation);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.ClaimTransformationExcludeIdentify, claimTransformation.TransformationName));
                       }
                   );
            }
        }

        private static void PutClaimTransformationOpenIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OpenIdConnectionWithClaimTransformation.json");
                var claimTransformation = Helper.GetJsonObjectFromFile<CreateExcludeTransformationRequest>("SampleData/ClaimTransformation.json");
                var putClaimTransformation = Helper.GetJsonObjectFromFile<ConnectionClaimTransformation>("SampleData/PutClaimTransformation.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.ClaimTransformationExcludeIdentify, claimTransformation);
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise Put claim transformation Open Id Connection");
                           var response = request.Put(RequestObject.ConnectionsClaimTransformation, putClaimTransformation);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.ClaimTransformationExcludeIdentify, claimTransformation.TransformationName));
                       }
                   );
            }
        }

        private static void GetOpenIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OpenIdConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise GET Open Id Connection");
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

        private static void DeleteOpenIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OpenIdConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise Delete Open Id Connection");
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

        private static void PutOpenIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OpenIdConnection.json");
                var connectionUpdate = Helper.GetJsonObjectFromFile<Connection>("SampleData/OpenIdConnectionUpdate.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise PUT Open Id Connection");
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

        private static void PostOpenIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/OpenIdConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");

                           Console.WriteLine("-> Exercise POST Open Id Connection");
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
