using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Bemby.AccountModule.Application.Interfaces.Services;

namespace Bemby.AccountModule.Application.Services
{
    public class PBKDF2PasswordService : IPasswordService
    {
        private const int _hashLength = 32;
        private const int _base64HashLength = ((_hashLength * 4) / 3) + (_hashLength % 3);
        private const int _saltLength = 32;
        private const int _base64IterationsLength = 8;
        
        /// <summary>
        /// Checks that the password is considered strong. Requires at least 1 number 1 special character (_!@#$&*) and at lease 1 upper case character. Also requires a minimum length of 8.
        /// </summary>
        /// <param name="password">Un-hashed password.</param>
        /// <returns></returns>
        public bool IsStrongPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[_!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z]).{8,}$");
        }

        public string HashPassword(string password, string salt = null, int? numberOfIterations = null)
        {
            salt ??= GenerateSalt();

            int iterations = numberOfIterations ?? new Random(DateTime.Now.Millisecond).Next(1000, 10000);
            var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), iterations);

            return Convert.ToBase64String(pbkdf2.GetBytes(_hashLength)) + Convert.ToBase64String(pbkdf2.Salt) + Convert.ToBase64String(BitConverter.GetBytes(iterations));
        }

        public bool CheckPassword(string password, string hashResult)
        {
            string base64Hash = hashResult.Substring(0, _base64HashLength);
            string base64NumberOfIterations = hashResult.Substring(hashResult.Length - _base64IterationsLength, _base64IterationsLength);
            string base64Salt = hashResult.Substring(_base64HashLength, hashResult.Length - _base64HashLength - _base64IterationsLength);

            int iterations = BitConverter.ToInt32(Convert.FromBase64String(base64NumberOfIterations));

            var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(base64Salt), iterations);
            return Convert.ToBase64String(pbkdf2.GetBytes(_hashLength)) == base64Hash;
        }

        public string GenerateSalt(int saltLength = 32)
        {
            byte[] salt = new byte[saltLength];
            using (RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }
    }
}