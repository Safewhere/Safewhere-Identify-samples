using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.ClaimDefinitions;

namespace Safewhere.Samples.RestApi.ClaimDefinitionSample
{
    class Program
    {
        private const string PostOneDataFile = "SampleData/CLD_POSTONE.json";
        private const string PostManyDataFile = "SampleData/CLD_POSTMANY.json";
        private const string PutOneDataFile = "SampleData/CLD_PUTONE.json";
        private const string PutManyDataFile = "SampleData/CLD_PUTMANY.json";
        private const string SearchDataFile = "SampleData/CLD_POSTSEARCH.json";
        private const string DelDataFile = "SampleData/CLD_DELTYPE.json";
        private const string FilterString = "?filter=claimType%20eq%20%22Free%201%22&pageIndex=1&sortBy=claimType&sortOrder=Ascending";
        private const string ClaimDefinitionsAttribute = "affectedClaimDefinitions";

        private static void Main()
        {
            Console.WriteLine("Begin POST Claim Definition");
            PostClaimDefinition();
            Console.WriteLine("End POST Claim Definition" + Environment.NewLine);

            Console.WriteLine("Begin POST Many Claim Definition");
            PostClaimDefinitionsMany();
            Console.WriteLine("End POST  Many Claim Definition" + Environment.NewLine);

            Console.WriteLine("Begin PUT Claim Definition");
            PutClaimDefinition();
            Console.WriteLine("End PUT  Claim Definition" + Environment.NewLine);

            Console.WriteLine("Begin PUT Many Claim Definition");
            PutClaimDefinitionsMany();
            Console.WriteLine("End PUT  Many Claim Definition" + Environment.NewLine);

            Console.WriteLine("Begin GET Claim Definition by Type");
            GetClaimDefinitionByClaimType();
            Console.WriteLine("End GET  Claim Definition by Type" + Environment.NewLine);

            Console.WriteLine("Begin GET Claim Definition by ID");
            GetClaimDefinitionById();
            Console.WriteLine("End GET  Claim Definition by ID" + Environment.NewLine);

            Console.WriteLine("Begin GET all Claim Definition");
            GetAllClaimDefinition();
            Console.WriteLine("End GET all  Claim Definition" + Environment.NewLine);

            Console.WriteLine("Begin DELETE Claim Definition by ID");
            DeleteClaimDefinitionById();
            Console.WriteLine("End DELETE  Claim Definition by ID" + Environment.NewLine);

            Console.WriteLine("Begin DELETE Claim Definition by Type");
            DeleteClaimDefinitionByType();
            Console.WriteLine("End DELETE  Claim Definition by Type" + Environment.NewLine);

            Console.WriteLine("Begin DELETE Many Claim Definition by ID");
            DeleteClaimDefinitionsManyById();
            Console.WriteLine("End DELETE  Many Claim Definition by ID" + Environment.NewLine);

            Console.WriteLine("Begin DELETE Many Claim Definition by Type");
            DeleteClaimDefinitionsManyByType();
            Console.WriteLine("End DELETE  Many Claim Definition by Type" + Environment.NewLine);

            Console.WriteLine("Begin SEARCH Claim Definition");
            SearchClaimDefinition();
            Console.WriteLine("End SEARCH  Claim Definition" + Environment.NewLine);

            Console.WriteLine("Begin GET Filter Claim Definition");
            GetFilterClaimDefinition();
            Console.WriteLine("End GET Filter Claim Definition" + Environment.NewLine);

            Console.WriteLine("All done!");
        }

        #region Sample client methods
        private static void PostClaimDefinition()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition>(PostOneDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Post Claim Definition");
                            var response = request.Post(RequestObject.ClaimDefinitions, postData);
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.ClaimDefinitions, postData.ClaimType));
                        }
                    );
            }
        }
		
        private static void PostClaimDefinitionsMany()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition[]>(PostManyDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Post Many Claim Definitions");
                            var response = request.Post(RequestObject.ClaimDefinitionsMany, postData);
                            return response;
                        },
                        () =>
                           {
                               var delData = Helper.GetJsonObjectFromFile<JArray>(DelDataFile);
                               request.Delete(
                                   string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.ClaimDefinitions,
                                       RequestObject.ClaimTypes), delData);
                           }
                    );
            }
        }
		
        private static void GetClaimDefinitionByClaimType()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition>(PostOneDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Claim Definition");
                            request.Post(RequestObject.ClaimDefinitions, postData);

                            Console.WriteLine("Get Claim Definition");
                            var response = request.Get(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                    RequestObject.ClaimDefinitions, postData.ClaimType));
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.ClaimDefinitions, postData.ClaimType));
                        }
                    );
            }
        }
		
        private static void GetClaimDefinitionById()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition>(PostOneDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Claim Definition");
                            var postResponse = request.Post(RequestObject.ClaimDefinitions, postData);
                            string id = ExtractClaimAttributeFromResult(postResponse, IdentifyJsonProperties.Id);
                            Console.WriteLine("Get Claim Definition");
                            var response = request.Get(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                    RequestObject.ClaimDefinitions, id));
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.ClaimDefinitions, postData.ClaimType));
                        }
                    );
            }
        }
		
        private static void GetAllClaimDefinition()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition[]>(PostManyDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Claim Definitions");
                            request.Post(RequestObject.ClaimDefinitionsMany, postData);

                            Console.WriteLine("Get All Claim Definitions");
                            HttpResponseMessage response = request.Get(RequestObject.ClaimDefinitions);
                            return response;
                        },
                        () =>
                        {
                            var delData = Helper.GetJsonObjectFromFile<JArray>(DelDataFile);
                            request.Delete(
                                string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.ClaimDefinitions,
                                    RequestObject.ClaimTypes), delData);
                        }
                    );
            }
        }
		
        private static void DeleteClaimDefinitionById()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition>(PostOneDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Claim Definition");
                            var postResponse = request.Post(RequestObject.ClaimDefinitions, postData);
                            string id = ExtractClaimAttributeFromResult(postResponse, IdentifyJsonProperties.Id);

                            Console.WriteLine("Delete by Id Claim Definition " + postData.Name);
                            var response =
                                request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                    RequestObject.ClaimDefinitions, id));
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.ClaimDefinitions, postData.ClaimType));
                        }
                    );
            }
        }
		
        private static void DeleteClaimDefinitionByType()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition>(PostOneDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Claim Definition");
                            request.Post(RequestObject.ClaimDefinitions, postData);

                            Console.WriteLine("Delete by Type Claim Definition " + postData.Name);
                            var response =
                                request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                    RequestObject.ClaimDefinitions,
                                    postData.ClaimType));
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.ClaimDefinitions, postData.ClaimType));
                        }
                    );
            }
        }
		
        private static void DeleteClaimDefinitionsManyById()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition[]>(PostManyDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            var postResponse = request.Post(RequestObject.ClaimDefinitionsMany, postData);
                            var guids = ExtractArrayClaimIdsFromResult(postResponse);
                            Console.WriteLine("Delete Claim Definitions");
                            var response = request.Delete(
                                string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.ClaimDefinitions, RequestObject.Ids), guids);
                            return response;
                        },
                        () =>
                        {
                            var delData = Helper.GetJsonObjectFromFile<JArray>(DelDataFile);
                            request.Delete(
                                string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.ClaimDefinitions,
                                    RequestObject.ClaimTypes), delData);
                        }
                    );
            }
        }
		
        private static void DeleteClaimDefinitionsManyByType()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition[]>(PostManyDataFile);
                var delData = Helper.GetJsonObjectFromFile<JArray>(DelDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            request.Post(RequestObject.ClaimDefinitionsMany, postData);

                            Console.WriteLine("Delete Claim Definitions");
                            var response =
                                request.Delete(
                                    string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                        RequestObject.ClaimDefinitions, RequestObject.ClaimTypes), delData);
                            return response;
                        },
                        () =>
                        {
                            request.Delete(
                                string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.ClaimDefinitions,
                                    RequestObject.ClaimTypes), delData);
                        }
                    );
            }
        }
		
        private static void PutClaimDefinition()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition>(PostOneDataFile);
                var putData = Helper.GetJsonObjectFromFile<ClaimDefinition>(PutOneDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Claim Definition");
                            request.Post(RequestObject.ClaimDefinitions, postData);

                            Console.WriteLine("Put to update Claim Definition");
                            var response = request.Put(RequestObject.ClaimDefinitions, putData);
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                RequestObject.ClaimDefinitions, postData.ClaimType));
                        }
                    );
            }
        }
		
        private static void PutClaimDefinitionsMany()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition[]>(PostManyDataFile);
                var putData = Helper.GetJsonObjectFromFile<ClaimDefinition[]>(PutManyDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            request.Post(RequestObject.ClaimDefinitionsMany, postData);

                            Console.WriteLine("Put Many Claim Definition");
                            var response = request.Put(RequestObject.ClaimDefinitionsMany, putData);
                            return response;
                        },
                        () =>
                        {
                            var delData = Helper.GetJsonObjectFromFile<JArray>(DelDataFile);
                            request.Delete(
                                string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.ClaimDefinitions,
                                    RequestObject.ClaimTypes), delData);
                        }
                    );
            }
        }
		
        private static void SearchClaimDefinition()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition[]>(PostManyDataFile);
                var searchData = Helper.GetJsonObjectFromFile<JObject>(SearchDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Claim Definitions");
                            request.Post(RequestObject.ClaimDefinitionsMany, postData);

                            Console.WriteLine("Search Claim Definition");
                            var response = request.Post(RequestObject.ClaimDefinitionsSearch, searchData);
                            return response;
                        },
                        () =>
                        {
                            var delData = Helper.GetJsonObjectFromFile<JArray>(DelDataFile);
                            request.Delete(
                                string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.ClaimDefinitions,
                                    RequestObject.ClaimTypes), delData);
                        }
                    );
            }
        }
		
        private static void GetFilterClaimDefinition()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<ClaimDefinition[]>(PostManyDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Claim Definitions");
                            request.Post(RequestObject.ClaimDefinitionsMany, postData);

                            Console.WriteLine("Get Filter Claim Definition with Filter string: {0}", FilterString);
                            var response = request.Get(RequestObject.ClaimDefinitionsFilter + FilterString);
                            return response;
                        },
                        () =>
                        {
                            var delData = Helper.GetJsonObjectFromFile<JArray>(DelDataFile);
                            request.Delete(
                                string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.ClaimDefinitions,
                                    RequestObject.ClaimTypes), delData);
                        }
                    );
            }
        }
		
        private static string ExtractClaimAttributeFromResult(HttpResponseMessage response, string attribute)
        {
            string ret = "";
            var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            if (json[attribute] != null)
            {
                ret = json[attribute].ToString();
            }
            return ret;
        }

        private static string[] ExtractArrayClaimIdsFromResult(HttpResponseMessage response)
        {
            var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            var resources = json[ClaimDefinitionsAttribute].ToString();
            var claims = JsonConvert.DeserializeObject<ClaimDefinition[]>(resources);
            return claims.Select(item => item.Id).ToArray<string>();
        }
        #endregion

    }
}
