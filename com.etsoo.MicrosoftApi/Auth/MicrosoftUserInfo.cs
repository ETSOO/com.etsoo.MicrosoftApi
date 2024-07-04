namespace com.etsoo.MicrosoftApi.Auth
{
    /// <summary>
    /// Microsoft auth user info
    /// 微软认证用户信息
    /// </summary>
    public record MicrosoftUserInfo
    {
        /// <summary>
        /// An identifier for the user, unique among all Microsoft accounts and never reused
        /// </summary>
        public required string Sub { get; init; }

        /// <summary>
        /// The user's full name, in a displayable form
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// The user's given name(s) or first name(s)
        /// </summary>
        public string? GivenName { get; init; }

        /// <summary>
        /// The user's surname(s) or last name(s)
        /// </summary>
        public string? FamilyName { get; init; }

        /// <summary>
        /// The URL of the user's profile picture
        /// </summary>
        public string? Picture { get; init; }

        /// <summary>
        /// The user's email address
        /// </summary>
        public required string Email { get; init; }
    }
}
