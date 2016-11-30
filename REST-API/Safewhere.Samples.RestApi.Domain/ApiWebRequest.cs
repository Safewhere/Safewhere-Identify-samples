using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Safewhere.Samples.RestApi.Domain
{
    public class ApiWebRequest : IDisposable
    {
        private HttpClient httpClient = new HttpClient();

        public ApiWebRequest()
        {
            try
            {
                httpClient.BaseAddress = GetBaseApiUri();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", AppSettings.AccessToken);
            }
            catch (Exception exception)
            {
                if (exception is FormatException) throw new FormatException("Please recheck and configure your AccessToken in App.config");
            }
        }

        public HttpResponseMessage Get(string path)
        {
            var response = httpClient.GetAsync(new Uri(path, UriKind.RelativeOrAbsolute)).ContinueWith(task => task.Result);
            return response.Result;
        }

        public HttpResponseMessage Post<T>(string path, T content)
        {
            var response = httpClient.PostAsJsonAsync(path, content).ContinueWith(task => task.Result);
            return response.Result;
        }

        public HttpResponseMessage Delete(string path)
        {
            var response = httpClient.DeleteAsync(new Uri(path, UriKind.RelativeOrAbsolute)).ContinueWith(task => task.Result);

            return response.Result;
        }

        public HttpResponseMessage Delete<T>(string path, T content)
        {
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Delete;
                request.RequestUri = new Uri(path, UriKind.RelativeOrAbsolute);
                request.Content = new ObjectContent<T>(content, new JsonMediaTypeFormatter());

                var response = httpClient.SendAsync(request).ContinueWith(task => task.Result);

                return response.Result;
            }
        }

        public HttpResponseMessage Put<T>(string path, T content)
        {
            var response = httpClient.PutAsJsonAsync(path, content).ContinueWith(task => task.Result);

            return response.Result;
        }

        public HttpResponseMessage Patch<T>(string path, T content)
        {
            using (var request = new HttpRequestMessage())
            {
                request.Method = new HttpMethod("PATCH");
                request.RequestUri = new Uri(path, UriKind.RelativeOrAbsolute);
                request.Content = new ObjectContent<T>(content, new JsonMediaTypeFormatter());

                var response = httpClient.SendAsync(request).ContinueWith(task => task.Result);

                return response.Result;
            }
        }

        private static Uri GetBaseApiUri()
        {
            return new Uri(string.Format(CultureInfo.CurrentCulture, "{0}{1}", AppSettings.Domain, AppSettings.RestApiPath));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && httpClient != null)
            {
                httpClient.Dispose();
                httpClient = null;
            }
        }
    }
}
