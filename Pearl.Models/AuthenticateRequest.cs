namespace Pearl.Models;

public sealed record AuthenticateRequest(string UserName, string Password);