namespace CalculateSHA256Hash
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class Startup
    {
        public static void Main()
        {
            var result = GetSHA256Hash("Hello_World");

            Console.WriteLine(result);
        }

        private static string GetSHA256Hash(string text)
        {
            // encode the text in byte array
            var bytes = Encoding.Unicode.GetBytes(text);

            var sha256 = new SHA256Managed();

            // compute the hash of the string and return byte array
            var hash = sha256.ComputeHash(bytes);
            string hashString = null;

            // concatenate the formatted string to hashed string in hex
            foreach (var x in hash)
            {
                hashString += $"{x:x2}";
            }

            return hashString;
        }
    }
}