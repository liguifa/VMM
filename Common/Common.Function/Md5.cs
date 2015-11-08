namespace Common.Function
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public class Md5
    {
        private const string salt = "81fbaa81762885ac3481fd4b416485e6";

        public static string GetEncryptionValue(string value)
        {
            MD5CryptoServiceProvider m = new MD5CryptoServiceProvider();
            byte[] MD5Source = System.Text.Encoding.UTF8.GetBytes(value+salt);
            byte[] MD5Out = m.ComputeHash(MD5Source);
            return Convert.ToBase64String(MD5Out);
        }
    }
}