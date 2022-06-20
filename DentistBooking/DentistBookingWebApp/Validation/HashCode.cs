using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace DentistBookingWebApp.Validation
{
    public static class HashCode
    {
        public static String HashPassword(String password)
        {
            byte[] salt = new byte[128 / 8];
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
