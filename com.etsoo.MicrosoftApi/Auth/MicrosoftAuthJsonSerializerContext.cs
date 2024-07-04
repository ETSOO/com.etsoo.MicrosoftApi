using System.Text.Json.Serialization;

namespace com.etsoo.MicrosoftApi.Auth
{
    /// <summary>
    /// Microsoft Auth API JSON serializer context
    /// 微软认证API JSON序列化上下文
    /// </summary>
    [JsonSourceGenerationOptions(
        PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
        DictionaryKeyPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    )]
    [JsonSerializable(typeof(MicrosoftRefreshTokenData))]
    [JsonSerializable(typeof(MicrosoftTokenData))]
    [JsonSerializable(typeof(MicrosoftUserInfo))]
    partial class MicrosoftAuthJsonSerializerContext : JsonSerializerContext
    {
    }
}
