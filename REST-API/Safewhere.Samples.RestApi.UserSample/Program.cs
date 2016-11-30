using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Groups;
using Safewhere.SCIMModel.Organizations;
using Safewhere.SCIMModel.Users;

namespace Safewhere.Samples.RestApi.UserSample
{
    class Program
    {
        private const string GroupDataFile = "SampleData/Group.json";
        private const string OrganizationDataFile = "SampleData/Organization.json";
        private const string PostDataFile01 = "SampleData/User_1.json";
        private const string PostDataFile02 = "SampleData/User_2.json";
        private const string PostMassDataFile = "SampleData/Users.json";
        private const string PutDataFile = "SampleData/Put_User.json";
        private const string PutMassDataFile = "SampleData/Put_Users.json";
        private const string SearchDataFile = "SampleData/Search_User_Attribute.json";
        private const string PatchUpdateDataFile = "SampleData/Patch_UserId.json";

        private static void Main()
        {
            Console.WriteLine("Prepare Group & Organization data");
            PreparePrerequisiteData();

            Console.WriteLine("Begin POST User");
            PostUser();
            Console.WriteLine("End POST User" + Environment.NewLine);

            Console.WriteLine("Begin POST Mass User");
            PostMassUsers();
            Console.WriteLine("End POST  Mass User" + Environment.NewLine);

            Console.WriteLine("Begin PUT User");
            PutUser();
            Console.WriteLine("End PUT  User" + Environment.NewLine);

            Console.WriteLine("Begin PUT Mass User");
            PutMassUsers();
            Console.WriteLine("End PUT  Mass User" + Environment.NewLine);

            Console.WriteLine("Begin DELETE User");
            DeleteUser();
            Console.WriteLine("End DELETE  User" + Environment.NewLine);

            Console.WriteLine("Begin DELETE Mass User");
            DeleteMassUsers();
            Console.WriteLine("End DELETE  Mass User" + Environment.NewLine);

            Console.WriteLine("Begin GET User");
            GetUser();
            Console.WriteLine("End GET  User" + Environment.NewLine);

            Console.WriteLine("Begin GET all User");
            GetAllUser();
            Console.WriteLine("End GET all  User" + Environment.NewLine);

            Console.WriteLine("Begin PATCH User");
            PatchUser();
            Console.WriteLine("End PATCH  User" + Environment.NewLine);

            Console.WriteLine("Begin SEARCH User");
            SearchUser();
            Console.WriteLine("End SEARCH  User" + Environment.NewLine);

            Console.WriteLine("Clean up data");
            CleanUpData();
            Console.WriteLine("All done!");
        }

        #region Sample client methods
        private static void DeleteUser()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<User>(PostDataFile01);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new User");
                            request.Post(RequestObject.Users, postData);

                            Console.WriteLine("Delete User " + postData.Name);
                            var response =
                                request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                    RequestObject.Users, postData.Id));
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Users,
                                postData.Name));
                        }
                    );
            }
        }
		
        private static void GetAllUser()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData1 = Helper.GetJsonObjectFromFile<User>(PostDataFile01);
                var postData2 = Helper.GetJsonObjectFromFile<User>(PostDataFile02);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Users");
                            request.Post(RequestObject.Users, postData1);
                            request.Post(RequestObject.Users, postData2);

                            Console.WriteLine("Get All Users");
                            HttpResponseMessage response = request.Get(RequestObject.Users);
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Users,
                                postData1.Id));
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Users,
                                postData2.Id));
                        }
                    );
            }
        }
		
        private static void GetUser()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<User>(PostDataFile01);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new User");
                            request.Post(RequestObject.Users, postData);

                            Console.WriteLine("Get User");
                            HttpResponseMessage response =
                                request.Get(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Users,
                                    postData.Name));
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Users,
                                postData.Id));
                        }
                    );
            }
        }
		
        private static void PostMassUsers()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<User[]>(PostMassDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Post Mass User");
                            HttpResponseMessage response = request.Post(RequestObject.MassUsers, postData);
                            return response;
                        },
                        () =>
                        {
                            List<Guid> delData = (from user in postData select new Guid(user.Id)).ToList<Guid>();
                            request.Delete(RequestObject.MassUsers, delData);
                        }
                    );
            }
        }
		
        private static void DeleteMassUsers()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<User[]>(PostMassDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            request.Post(RequestObject.MassUsers, postData);

                            Console.WriteLine("Delete users");
                            List<Guid> delData = (from user in postData select new Guid(user.Id)).ToList<Guid>();
                            var response = request.Delete(RequestObject.MassUsers, delData);
                            return response;
                        },
                        () => { }
                    );
            }
        }
		
        private static void PostUser()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<User>(PostDataFile01);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Post User");
                            var response = request.Post(RequestObject.Users, postData);
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Users,
                                postData.Id));
                        }
                    );
            }
        }
		
        private static void PutUser()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<User>(PostDataFile01);
                var putData = Helper.GetJsonObjectFromFile<User>(PutDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new User");
                            request.Post(RequestObject.Users, postData);
                            Console.WriteLine("Put to update User");
                            var response = request.Put<User>(RequestObject.Users, putData);
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Users,
                                postData.Id));
                        }
                    );
            }
        }
		
        private static void PutMassUsers()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<User[]>(PostMassDataFile);
                var putData = Helper.GetJsonObjectFromFile<User[]>(PutMassDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            request.Post(RequestObject.MassUsers, postData);

                            Console.WriteLine("Put Mass User");
                            HttpResponseMessage response = request.Put(RequestObject.MassUsers, putData);
                            return response;
                        },
                        () =>
                        {
                            List<Guid> delData = (from user in putData select new Guid(user.Id)).ToList<Guid>();
                            request.Delete(RequestObject.MassUsers, delData);
                        }
                    );
            }
        }
		
        private static void PatchUser()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<User>(PostDataFile01);
                var patchData = Helper.GetJsonObjectFromFile<JArray>(PatchUpdateDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Post prepare User");
                            request.Post(RequestObject.Users, postData);

                            Console.WriteLine("Patch User");
                            var response =
                                request.Patch<JArray>(
                                    string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Users,
                                        postData.Id), patchData);
                            return response;
                        },
                        () =>
                        {
                            request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Users,
                                postData.Id));
                        }
                    );
            }
        }
		
        private static void SearchUser()
        {
            using (var request = new ApiWebRequest())
            {
                Console.WriteLine("Prepare data");
                var postData = Helper.GetJsonObjectFromFile<User[]>(PostMassDataFile);
                var searchData = Helper.GetJsonObjectFromFile<JObject>(SearchDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Create new Users");
                            request.Post(RequestObject.MassUsers, postData);

                            Console.WriteLine("Search User");
                            var response = request.Post(RequestObject.UsersSearch, searchData);
                            return response;
                        },
                        () =>
                           {
                               List<Guid> delData = (from user in postData select new Guid(user.Id)).ToList<Guid>();
                               request.Delete(RequestObject.MassUsers, delData);
                           }
                    );
            }
        }
        #endregion

        #region Data Preparation
        private static void PostOrganization()
        {
            using (var request = new ApiWebRequest())
            {
                var postData = Helper.GetJsonObjectFromFile<Organization>(OrganizationDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            HttpResponseMessage message = request.Post(RequestObject.Organizations, postData);
                            return message;
                        },
                        () => { }
                    );
            }
        }
		
        private static void PostGroup()
        {
            using (var request = new ApiWebRequest())
            {
                var postData = Helper.GetJsonObjectFromFile<Group>(GroupDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            HttpResponseMessage message = request.Post(RequestObject.Groups, postData);
                            return message;
                        },
                        () => { }
                    );
            }
        }
		
        private static void DeleteGroup()
        {
            using (var request = new ApiWebRequest())
            {
                var postData = Helper.GetJsonObjectFromFile<Group>(GroupDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Clean up groups");
                            HttpResponseMessage message =
                                request.Delete(string.Format(CultureInfo.InvariantCulture,
                                    "{0}/{1}", RequestObject.Groups, postData.Name));
                            return message;
                        },
                        () => { }
                    );
            }
        }
		
        private static void DeleteOrganization()
        {
            using (var request = new ApiWebRequest())
            {
                var postData = Helper.GetJsonObjectFromFile<Organization>(OrganizationDataFile);
                RestApiCaller.CallAndHandleError
                    (
                        () =>
                        {
                            Console.WriteLine("Clean up organizations");
                            HttpResponseMessage message =
                                request.Delete(string.Format(CultureInfo.InvariantCulture,
                                    "{0}/{1}", RequestObject.Organizations, postData.Name));
                            return message;
                        },
                        () => { }
                    );
            }
        }
		
        private static void PreparePrerequisiteData()
        {
            PostGroup();
            PostOrganization();
        }

        private static void CleanUpData()
        {
            DeleteGroup();
            DeleteOrganization();
        }
        #endregion

    }
}
