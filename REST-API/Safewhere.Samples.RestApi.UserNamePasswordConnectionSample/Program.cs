using System;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.UserNamePasswordConnectionSample
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("Begin POST UserNamePassword connection");
			PostUserNamePasswordConnection();
			Console.WriteLine("End POST UserNamePassword connection Sample\n");

			Console.WriteLine("Begin PUT UserNamePassword connection");
			PutUserNamePasswordConnection();
			Console.WriteLine("End PUT UserNamePassword connection Sample\n");

			Console.WriteLine("Begin GET UserNamePassword connection");
			GetUserNamePasswordConnection();
			Console.WriteLine("End GET UserNamePassword connection Sample\n");

			Console.WriteLine("Begin DELETE UserNamePassword connection");
			DeleteUserNamePasswordConnection();
			Console.WriteLine("End DELETE UserNamePassword connection Sample\n");

			Console.WriteLine("All done!");
		}

		private static void PostUserNamePasswordConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostUserNamePasswordConnectionSample.json");

				RestApiCaller.CallAndHandleError
					(
						() =>
						{
							Console.WriteLine("-> Post UserNamePassword connection");
							return request.Post(RequestObject.Connections, connection);
						},
						() =>
							request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections,
								connection.Name))
					);

			}
		}

		private static void PutUserNamePasswordConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostUserNamePasswordConnectionSample.json");
				var connectionUpdate = Helper.GetJsonObjectFromFile<Connection>("SampleData/PutUserNamePasswordConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise PUT UserNamePassword connection");
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

		private static void GetUserNamePasswordConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostUserNamePasswordConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise Get UserNamePassword connection");
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

		private static void DeleteUserNamePasswordConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostUserNamePasswordConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise DELETE UserNamePassword connection");
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
	}
}
