namespace Waves.Web.Identity.Endpoints
{
    /// <summary>
    /// Endpoints constants.
    /// </summary>
    public static class WavesBaseEndpoints
    {
        /// <summary>
        /// Gets controller base URL.
        /// </summary>
        public const string ControllerBaseUrl = CurrentApiRoute + "/" + "[controller]";

        /// <summary>
        /// Gets current API route.
        /// </summary>
        public const string CurrentApiRoute = ApiId + "/" + CurrentApiVersion;

        /// <summary>
        /// Gets API id.
        /// </summary>
        private const string ApiId = "api";

        /// <summary>
        /// Gets current API version.
        /// </summary>
        private const string CurrentApiVersion = "v1";
    }
}
