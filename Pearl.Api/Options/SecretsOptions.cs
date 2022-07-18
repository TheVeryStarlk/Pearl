namespace Pearl.Api.Options;

public sealed class SecretsOptions
{
    public static string Secrets => nameof(Secrets);

    /// <summary>
    ///     The access token secret key.
    /// </summary>
    public string Key { get; set; } = null!;

    public TimeSpan AccessTokenLifetime { get; set; }

    public TimeSpan RefreshTokenLifetime { get; set; }
}