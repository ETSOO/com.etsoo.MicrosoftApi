namespace com.etsoo.MicrosoftApi.Auth
{
    /// <summary>
    /// Microsoft Auth options
    /// 微软认证选项
    /// </summary>
    public record MicrosoftAuthOptions
    {
        /// <summary>
        /// Authority, default is https://login.microsoftonline.com/common
        /// 'common' is the tenant value. Valid values are common, organizations, consumers, and tenant identifiers.
        /// </summary>
        public string? Authority { get; set; }

        /// <summary>
        /// The Application (client) ID that the Microsoft Entra admin center – App registrations experience assigned to your app
        /// </summary>
        public string ClientId { get; set; } = string.Empty!;

        /// <summary>
        /// The application secret that you created in the app registration portal for your app
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty!;

        /// <summary>
        /// Authorized redirect URIs for the server side application
        /// </summary>
        public string? ServerRedirectUrl { get; set; }

        /// <summary>
        /// Authorized redirect URIs for the script side application
        /// </summary>
        public string? ScriptRedirectUrl { get; set; }
    }
}
