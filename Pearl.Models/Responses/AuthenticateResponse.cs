using System.Text.Json.Serialization;

namespace Pearl.Models.Responses;

public sealed record AuthenticateResponse(
    [property: JsonPropertyName("accessToken")] string AccessToken,
    [property: JsonPropertyName("refreshToken")] string RefreshToken);