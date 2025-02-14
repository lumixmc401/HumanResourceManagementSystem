using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Core.Share.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8, // 4 cores
                MemorySize = 1024 * 1024, // 1 GB
                Iterations = 4
            };

            var bytes = argon2.GetBytes(16);

            return Convert.ToBase64String(bytes);
        }

        public static byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static bool VerifyPassword(string password, string hashPassword, byte[] salt)
        {
            var newHashPassword = HashPassword(password, salt);
            return newHashPassword == hashPassword;
        }
    }
}
