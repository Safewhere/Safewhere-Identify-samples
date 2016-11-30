using System;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.OAuth20ConnectionSample
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("Begin POST OAuth20 connection");
			PostOAuth20Connection();
			Console.WriteLine("End POST OAuth20 connection Sample\n");

			Console.WriteLine("Begin PUT OAuth20 connection");
			PutOAuth20Connection();
			Console.WriteLine("End PUT OAuth20 connection Sample\n");

			Console.WriteLine("Begin GET OAuth20 connection");
			GetOAuth20Connection();
			Console.WriteLine("End GET OAuth20 connection Sample\n");

			Console.WriteLine("Begin DELETE OAuth20 connection");
			DeleteOAuth20Connection();
			Console.WriteLine("End DELETE OAuth20 connection Sample\n");

			Console.WriteLine("All done!");
		}

		private static void PostOAuth20Connection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostOAuth20ConnectionSample.json");

				RestApiCaller.CallAndHandleError
					(
						() =>
						{
							Console.WriteLine("-> Exercise POST OAuth20 connection");
							return request.Post(RequestObject.Connections, connection);
						},
						() =>
							request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections,
								connection.Name))
					);
			}
		}

		private static void PutOAuth20Connection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostOAuth20ConnectionSample.json");
				var connectionUpdate = Helper.GetJsonObjectFromFile<Connection>("SampleData/PutOAuth20ConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise PUT OAuth20 connection");
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

		private static void GetOAuth20Connection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostOAuth20ConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise Get OAuth20 connection");
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

		private static void DeleteOAuth20Connection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostOAuth20ConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise DELETE OAuth20 connection");
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
