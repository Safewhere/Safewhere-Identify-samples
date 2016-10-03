using Serilog;

namespace Safewhere.Samples.STS.Common
{
    /// <summary>
    ///     Provides a logger object that all projects can have access to. Keep it simple.
    /// </summary>
    public static class Logging
    {
        public static readonly ILogger Instance =new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();
    }
}
