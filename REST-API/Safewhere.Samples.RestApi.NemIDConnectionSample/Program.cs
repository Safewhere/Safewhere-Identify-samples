using System;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.ClaimTransformations;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.NemIDConnectionSample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Begin POST NemId connection");
            PostNemIdConnection();
            Console.WriteLine("End POST NemId connection");

            Console.WriteLine("Begin PUT NemId connection");
            PutNemIdConnection();
            Console.WriteLine("End PUT NemId connection");

            Console.WriteLine("Begin DELETE NemId connection");
            DeleteNemIdConnection();
            Console.WriteLine("End DELETE NemId connection");

            Console.WriteLine("Begin GET NemId connection");
            GetNemIdConnection();
            Console.WriteLine("End GET NemId connection");

            Console.WriteLine("Begin PUT certificate for NemId connection");
            PutCertificateNemIdConnection();
            Console.WriteLine("End PUT certificate for NemId connection");

            Console.WriteLine("Begin GET certificate for NemId connection");
            GetCertificateNemIdConnection();
            Console.WriteLine("End GET certificate for NemId connection");

            Console.WriteLine("Begin Put claim transformation NemId connection");
            PutClaimTransformationNemIdConnection();
            Console.WriteLine("End Put claim transformation NemId connection");

            Console.WriteLine("Begin Delete claim transformation NemId connection");
            DeleteClaimTransformationNemIdConnection();
            Console.WriteLine("End Delete claim transformation NemId connection");

            Console.WriteLine("Begin Put many claim transformation NemId connection");
            PutClaimTransformationsNemIdConnection();
            Console.WriteLine("End Put many claim transformation NemId connection");

            Console.WriteLine("Begin Delete many claim transformation NemId connection");
            DeleteClaimTransformationsNemIdConnection();
            Console.WriteLine("End Delete many claim transformation NemId connection");

            Console.WriteLine("All Done!");
            Console.ReadLine();
        }

        private static void DeleteClaimTransformationsNemIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/NemIdConnectionWithClaimTransformation.json");
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


                           Console.WriteLine("-> Exercise Delete many claim transformation NemId connection");
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

        private static void PutClaimTransformationsNemIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/NemIdConnectionWithClaimTransformation.json");
                var claimTransformation = Helper.GetJsonObjectFromFile<CreateExcludeTransformationRequest>("SampleData/ClaimTransformation.json");
                var claimTransformation1 = Helper.GetJsonObjectFromFile<CreateExcludeTransformationRequest>("SampleData/ClaimTransformation1.json");
                var putClaimTransformations = Helper.GetJsonObjectFromFile<object>("SampleData/PutClaimTransformations.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           request.Post(RequestObject.ClaimTransformationExcludeIdentify, claimTransformation);
                           request.Post(RequestObject.ClaimTransformationExcludeIdentify, claimTransformation1);
                           request.Post(RequestObject.Connections, connection);


                           Console.WriteLine("-> Exercise Put many claim transformation NemId connection");
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

        private static void DeleteClaimTransformationNemIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/NemIdConnectionWithClaimTransformation.json");
                var claimTransformation = Helper.GetJsonObjectFromFile<CreateExcludeTransformationRequest>("SampleData/ClaimTransformation.json");
                var putClaimTransformation = Helper.GetJsonObjectFromFile<ConnectionClaimTransformation>("SampleData/PutClaimTransformation.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           request.Post(RequestObject.ClaimTransformationExcludeIdentify, claimTransformation);
                           request.Post(RequestObject.Connections, connection);
                           request.Put(RequestObject.ConnectionsClaimTransformation, putClaimTransformation);


                           Console.WriteLine("-> Exercise Delete claim transformation NemId connection");
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

        private static void PutClaimTransformationNemIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/NemIdConnectionWithClaimTransformation.json");
                var claimTransformation = Helper.GetJsonObjectFromFile<CreateExcludeTransformationRequest>("SampleData/ClaimTransformation.json");
                var putClaimTransformation = Helper.GetJsonObjectFromFile<ConnectionClaimTransformation>("SampleData/PutClaimTransformation.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.ClaimTransformationExcludeIdentify, claimTransformation);
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise Put claim transformation NemId connection");
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

        private static void GetCertificateNemIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/NemIdConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise GET certificate for NemId connection");
                           var response = request.Get(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.ConnectionsCertificate, connection.Name));
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                       }
                   );
            }
        }

        private static void PutCertificateNemIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/NemIdConnection.json");
                var certificate = Helper.GetJsonObjectFromFile<ConnectionCertificate>("SampleData/Certificate.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise Put certificate for NemId connection");
                           var response = request.Put(RequestObject.ConnectionsCertificate, certificate);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Connections, connection.Name));
                       }
                   );
            }
        }

        private static void GetNemIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/NemIdConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise GET NemId connection");
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

        private static void DeleteNemIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/NemIdConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise Delete NemId connection");
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

        private static void PutNemIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/NemIdConnection.json");
                var connectionUpdate = Helper.GetJsonObjectFromFile<Connection>("SampleData/NemIdConnectionUpdate.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Connections, connection);

                           Console.WriteLine("-> Exercise PUT NemId connection");
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

        private static void PostNemIdConnection()
        {
            using (var request = new ApiWebRequest())
            {
                var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/NemIdConnection.json");
                
                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");

                           Console.WriteLine("-> Exercise POST NemId connection");
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
