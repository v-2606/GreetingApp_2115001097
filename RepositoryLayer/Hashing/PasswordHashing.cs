using System;
using System.Collections;
using System.Security.Cryptography;

namespace RepositoryLayer.Helper
{
    public class PasswordHashing
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10;

        public static byte[] HashPassword(string password)
        {
            try
            {
                byte[] salt = new byte[SaltSize];
                new RNGCryptoServiceProvider().GetBytes(salt);  

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
                byte[] hash = pbkdf2.GetBytes(HashSize);

                byte[] hashByte = new byte[SaltSize + HashSize];

                Array.Copy(salt, 0, hashByte, 0, SaltSize);
                Array.Copy(hash, 0, hashByte, SaltSize, HashSize);

                byte[] hashedPassword = hashByte;

                return hashByte; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error in hashing password", ex);
            }
        }

        public static bool VerifyPassword(string userPass, byte[] storedHashPass)
        {
            try
            {
                byte[] hashByte = storedHashPass; 
                byte[] salt = new byte[SaltSize];

                Array.Copy(hashByte, 0, salt, 0, SaltSize);

                var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations);
                byte[] hash = pbkdf2.GetBytes(HashSize);

                for (int i = 0; i < HashSize; i++)
                {
                    if (hashByte[i + SaltSize] != hash[i])
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in verifying password", ex);
            }
        }
    }
}

     
