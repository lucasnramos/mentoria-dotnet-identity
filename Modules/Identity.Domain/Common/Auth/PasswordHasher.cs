using System.Security.Cryptography;

public sealed class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;

    public static string Hash(string password, int iterations)
    {
        byte[] salt;
        using (RNGCryptoServiceProvider rngCsp = new())
        {
            rngCsp.GetBytes(salt = new byte[SaltSize]);
        }

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(HashSize);
        var hashBytes = new byte[SaltSize + HashSize];

        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        var base64Hash = Convert.ToBase64String(hashBytes);

        return string.Format("$PWDHASH$V1${0}${1}", iterations, base64Hash);
    }

    public static string Hash(string password)
    {
        return Hash(password, 10000);
    }

    public static bool IsHashSupported(string hashString)
    {
        return hashString.Contains("$PWDHASH$V1$");
    }

    public static bool Verify(string password, string hashedPassword)
    {
        if (!IsHashSupported(hashedPassword))
        {
            Console.WriteLine($"[Diagnostics] Hash not supported: {hashedPassword}");
            throw new NotSupportedException("The hashtype is not supported");
        }

        var splittedHashString = hashedPassword.Replace("$PWDHASH$V1$", "").Split('$');
        var iterations = int.Parse(splittedHashString[0]);
        var base64Hash = splittedHashString[1];

        var hashBytes = Convert.FromBase64String(base64Hash);
        Console.WriteLine($"[Diagnostics] hashBytes (Base64): {base64Hash}");
        Console.WriteLine($"[Diagnostics] hashBytes (Hex): {BitConverter.ToString(hashBytes)}");

        var salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);
        Console.WriteLine($"[Diagnostics] salt (Hex): {BitConverter.ToString(salt)}");

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(HashSize);
        Console.WriteLine($"[Diagnostics] hash (Hex): {BitConverter.ToString(hash)}");

        bool match = true;
        for (var i = 0; i < HashSize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                Console.WriteLine($"[Diagnostics] Mismatch at byte {i}: stored={hashBytes[i + SaltSize]}, computed={hash[i]}");
                match = false;
            }
        }
        Console.WriteLine($"[Diagnostics] Password match: {match}");
        return match;
    }

}