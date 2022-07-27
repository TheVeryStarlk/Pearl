using System.Text.Json.Serialization;

namespace Pearl.Models.Responses;

public sealed record ErrorResponse([property: JsonPropertyName("errors")] string[] Errors);