using System;
using System.Security.Cryptography;
using System.Text;

namespace chilco
{
    internal class Settings
    {
        private static string SettingsPath = @".properties";

        /// <summary>
        /// @[0] is the Hash of the Password
        /// @[1] is the Path for the ProcessManager Log files
        /// From [2] to [x]="end Processes" -- ProcessInformation
        /// [n] = ExePaths, separated by a ','
        /// [n+1] = PlayTime
        /// </summary>
        private static string[] properties;

        public static string[,] ProcessManagerProperties;

        public static void Load()
        {
            properties = System.IO.File.ReadAllLines(SettingsPath);
        }

        public static void LoadProcessManagerProperties()
        {
            int i = 2;
            while (properties[i] != "end Processes")
            {
                i++;
            }
            string[,] output = new string[i / 2, 2];
            i = 2;
            while (properties[i] != "end Processes")
            {
                output[(i - 2) / 2, i % 2] = properties[i];
                i++;
            }
        }

        public static string GetLogPath()
        {
            return properties[1];
        }

        public static void SetLogPath(String LogPath)
        {
            properties[1] = LogPath;
            SaveProperties();
        }
        public static bool CheckPassword(string password)
        {
            string inputedHash = GetSha256Hash(SHA256.Create(), password);
            string savedHash = properties[0];
            if (inputedHash.Equals(savedHash)) return true;
            else return false;
        }

        public static void ChangePassword(string NewPassword)
        {
            string inputedHash = GetSha256Hash(SHA256.Create(), NewPassword);
            properties[0] = inputedHash;
            SaveProperties();
        }

        public static void SaveProperties()
        {
            System.IO.File.WriteAllLines(SettingsPath, properties);
        }

        public static string GetSha256Hash(SHA256 shaHash, string input)
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
