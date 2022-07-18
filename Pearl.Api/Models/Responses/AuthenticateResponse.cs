namespace Pearl.Api.Models.Responses;

public sealed record AuthenticateResponse(string AccessToken, string RefreshToken);