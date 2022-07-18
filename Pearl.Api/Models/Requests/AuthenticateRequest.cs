namespace Pearl.Api.Models.Requests;

public sealed record AuthenticateRequest(string Username, string Password);