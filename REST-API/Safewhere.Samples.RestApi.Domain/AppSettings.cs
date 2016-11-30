using System.Configuration;

namespace Safewhere.Samples.RestApi.Domain
{
	public static class AppSettings
	{
        public static string Domain
        {
            get
            {
                return ConfigurationManager.AppSettings["Domain"];
            }
        }

        public static string RestApiPath
        {
            get
            {
                return ConfigurationManager.AppSettings["RestApiPath"];
            }
        }

        public static string AccessToken
        {
            get
            {
                return ConfigurationManager.AppSettings["AccessToken"];
            }
        }
	}
}
