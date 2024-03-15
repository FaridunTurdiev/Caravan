using System.Security.Cryptography;

namespace CaravanApi.Services;
internal class PasswordHasher
{
    /// <summary> To hash passwords before saving them to the DB </summary>
    public static void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using(var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    /// <summary> To verify if the password is right or not</summary>
    public static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt) 
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

    }
}
