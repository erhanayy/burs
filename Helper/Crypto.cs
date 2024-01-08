using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace MyAdmin.Helper
{
    public class Crypto
    {
        public static string HashPassword(string plainPassword)
        {
            
            using(SHA512 sha512 = SHA512.Create())
            {
                var bytesPassword = Encoding.UTF8.GetBytes(plainPassword);

                var hashedBytes = sha512.ComputeHash(bytesPassword);

                var stringBuilder = new StringBuilder();
                foreach(byte b in  hashedBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString().Substring(0, 20);
            }
            
        }
    }
}