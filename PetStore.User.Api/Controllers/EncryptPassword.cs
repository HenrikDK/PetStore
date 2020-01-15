using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PetStore.User.Api.Controllers
{
    public interface IEncryptPassword
    {
        (string hash, string salt) Execute(string password);
        (string hash, string salt) Execute(string password, string salt);
    }
    
    public class EncryptPassword : IEncryptPassword
    {
        public (string hash, string salt) Execute(string password)
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Execute(password, salt);
        }

        public (string hash, string salt) Execute(string password, string salt)
        {
            return Execute(password, Convert.FromBase64String(salt));
        }

        private (string hash, string salt) Execute(string password, byte[] salt)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return (hashed, Convert.ToBase64String(salt));
        }
    }
}