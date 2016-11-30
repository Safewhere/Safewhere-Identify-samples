using System;
using System.Net;
using Newtonsoft.Json;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel;

namespace Safewhere.Samples.RestApi.SystemSetupSample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Begin Get System Setup");
            GetSystemSetup();
            Console.WriteLine("End Get System Setup");

            Console.WriteLine("Begin PUT System Setup");
            PutSystemSetup();
            Console.WriteLine("End PUT System Setup");

            Console.WriteLine("All Done!");
            Console.ReadLine();
        }

        private static void GetSystemSetup()
        {
            using (var request = new ApiWebRequest())
            {
                var response = request.Get(RequestObject.SystemSetup);
                Console.WriteLine("-> Response:");
                Console.WriteLine("Status code: {0}, Content {1}", response.StatusCode, Helper.ReadResponseContentAsString(response));
            }
        }

        private static void PutSystemSetup()
        {
            using (var request = new ApiWebRequest())
            {
                var rollbackSystemSetup = JsonConvert.DeserializeObject<SystemSetup>(
                    Helper.ReadResponseContentAsString(request.Get(RequestObject.SystemSetup)));
                
                var systemSetup = Helper.GetJsonObjectFromFile<SystemSetup>("SystemSetup.json");
                var response = request.Put(RequestObject.SystemSetup, systemSetup);
                Console.WriteLine("-> Response:");
                Console.WriteLine("Status code: {0}, Content {1}", response.StatusCode, Helper.ReadResponseContentAsString(response));

                Console.WriteLine("-> Rollback System Setup Settings");
                response = request.Put(RequestObject.SystemSetup, rollbackSystemSetup);
                var isRollbackSuccess = response.StatusCode == HttpStatusCode.OK ?
                "Success" : "Failed";
                Console.WriteLine("Rollback is {0}", isRollbackSuccess);
            }
        }
    }
}
