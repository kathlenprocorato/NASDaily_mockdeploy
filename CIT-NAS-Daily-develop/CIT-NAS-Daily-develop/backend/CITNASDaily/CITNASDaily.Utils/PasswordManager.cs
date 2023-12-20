namespace CITNASDaily.Utils
{
    public static class PasswordManager
    {
        /// <summary>
        /// Hash a string <paramref name="password"/> using BCrypt
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Hashed <paramref name="password"/></returns>
        public static string HashPassword(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);

        /// <summary>
        /// Verify a <paramref name="password"/> against a <paramref name="passwordHash"/>
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <returns>True if verified, false otherwise</returns>
        public static bool VerifyPassword(string password, string passwordHash)
            => BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
