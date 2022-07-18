using Microsoft.Extensions.Options;
using Pearl.Api.Options;
using Pearl.Database;
using Pearl.Database.Models;

namespace Pearl.Api.Services;

public sealed class RefreshTokenService
{
    private readonly PearlContext pearlContext;
    private readonly IOptions<SecretsOptions> secretsOptions;

    public RefreshTokenService(PearlContext pearlContext, IOptions<SecretsOptions> secretsOptions)
    {
        this.pearlContext = pearlContext;
        this.secretsOptions = secretsOptions;
    }

    public async Task<string> GenerateAsync(string username)
    {
        var token = Guid.NewGuid().ToString();

        pearlContext.RefreshTokens.Add(new RefreshToken()
        {
            User = pearlContext.Users.First(user => user.Name == username),
            Value = token,
            ExpiryDate = DateTime.UtcNow.Add(secretsOptions.Value.RefreshTokenLifetime)
        });

        await pearlContext.SaveChangesAsync();
        return token;
    }
}