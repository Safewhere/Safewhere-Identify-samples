using System;
using System.Globalization;
using Safewhere.Samples.RestApi.Domain;
using Safewhere.SCIMModel.Connections;

namespace Safewhere.Samples.RestApi.DeviceBasedConnectionSample
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("Begin POST DeviceBased connection");
			PostDeviceBasedConnection();
			Console.WriteLine("End POST DeviceBased connection Sample\n");

			Console.WriteLine("Begin PUT DeviceBased connection");
			PutDeviceBasedConnection();
			Console.WriteLine("End PUT DeviceBased connection Sample\n");

			Console.WriteLine("Begin GET DeviceBased connection");
			GetDeviceBasedConnection();
			Console.WriteLine("End GET DeviceBased connection Sample\n");

			Console.WriteLine("Begin DELETE DeviceBased connection");
			DeleteDeviceBasedConnection();
			Console.WriteLine("End DELETE DeviceBased connection Sample\n");

			Console.WriteLine("All done!");
		}

		private static void PostDeviceBasedConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostDeviceBasedConnectionSample.json");

				RestApiCaller.CallAndHandleError
					(
						() =>
						{
							Console.WriteLine("-> Exercise POST DeviceBased connection");
							return request.Post(RequestObject.Connections, connection);
						},
						() =>
							request.Delete(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", RequestObject.Connections,
								connection.Name))
					);
			}
		}

		private static void PutDeviceBasedConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostDeviceBasedConnectionSample.json");
				var connectionUpdate = Helper.GetJsonObjectFromFile<Connection>("SampleData/PutDeviceBasedConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise PUT DeviceBased connection");
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

		private static void GetDeviceBasedConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostDeviceBasedConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise Get DeviceBased connection");
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

		private static void DeleteDeviceBasedConnection()
		{
			using (var request = new ApiWebRequest())
			{
				var connection = Helper.GetJsonObjectFromFile<Connection>("SampleData/PostDeviceBasedConnectionSample.json");

				RestApiCaller.CallAndHandleError
				   (
					   () =>
					   {
						   Console.WriteLine("-> Create data");
						   request.Post(RequestObject.Connections, connection);

						   Console.WriteLine("-> Exercise DELETE DeviceBased connection");
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
