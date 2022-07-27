using System.Text.Json.Serialization;

namespace Pearl.Models;

public sealed record Message(
    [property: JsonPropertyName("userName")] string UserName,
    [property: JsonPropertyName("content")] string Content);