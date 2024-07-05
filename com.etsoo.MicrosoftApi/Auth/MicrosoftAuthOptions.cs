using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

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
        [Url]
        public string? Authority { get; set; }

        /// <summary>
        /// The Application (client) ID that the Microsoft Entra admin center – App registrations experience assigned to your app
        /// </summary>
        [Required]
        public required string ClientId { get; set; }

        /// <summary>
        /// The application secret that you created in the app registration portal for your app
        /// </summary>
        [Required]
        public required string ClientSecret { get; set; }

        /// <summary>
        /// Authorized redirect URIs for the server side application
        /// </summary>
        [Url]
        public string? ServerRedirectUrl { get; set; }

        /// <summary>
        /// Authorized redirect URIs for the script side application
        /// </summary>
        [Url]
        public string? ScriptRedirectUrl { get; set; }
    }

    [OptionsValidator]
    public partial class ValidateMicrosoftAuthOptions : IValidateOptions<MicrosoftAuthOptions>
    {
    }
}
