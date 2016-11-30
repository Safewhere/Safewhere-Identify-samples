using System;
using System.Collections.Generic;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Groups;

namespace Safewhere.Samples.RestApi.GroupSample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Get Many Groups");
            GetGroups();

            Console.WriteLine("Post Group");
            PostGroup();

            Console.WriteLine("Put Group");
            PutGroup();

            Console.WriteLine("Delete Group");
            DeleteGroup();

            Console.WriteLine("Get Group");
            GetGroup();

            Console.WriteLine("Delete Many Groups");
            DeleteGroups();
            
            Console.WriteLine("All done!");
            Console.ReadLine();
        }

		private static void GetGroup()
		{
			using (var request = new ApiWebRequest())
			{
				var groupData = Helper.GetJsonObjectFromFile<Group>("SampleData/Group.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Groups, groupData);

                           Console.WriteLine("-> Exercise Get");
                           var response = request.Get(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupData.Name));
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupData.Name));
                       }
                   );
			}
		}

        private static void GetGroups()
        {
            using (var request = new ApiWebRequest())
            {
                var groupsData = Helper.GetJsonObjectFromFile<List<Group>>("SampleData/Groups.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Groups, groupsData[0]);
                           request.Post(RequestObject.Groups, groupsData[1]);
                           request.Post(RequestObject.Groups, groupsData[2]);
                           request.Post(RequestObject.Groups, groupsData[3]);

                           Console.WriteLine("-> Exercise Get");
                           var response = request.Get(RequestObject.Groups);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupsData[0].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupsData[1].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupsData[2].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupsData[3].Name));
                       }
                   );
            }
        }

		private static void PutGroup()
		{
			using (var request = new ApiWebRequest())
			{
				var groupData = Helper.GetJsonObjectFromFile<Group>("SampleData/Group.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Groups, groupData);

                           Console.WriteLine("-> Exercise Put");
                           groupData.Description = "GroupDescriptionUpdated";
                           groupData.ClaimValues[0].Value = "OrganizationAdmin";
                           groupData.ClaimValues[1].Value = "ClaimTransformationAdmin";
                           var response = request.Put(RequestObject.Groups, groupData);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupData.Name));
                       }
                   );
			}
		}

		private static void PostGroup()
		{
			using (var request = new ApiWebRequest())
			{
				var groupData = Helper.GetJsonObjectFromFile<Group>("SampleData/Group.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");

                           Console.WriteLine("-> Exercise Post");
                           var response = request.Post(RequestObject.Groups, groupData);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupData.Name));
                       }
                   );
			}
		}

        private static void DeleteGroup()
        {
            using (var request = new ApiWebRequest())
            {
                var groupData = Helper.GetJsonObjectFromFile<Group>("SampleData/Group.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Groups, groupData);

                           Console.WriteLine("-> Exercise Delete");
                           var response = request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupData.Name));
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupData.Name));
                       }
                   );
            }
        }

		private static void DeleteGroups()
		{
			using (var request = new ApiWebRequest())
			{
				var groupsData = Helper.GetJsonObjectFromFile<List<Group>>("SampleData/Groups.json");

                RestApiCaller.CallAndHandleError
                   (
                       () =>
                       {
                           Console.WriteLine("-> Create data");
                           request.Post(RequestObject.Groups, groupsData[0]);
                           request.Post(RequestObject.Groups, groupsData[1]);
                           request.Post(RequestObject.Groups, groupsData[2]);
                           request.Post(RequestObject.Groups, groupsData[3]);

                           Console.WriteLine("-> Exercise Delete many");
                           var deletedGroups = new[]
                    {
                        groupsData[0].Name, groupsData[1].Name, groupsData[2].Name, groupsData[3].Name
                    };
                           var response = request.Delete(RequestObject.GroupsMany, deletedGroups);
                           return response;
                       },
                       () =>
                       {
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupsData[0].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupsData[1].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupsData[2].Name));
                           request.Delete(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", RequestObject.Groups, groupsData[3].Name));
                       }
                   );
			}
		}
    }
}
