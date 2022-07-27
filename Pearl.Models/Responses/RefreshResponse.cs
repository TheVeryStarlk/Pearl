using System.Text.Json.Serialization;

namespace Pearl.Models.Responses;

public sealed record RefreshResponse([property: JsonPropertyName("accessToken")] string AccessToken);