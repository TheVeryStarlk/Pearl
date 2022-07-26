using FluentResults;
using Microsoft.Extensions.Options;
using Pearl.Maui.Models;
using Pearl.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Pearl.Maui.Services;

public sealed class AuthenticationService
{
    private readonly Settings settingsOptions;
    private readonly HttpClient httpClient;

    public AuthenticationService(Settings settingsOptions, HttpClient httpClient)
    {
        this.settingsOptions = settingsOptions;
        this.httpClient = httpClient;
    }

    public async Task<Result<AuthenticateResponse?>> AuthenticateAsync(string userName, string password)
    {
        var body = JsonSerializer.Serialize(new AuthenticateRequest(userName, password));

        var request = await httpClient.PostAsync($"{settingsOptions.Url}/authentication/authenticate",
            new StringContent(body, Encoding.Default, "application/json"));

        var response = await request.Content.ReadAsStringAsync();

        return request.StatusCode is HttpStatusCode.OK
            ? Result.Ok(JsonSerializer.Deserialize<AuthenticateResponse>(response))
            : Result.Fail(JsonSerializer.Deserialize<ErrorResponse>(response)?.Errors);
    }
}