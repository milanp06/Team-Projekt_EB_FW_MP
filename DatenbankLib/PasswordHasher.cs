using System.Security.Cryptography;
using System.Text;

namespace DatenbankLib
{
    internal static class PasswordHasher
    {
        internal static string HashPassword(string password)
        {
            // 16 byte Salt generieren
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // Hash aus Passwort + Salt erzeugen
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            // Salt + Hash Base64 speichern
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        internal static bool VerifyPassword(string password, string stored)
        {
            var parts = stored.Split(':');
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] storedHash = Convert.FromBase64String(parts[1]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] testHash = pbkdf2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(storedHash, testHash);
        }
    }
}
