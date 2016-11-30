using System;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.ClaimTransformations;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.LiveIdConnectionSample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Begin POST Live Id Connection");
            PostLiveIdConnection();
            Console.WriteLine("End POST Live Id Connection");

            Console.WriteLine("Begin PUT Live Id Connection");
            PutLiveIdConnection();
            Console.WriteLine("End PUT Live Id Connection");

            Console.WriteLine("Begin DELETE Live Id Connection");
            DeleteLiveIdConnection();
            Console.WriteLine("End DELETE Live Id Connection");

            Console.WriteLine("Begin GET Live Id Connection");
            GetLiveIdConnection();
            Console.WriteLine("End GET Live Id Connection");

            Console.WriteLine("Begin Put claim transformation Live Id Connection");
            PutClaimTransformationLiveIdConnection();
            Console.WriteLine("End Put claim transformation Live Id Connection");

            Console.WriteLine("Begin Delete claim transformation Live Id Connection");
            DeleteClaimTransformationLiveIdConnection();
            Console.WriteLine("End Delete claim transformation Live Id Connection");

            Console.WriteLine("Begin Put many claim transformation Live Id Connection");
            PutClaimTransformationsLiveIdConnection();
            Console.WriteLine("End Put many claim transformation Live Id Connection");

            Console.WriteLine("Begin Delete many claim transformation Live Id Connection");
            DeleteClaimTransformationsLiveIdConnection();
            Console.WriteLine("End Delete many claim transformation Live Id Connection");

            Console.WriteLine("All Done!");
            Console.ReadLine();
        }

        private static void DeleteClaimTransformationsLiveIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/LiveIdConnectionWithClaimTransformation.json");
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


                           Console.WriteLine("-> Exercise Delete many claim transformation Live Id Connection");
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

        private static void PutClaimTransformationsLiveIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/LiveIdConnectionWithClaimTransformation.json");
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


                           Console.WriteLine("-> Exercise Put many claim transformation Live Id Connection");
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

        private static void DeleteClaimTransformationLiveIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/LiveIdConnectionWithClaimTransformation.json");
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


                           Console.WriteLine("-> Exercise Delete claim transformation Live Id Connection");
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

        private static void PutClaimTransformationLiveIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/LiveIdConnectionWithClaimTransformation.json");
                var claimTransformation = Helper.GetJsonObjectFromFile<CreateExcludeTransformationRequest>("SampleData/ClaimTransformation.json");
                var putClaimTransformation = Helper.GetJsonObjectFromFile<ConnectionClaimTransformation>("SampleData/PutClaimTransformation.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.ClaimTransformationExcludeIdentify, claimTransformation);
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise Put claim transformation Live Id Connection");
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

        private static void GetLiveIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/LiveIdConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise GET Live Id Connection");
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

        private static void DeleteLiveIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/LiveIdConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise Delete Live Id Connection");
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

        private static void PutLiveIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/LiveIdConnection.json");
                var connectionUpdate = Helper.GetJsonObjectFromFile<Connection>("SampleData/LiveIdConnectionUpdate.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise PUT Live Id Connection");
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

        private static void PostLiveIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/LiveIdConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");

                           Console.WriteLine("-> Exercise POST Live Id Connection");
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
