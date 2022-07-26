using System.Text.Json.Serialization;

namespace Pearl.Models;

public sealed record ErrorResponse([property: JsonPropertyName("errors")] string[] Errors);