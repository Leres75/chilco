using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace chilco
{
    class Settings
    {
        private string SettingsPath = @".properties";
        private string[] properties;                    // @[0] is the Hash of the Password
                                                        //
        public void Load()
        {
            properties = System.IO.File.ReadAllLines(SettingsPath);
        }

        public bool CheckPassword(string password)
        {
            string inputedHash = GetSha256Hash(SHA256.Create(), password);
            string savedHash = properties[0];
            if (inputedHash.Equals(savedHash)) return true;
            else return false;
        }

        public void ChangePassword(string NewPassword)
        {
            string inputedHash = GetSha256Hash(SHA256.Create(), NewPassword);
            properties[0] = inputedHash;
            SaveProperties();
        }

        public void SaveProperties()
        {
            System.IO.File.WriteAllLines(SettingsPath, properties);
        }

        static string GetSha256Hash(SHA256 shaHash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
