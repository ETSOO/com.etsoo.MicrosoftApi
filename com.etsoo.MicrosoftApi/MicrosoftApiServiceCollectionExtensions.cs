using com.etsoo.MicrosoftApi.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace com.etsoo.MicrosoftApi
{
    /// <summary>
    /// Microsoft API service collection extensions
    /// 微软API服务集合扩展
    /// </summary>
    public static class MicrosoftApiServiceCollectionExtensions
    {
        /// <summary>
        /// Add Microsoft auth client
        /// 添加微软授权客户端
        /// </summary>
        /// <param name="services">Services</param>
        /// <param name="configuration">configuration</param>
        /// <returns>Services</returns>
        public static IServiceCollection AddMicrosoftAuthClient(this IServiceCollection services, IConfigurationSection configuration)
        {
            services.AddSingleton<IValidateOptions<MicrosoftAuthOptions>, ValidateMicrosoftAuthOptions>();
            services.AddOptions<MicrosoftAuthOptions>().Bind(configuration).ValidateOnStart();
            services.AddHttpClient<IMicrosoftAuthClient, MicrosoftAuthClient>();
            return services;
        }
    }
}
