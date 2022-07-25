using FluentResults;
using Microsoft.EntityFrameworkCore;
using Pearl.Api.Models.Requests;
using Pearl.Api.Models.Responses;
using Pearl.Database;
using Pearl.Database.Models;

namespace Pearl.Api.Services;

public sealed class AuthenticationService
{
    private readonly PearlContext pearlContext;
    private readonly HashService hashService;
    private readonly AccessTokenService accessTokenService;
    private readonly RefreshTokenService refreshTokenService;

    public AuthenticationService(PearlContext pearlContext, HashService hashService,
        AccessTokenService accessTokenService,
        RefreshTokenService refreshTokenService)
    {
        this.pearlContext = pearlContext;
        this.hashService = hashService;
        this.accessTokenService = accessTokenService;
        this.refreshTokenService = refreshTokenService;
    }

    public async Task<Result<AuthenticateResponse>> AuthenticateAsync(AuthenticateRequest request)
    {
        var user = pearlContext.Users.FirstOrDefault(user => user.Name == request.UserName);

        if (user is not null)
        {
            var response = new AuthenticateResponse(
                accessTokenService.Generate(request.UserName),
                await refreshTokenService.GenerateAsync(request.UserName));

            return hashService.Verify(request.Password, user.Hash, user.Salt)
                ? Result.Ok(response)
                : Result.Fail("The provided authentication credentials were incorrect.");
        }

        var password = hashService.Generate(request.Password);

        pearlContext.Users.Add(new User()
        {
            Name = request.UserName,
            Hash = password.Hash,
            Salt = password.Salt
        });

        await pearlContext.SaveChangesAsync();

        return Result.Ok(new AuthenticateResponse(
            accessTokenService.Generate(request.UserName),
            await refreshTokenService.GenerateAsync(request.UserName)));
    }

    public async Task<Result<RefreshResponse>> RefreshAsync(RefreshRequest request)
    {
        var refreshToken = pearlContext.RefreshTokens.Include(path => path.User)
            .FirstOrDefault(refreshToken => refreshToken.Value == request.RefreshToken);

        if (refreshToken is null)
        {
            return Result.Fail("The provided refresh token could not be found.");
        }

        var userName = refreshToken.User.Name;

        var accessTokenResult = accessTokenService.Verify(request.AccessToken);

        if (accessTokenResult.IsSuccess && accessTokenResult.Value == userName)
        {
            if (DateTime.UtcNow > refreshToken.ExpiryDate)
            {
                return Result.Fail("The provided refresh token has expired.");
            }

            return Result.Ok(new RefreshResponse(accessTokenService.Generate(userName)));
        }

        return Result.Fail(accessTokenResult.Errors);
    }
}