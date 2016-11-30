using System;
using System.Net.Http;

namespace Safewhere.Samples.RestApi.Domain
{
    public static class RestApiCaller
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This is a demonstration application and we want to handle and show all errors to users.")]
        public static void CallAndHandleError(Func<HttpResponseMessage> doAction, Action cleanupAction)
        {
            if (doAction == null)
                throw new ArgumentNullException("doAction");
            if (cleanupAction == null)
                throw new ArgumentNullException("cleanupAction");

            try
            {
                var response = doAction();
                Console.WriteLine("-> Got response:");
                Console.WriteLine("StatusCode {0}, Content: {1}", response.StatusCode, Helper.ReadResponseContentAsString(response));
            }
            catch (Exception ex)
            {
                Console.WriteLine("-> Error Happened:");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.WriteLine("-> Clean up");
                cleanupAction();
            }
        }
    }
}
