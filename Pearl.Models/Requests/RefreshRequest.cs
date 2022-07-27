using System.Text.Json.Serialization;

namespace Pearl.Models.Requests;

public sealed record RefreshRequest(
    [property: JsonPropertyName("accessToken")] string AccessToken,
    [property: JsonPropertyName("refreshToken")] string RefreshToken);