namespace Pearl.Models.Requests;

public sealed record AuthenticateRequest(string UserName, string Password);