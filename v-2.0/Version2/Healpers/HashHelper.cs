using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WorkSanse.Healpers
{
    public static class HashHelper
    {
        public static string Sha256(string input)
        {

            if (string.IsNullOrEmpty(input))

                return string.Empty;

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));

                }
                return builder.ToString();
            }
        }
    }
}
