using System;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.FacebookConnectionSample
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("Begin POST Facebook connection");
			PostFacebookConnection();
			Console.WriteLine("End POST Facebook connection Sample\n");

			Console.WriteLine("Begin PUT Facebook connection");
			PutFacebookConnection();
			Console.WriteLine("End PUT Facebook connection Sample\n");

			Console.WriteLine("Begin GET Facebook connection");
			GetFacebookConnection();
			Console.WriteLine("End GET Facebook connection Sample\n");

			Console.WriteLine("Begin DELETE Facebook connection");
			DeleteFacebookConnection();
			Console.WriteLine("End DELETE Facebook connection Sample\n");

			Console.WriteLine("All done!");
		}

		private static void PostFacebookConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostFacebookConnectionSample.json");

				RestApiCaller.CallAndHandleError
					(
						() =>
						{
							Console.WriteLine("-> Exercise POST Facebook connection");
							return request.Post(RequestObject.Connections, connection);
						},
						() =>
							request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections,
								connection.Name))
					);
			}
		}

		private static void PutFacebookConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostFacebookConnectionSample.json");
				var connectionUpdate = Helper.GetJsonObjectFromFile<Connection>("SampleData/PutFacebookConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise PUT Facebook connection");
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

		private static void GetFacebookConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostFacebookConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise Get Facebook connection");
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

		private static void DeleteFacebookConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostFacebookConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise DELETE Facebook connection");
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
