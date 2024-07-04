namespace com.etsoo.MicrosoftApi.Auth
{
    /// <summary>
    /// Microsoft Refresh Token data
    /// 微软刷新令牌数据
    /// </summary>
    public record MicrosoftRefreshTokenData
    {
        /// <summary>
        /// A token that can be sent to a Microsoft API for access
        /// 令牌可以发送到 Microsoft API 以获取访问权限
        /// </summary>
        public required string AccessToken { get; init; }

        /// <summary>
        /// The token type. Always set to Bearer
        /// 令牌类型。始终设置为 Bearer
        /// </summary>
        public required string TokenType { get; init; }

        /// <summary>
        /// The remaining lifetime of the access token in seconds
        /// 访问令牌的剩余生存时间（以秒为单位）
        /// </summary>
        public required int ExpiresIn { get; init; }

        /// <summary>
        /// The scopes of access granted by the access_token expressed as a list of space-delimited, case-sensitive strings
        /// 用于访问 access_token 授予的访问权限的范围，表示为一组以空格分隔的区分大小写的字符串
        /// </summary>
        public required string Scope { get; init; }
    }
}
