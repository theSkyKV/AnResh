using System.Security.Cryptography;
using System.Text;

namespace AnResh.HelperFunctions
{
    public class Hash
    {
        public static string StringToHash(string value)
        {
            byte[] tmpSource;
            byte[] tmpHash;

            tmpSource = Encoding.ASCII.GetBytes(value);
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            var result = ByteArrayToString(tmpHash);

            return result;
        }

        private static string ByteArrayToString(byte[] array)
        {
            StringBuilder result = new StringBuilder(array.Length);

            for (var i = 0; i < array.Length; i++)
            {
                result.Append(array[i].ToString("X2"));
            }

            return result.ToString();
        }
    }
}