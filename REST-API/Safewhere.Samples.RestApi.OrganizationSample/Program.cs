using System;
using System.Collections.Generic;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Organizations;

namespace Safewhere.Samples.RestApi.OrganizationSample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Get Many Organizations");
            GetOrganizations();

            Console.WriteLine("Post Organization");
            PostOrganization();

            Console.WriteLine("Put Organization");
            PutOrganization();

            Console.WriteLine("Delete Organization");
            DeleteOrganization();

            Console.WriteLine("Get Organization");
            GetOrganization();

            Console.WriteLine("Get Childs Organization");
            GetChildsOrganization();

            Console.WriteLine("Delete Many Organizations");
            DeleteOrganizations();

            Console.WriteLine("All done!");
            Console.ReadLine();
        }

        private static void GetChildsOrganization()
        {
            using (var request = new ApiWebRequest())
            {
                var organizationsData = Helper.GetJsonObjectFromFile<List<Organization>>("SampleData/Organizations.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Organizations, organizationsData[0]);
                           request.Post(RequestObject.Organizations, organizationsData[1]);
                           request.Post(RequestObject.Organizations, organizationsData[2]);
                           request.Post(RequestObject.Organizations, organizationsData[3]);

                           Console.WriteLine("-> Exercise Get Childs Organization");
                           var response = request.Get(string.Format(CultureInfo.CurrentCulture, RequestObject.OrganizationsChilds, organizationsData[0].Name));
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[3].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[2].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[1].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[0].Name));
                       }
                   );
            }
        }

        private static void DeleteOrganizations()
        {
            using (var request = new ApiWebRequest())
            {
                var organizationsData = Helper.GetJsonObjectFromFile<List<Organization>>("SampleData/Organizations.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Organizations, organizationsData[0]);
                           request.Post(RequestObject.Organizations, organizationsData[1]);
                           request.Post(RequestObject.Organizations, organizationsData[2]);
                           request.Post(RequestObject.Organizations, organizationsData[3]);

                           Console.WriteLine("-> Exercise Delete many");
                           var deletedGroups = new[]
                            {
                                organizationsData[1].Name, organizationsData[2].Name, organizationsData[3].Name,organizationsData[0].Name
                            };
                           var response = request.Delete(RequestObject.OrganizationsMany, deletedGroups);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[0].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[1].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[2].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[3].Name));
                       }
                   );
            }
        }

        private static void GetOrganization()
        {
            using (var request = new ApiWebRequest())
            {
                var organizationData = Helper.GetJsonObjectFromFile<Organization>("SampleData/Organization.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Organizations, organizationData);

                           Console.WriteLine("-> Exercise Get");
                           var response = request.Get(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationData.Name));
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationData.Name));
                       }
                   );
            }
        }

        private static void DeleteOrganization()
        {
            using (var request = new ApiWebRequest())
            {
                var organizationData = Helper.GetJsonObjectFromFile<Organization>("SampleData/Organization.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Organizations, organizationData);

                           Console.WriteLine("-> Exercise Delete");
                           var response = request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationData.Name));
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationData.Name));
                       }
                   );
            }
        }

        private static void PutOrganization()
        {
            using (var request = new ApiWebRequest())
            {
                var organizationData = Helper.GetJsonObjectFromFile<Organization>("SampleData/Organization.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Organizations, organizationData);

                           Console.WriteLine("-> Exercise Put");
                           organizationData.Description = "OrganizationDescriptionUpdated";
                           organizationData.DisplayName = "OrganizationDisplayNameUpdated";
                           organizationData.ExpiryDisallowLoginDays = 99;
                           organizationData.ForceResetPasswordDefaultValue = true;
                           organizationData.PasswordExpiryDays = 100;
                           var response = request.Put(RequestObject.Organizations, organizationData);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationData.Name));
                       }
                   );
            }
        }

        private static void PostOrganization()
        {
            using (var request = new ApiWebRequest())
            {
                var organizationData = Helper.GetJsonObjectFromFile<Organization>("SampleData/Organization.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");

                           Console.WriteLine("-> Exercise Post");
                           var response = request.Post(RequestObject.Organizations, organizationData);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationData.Name));
                       }
                   );
            }
        }

        private static void GetOrganizations()
        {
            using (var request = new ApiWebRequest())
            {
                var organizationsData = Helper.GetJsonObjectFromFile<List<Organization>>("SampleData/Organizations.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Organizations, organizationsData[0]);
                           request.Post(RequestObject.Organizations, organizationsData[1]);
                           request.Post(RequestObject.Organizations, organizationsData[2]);
                           request.Post(RequestObject.Organizations, organizationsData[3]);

                           Console.WriteLine("-> Exercise Get");
                           var response = request.Get(RequestObject.Organizations);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[0].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[1].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[2].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Organizations, organizationsData[3].Name));
                       }
                   );
            }
        }
    }
}
