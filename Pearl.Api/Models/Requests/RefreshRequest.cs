namespace Pearl.Api.Models.Requests;

public sealed record RefreshRequest(string AccessToken, string RefreshToken);