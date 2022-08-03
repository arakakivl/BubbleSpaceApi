using System.Security.Cryptography;

namespace BubbleSpaceApi.Application.Common;

public static class PasswordHashing
{
    // Config about our PasswordHash
    private const int saltSize = 16;
    private const int keySize = 32;
    private const int iterations = 20000;
    
    public static string GeneratePasswordHash(string pswd)
    {
        HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
        char segmentDelmiter = ':';

        // Creating salt
        var salt = RandomNumberGenerator.GetBytes(saltSize);
        var key = Rfc2898DeriveBytes.Pbkdf2(pswd, salt, iterations, algorithm, keySize);

        return String.Join(segmentDelmiter, Convert.ToBase64String(key), Convert.ToBase64String(salt), iterations, algorithm);
    }

    public static bool VerifyHashes(string pswd, string hash)
    {
        var segments = hash.Split(':');
        var key = Convert.FromBase64String(segments[0]);
        var salt = Convert.FromBase64String(segments[1]);
        var iterations = int.Parse(segments[2]);
        var algorithm = new HashAlgorithmName(segments[3]);
        var inputSecretKey = Rfc2898DeriveBytes.Pbkdf2(pswd, salt, iterations, algorithm, key.Length);

        return key.SequenceEqual(inputSecretKey);
    }
}