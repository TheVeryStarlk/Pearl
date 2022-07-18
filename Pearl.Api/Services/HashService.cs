using System.Security.Cryptography;
using System.Text;

namespace Pearl.Api.Services;

public sealed class HashService
{
    public (byte[] Hash, byte[] Salt) Generate(string password)
    {
        using var hmac = new HMACSHA256();

        return (hmac.ComputeHash(Encoding.UTF8.GetBytes(password)), hmac.Key);
    }

    public bool Verify(string password, byte[] hash, byte[] salt)
    {
        using var hmac = new HMACSHA256(salt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(hash);
    }
}