using FluentResults;
using Pearl.Models;
using Pearl.Models.Requests;
using Pearl.Models.Responses;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Pearl.Maui.Services;

public sealed class AuthenticationService
{
    private readonly HttpClient httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Result<AuthenticateResponse?>> AuthenticateAsync(string userName, string password)
    {
        var body = JsonSerializer.Serialize(new AuthenticateRequest(userName, password));

        var request = await httpClient.PostAsync($"{Preferences.Get("Url", null)}/authentication/authenticate",
            new StringContent(body, Encoding.Default, "application/json"));

        var response = await request.Content.ReadAsStringAsync();

        return request.StatusCode is HttpStatusCode.OK
            ? Result.Ok(JsonSerializer.Deserialize<AuthenticateResponse>(response))
            : Result.Fail(JsonSerializer.Deserialize<ErrorResponse>(response)?.Errors);
    }

    public async Task<Result<string[]?>> GroupsAsync()
    {
        Result<RefreshResponse>? refreshResponse = await RefreshAsync(Preferences.Get("AccessToken", null));

        if (refreshResponse.IsFailed)
        {
            return Result.Fail(refreshResponse.Errors);
        }

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", refreshResponse.Value!.AccessToken);

        var request = await httpClient.GetAsync($"{Preferences.Get("Url", null)}/groups");

        var response = await request.Content.ReadAsStringAsync();

        return request.StatusCode is HttpStatusCode.OK
            ? Result.Ok(JsonSerializer.Deserialize<string[]>(response))
            : Result.Fail(JsonSerializer.Deserialize<ErrorResponse>(response)?.Errors);
    }

    public async Task<Result<Message[]?>> MessagesAsync(string name)
    {
        Result<RefreshResponse>? refreshResponse = await RefreshAsync(Preferences.Get("AccessToken", null));

        if (refreshResponse.IsFailed)
        {
            return Result.Fail(refreshResponse.Errors);
        }

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", refreshResponse.Value!.AccessToken);

        var request = await httpClient.GetAsync($"{Preferences.Get("Url", null)}/groups/messages/{name}");

        var response = await request.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(response))
        {
            return Result.Fail("No messages were found.");
        }

        return request.StatusCode is HttpStatusCode.OK
            ? Result.Ok(JsonSerializer.Deserialize<Message[]>(response))
            : Result.Fail(JsonSerializer.Deserialize<ErrorResponse>(response)?.Errors);
    }

    public async Task<Result<RefreshResponse>> RefreshAsync(string? accessToken)
    {
        var refreshToken = Preferences.Get("RefreshToken", null);

        if (accessToken is null)
        {
            return Result.Fail("Could not find the access token.");
        }

        if (refreshToken is null)
        {
            return Result.Fail("Could not find the refresh token.");
        }

        var body = JsonSerializer.Serialize(new RefreshRequest(accessToken, refreshToken));

        var request = await httpClient.PostAsync($"{Preferences.Get("Url", null)}/authentication/refresh",
            new StringContent(body, Encoding.Default, "application/json"));

        var response = await request.Content.ReadAsStringAsync();

        if (request.StatusCode is HttpStatusCode.OK)
        {
            var serializedResponse = JsonSerializer.Deserialize<RefreshResponse>(response)!;

            Preferences.Set("AccessToken", serializedResponse.AccessToken);
            return Result.Ok(serializedResponse);
        }

        return Result.Fail(JsonSerializer.Deserialize<ErrorResponse>(response)?.Errors);
    }
}