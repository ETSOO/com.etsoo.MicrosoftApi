using com.etsoo.ApiModel.Auth;
using com.etsoo.Utils.Actions;
using Microsoft.AspNetCore.Http;

namespace com.etsoo.MicrosoftApi.Auth
{
    public interface IMicrosoftAuthClient : IAuthClient
    {
        /// <summary>
        /// Create access token from authorization code
        /// 从授权码创建访问令牌
        /// </summary>
        /// <param name="code">Authorization code</param>
        /// <returns>Token data</returns>
        ValueTask<MicrosoftTokenData?> CreateTokenAsync(string code);

        /// <summary>
        /// Get user info
        /// 获取用户信息
        /// </summary>
        /// <param name="tokenData">Token data</param>
        /// <returns>Result</returns>
        ValueTask<MicrosoftUserInfo?> GetUserInfoAsync(MicrosoftTokenData tokenData);

        /// <summary>
        /// Refresh the access token with refresh token
        /// 用刷新令牌获取访问令牌
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>Result</returns>
        Task<MicrosoftRefreshTokenData?> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Validate auth callback
        /// 验证认证回调
        /// </summary>
        /// <param name="request">Callback request</param>
        /// <param name="stateCallback">Callback to verify request state</param>
        /// <returns>Action result & Token data</returns>
        Task<(IActionResult result, MicrosoftTokenData? tokenData)> ValidateAuthAsync(HttpRequest request, Func<string, bool> stateCallback);
    }
}
