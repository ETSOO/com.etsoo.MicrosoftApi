﻿using com.etsoo.ApiModel.Auth;
using com.etsoo.Utils.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;

namespace com.etsoo.MicrosoftApi.Auth
{
    /// <summary>
    /// Microsoft Auth client
    /// 微软认证客户端
    /// </summary>
    public class MicrosoftAuthClient : IMicrosoftAuthClient
    {
        private readonly HttpClient _client;
        private readonly MicrosoftAuthOptions _options;
        private readonly ILogger _logger;

        private readonly string _authority;

        public MicrosoftAuthClient(HttpClient client, MicrosoftAuthOptions options, ILogger logger)
        {
            _client = client;
            _options = options;
            _logger = logger;

            _authority = options.Authority ?? "https://login.microsoftonline.com/common";
        }

        [ActivatorUtilitiesConstructor]
        public MicrosoftAuthClient(HttpClient client, IOptions<MicrosoftAuthOptions> options, ILogger<MicrosoftAuthClient> logger)
            : this(client, options.Value, logger)
        {

        }

        /// <summary>
        /// Get server auth URL, for back-end processing
        /// 获取服务器授权URL，用于后端处理
        /// </summary>
        /// <param name="state">Specifies any string value that your application uses to maintain state between your authorization request and the authorization server's response</param>
        /// <param name="scope">A space-delimited list of scopes that identify the resources that your application could access on the user's behalf</param>
        /// <param name="offline">Set to true if your application needs to refresh access tokens when the user is not present at the browser</param>
        /// <param name="loginHint">Set the parameter value to an email address or sub identifier, which is equivalent to the user's Microsoft ID</param>
        /// <returns>URL</returns>
        public string GetServerAuthUrl(string state, string scope, bool offline = false, string? loginHint = null)
        {
            if (offline) scope += " offline_access";

            return GetAuthUrl(_options.ServerRedirectUrl, "code", scope, state, loginHint);
        }

        /// <summary>
        /// Get script auth URL, for front-end page
        /// 获取脚本授权URL，用于前端页面
        /// </summary>
        /// <param name="state">Specifies any string value that your application uses to maintain state between your authorization request and the authorization server's response</param>
        /// <param name="scope">A space-delimited list of scopes that identify the resources that your application could access on the user's behalf</param>
        /// <param name="loginHint">Set the parameter value to an email address or sub identifier, which is equivalent to the user's Microsoft ID</param>
        /// <returns>URL</returns>
        public string GetScriptAuthUrl(string state, string scope, string? loginHint = null)
        {
            return GetAuthUrl(_options.ScriptRedirectUrl, "token", scope, state, loginHint);
        }

        /// <summary>
        /// Get auth URL
        /// 获取授权URL
        /// </summary>
        /// <param name="redirectUrl">The value must exactly match one of the authorized redirect URIs for the OAuth 2.0 client, which you configured in your client's API Console</param>
        /// <param name="responseType">Set the parameter value to 'code' for web server applications or 'token' for SPA</param>
        /// <param name="scope">A space-delimited list of scopes that identify the resources that your application could access on the user's behalf</param>
        /// <param name="state">Specifies any string value that your application uses to maintain state between your authorization request and the authorization server's response</param>
        /// <param name="loginHint">Set the parameter value to an email address or sub identifier, which is equivalent to the user's Microsoft ID</param>
        /// <returns>URL</returns>
        /// <exception cref="ArgumentNullException">Parameter 'redirectUrl' is required</exception>
        public string GetAuthUrl(string? redirectUrl, string responseType, string scope, string state, string? loginHint = null)
        {
            if (string.IsNullOrEmpty(redirectUrl))
            {
                throw new ArgumentNullException(nameof(redirectUrl));
            }

            // https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-auth-code-flow
            return $"{_authority}/oauth2/v2.0/authorize?scope={HttpUtility.UrlEncode(scope)}&response_type={responseType}&state={HttpUtility.UrlEncode(state)}&redirect_uri={HttpUtility.UrlEncode(redirectUrl)}&response_mode=query&client_id={_options.ClientId}&login_hint={loginHint}";
        }

        /// <summary>
        /// Get auth URL
        /// 获取授权URL
        /// </summary>
        /// <param name="redirectUrl">The value must exactly match one of the authorized redirect URIs for the OAuth 2.0 client, which you configured in your client's API Console</param>
        /// <param name="responseType">Set the parameter value to 'code' for web server applications or 'token' for SPA</param>
        /// <param name="scope">A space-delimited list of scopes that identify the resources that your application could access on the user's behalf</param>
        /// <param name="state">Specifies any string value that your application uses to maintain state between your authorization request and the authorization server's response</param>
        /// <param name="loginHint">Set the parameter value to an email address or sub identifier, which is equivalent to the user's Microsoft ID</param>
        /// <returns>URL</returns>
        /// <exception cref="ArgumentNullException">Parameter 'redirectUrl' is required</exception>
        public async ValueTask<MicrosoftTokenData?> CreateTokenAsync(string code)
        {
            if (string.IsNullOrEmpty(_options.ServerRedirectUrl))
            {
                throw new Exception("ServerRedirectUrl is required for server side authentication");
            }

            var api = $"{_authority}/oauth2/v2.0/token";
            var response = await _client.PostAsync(api, new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["redirect_uri"] = _options.ServerRedirectUrl,
                ["grant_type"] = "authorization_code"
            }));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync(MicrosoftAuthJsonSerializerContext.Default.MicrosoftTokenData);
        }

        /// <summary>
        /// Refresh the access token with refresh token
        /// 用刷新令牌获取访问令牌
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>Result</returns>
        public async Task<MicrosoftRefreshTokenData?> RefreshTokenAsync(string refreshToken)
        {
            var api = $"{_authority}/oauth2/v2.0/token";
            var response = await _client.PostAsync(api, new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["refresh_token"] = refreshToken,
                ["grant_type"] = "refresh_token"
            }));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync(MicrosoftAuthJsonSerializerContext.Default.MicrosoftRefreshTokenData);
        }

        /// <summary>
        /// Get user info
        /// 获取用户信息
        /// </summary>
        /// <param name="tokenData">Token data</param>
        /// <returns>Result</returns>
        public async ValueTask<MicrosoftUserInfo?> GetUserInfoAsync(MicrosoftTokenData tokenData)
        {
            // Not like Google, currently the Id Token does not include all user information

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData.TokenType, tokenData.AccessToken);

            var response = await _client.GetAsync($"https://graph.microsoft.com/oidc/userinfo");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync(MicrosoftAuthJsonSerializerContext.Default.MicrosoftUserInfo);
        }

        /// <summary>
        /// Get user info from callback request
        /// 从回调请求获取用户信息
        /// </summary>
        /// <param name="request">Callback request</param>
        /// <param name="state">Request state</param>
        /// <returns>Action result & user information</returns>
        public ValueTask<(IActionResult result, AuthUserInfo? userInfo)> GetUserInfoAsync(HttpRequest request, string state)
        {
            return GetUserInfoAsync(request, s => s == state);
        }

        /// <summary>
        /// Get user info from callback request
        /// 从回调请求获取用户信息
        /// </summary>
        /// <param name="request">Callback request</param>
        /// <param name="stateCallback">Callback to verify request state</param>
        /// <returns>Action result & user information</returns>
        public async ValueTask<(IActionResult result, AuthUserInfo? userInfo)> GetUserInfoAsync(HttpRequest request, Func<string, bool> stateCallback)
        {
            var (result, tokenData) = await ValidateAuthAsync(request, stateCallback);
            AuthUserInfo? userInfo = null;
            if (result.Ok && tokenData != null)
            {
                var data = await GetUserInfoAsync(tokenData);
                if (data == null)
                {
                    result = new ActionResult
                    {
                        Type = "NoDataReturned",
                        Field = "userinfo"
                    };
                }
                else
                {
                    userInfo = new AuthUserInfo
                    {
                        OpenId = data.Sub,
                        Name = data.Name,
                        GivenName = data.GivenName,
                        FamilyName = data.FamilyName,
                        Picture = data.Picture,
                        Email = data.Email,
                        EmailVerified = true
                    };
                }
            }

            return (result, userInfo);
        }

        /// <summary>
        /// Validate auth callback
        /// 验证认证回调
        /// </summary>
        /// <param name="request">Callback request</param>
        /// <param name="stateCallback">Callback to verify request state</param>
        /// <returns>Action result & Token data</returns>
        public async Task<(IActionResult result, MicrosoftTokenData? tokenData)> ValidateAuthAsync(HttpRequest request, Func<string, bool> stateCallback)
        {
            IActionResult result;
            MicrosoftTokenData? tokenData = null;

            if (request.Query.TryGetValue("error", out var error))
            {
                result = new ActionResult
                {
                    Type = "AccessDenied",
                    Field = error
                };
            }
            else if (request.Query.TryGetValue("state", out var actualState) && request.Query.TryGetValue("code", out var codeSource))
            {
                var code = codeSource.ToString();
                if (!stateCallback(actualState.ToString()))
                {
                    result = new ActionResult
                    {
                        Type = "AccessDenied",
                        Field = "state"
                    };
                }
                else if (string.IsNullOrEmpty(code))
                {
                    result = new ActionResult
                    {
                        Type = "NoDataReturned",
                        Field = "code"
                    };
                }
                else
                {
                    try
                    {
                        tokenData = await CreateTokenAsync(code);
                        if (tokenData == null)
                        {
                            result = new ActionResult
                            {
                                Type = "NoDataReturned",
                                Field = "token"
                            };
                        }
                        else
                        {
                            result = ActionResult.Success;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Create token failed");
                        result = ActionResult.From(ex);
                    }
                }
            }
            else
            {
                result = new ActionResult
                {
                    Type = "NoDataReturned",
                    Field = "state"
                };
            }

            return (result, tokenData);
        }
    }
}
