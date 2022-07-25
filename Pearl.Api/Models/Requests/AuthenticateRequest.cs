namespace Pearl.Api.Models.Requests;

public sealed record AuthenticateRequest(string UserName, string Password);