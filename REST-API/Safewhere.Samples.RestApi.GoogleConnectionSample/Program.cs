using System;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.GoogleConnectionSample
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("Begin POST Google connection");
			PostGoogleConnection();
			Console.WriteLine("End POST Google connection Sample\n");

			Console.WriteLine("Begin PUT Google connection");
			PutGoogleConnection();
			Console.WriteLine("End PUT Google connection Sample\n");

			Console.WriteLine("Begin GET Google connection");
			GetGoogleConnection();
			Console.WriteLine("End GET Google connection Sample\n");

			Console.WriteLine("Begin DELETE Google connection");
			DeleteGoogleConnection();
			Console.WriteLine("End DELETE Google connection Sample\n");

			Console.WriteLine("All done!");
		}

		private static void PostGoogleConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostGoogleConnectionSample.json");

				RestApiCaller.CallAndHandleError
					(
						() =>
						{
							Console.WriteLine("-> Exercise POST Google connection");
							return request.Post(RequestObject.Connections, connection);
						},
						() =>
							request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections,
								connection.Name))
					);
			}
		}

		private static void PutGoogleConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostGoogleConnectionSample.json");
				var connectionUpdate = Helper.GetJsonObjectFromFile<Connection>("SampleData/PutGoogleConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise PUT Google connection");
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

		private static void GetGoogleConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostGoogleConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise Get Google connection");
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

		private static void DeleteGoogleConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostGoogleConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise DELETE Google connection");
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
