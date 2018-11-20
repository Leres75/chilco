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
        /// [n] = ProcessGroupName
        /// [n+1] = ExePaths, separated by a ','
        /// [n+2] = PlayTime in ms
        /// </summary>
        public static string[] properties;

        public static string[,] ProcessManagerProperties;

        public static void Load()
        {
            properties = System.IO.File.ReadAllLines(SettingsPath);
        }

        public static void LoadProcessManagerProperties()
        {
            int i = 1;
            while (properties[i] != "end Processes")
            {
                i++;
            }
            //Console.WriteLine(i);
            string[,] output = new string[(i-2) / 3, 3];
            //Console.WriteLine("["+ ((i - 2) / 3) + ", " + 3 + "]");
            i = 2;
            while (properties[i] != "end Processes")
            {
                //Console.WriteLine("output[" + ((i - 2) / 3) + ", " + (i % 3) + "] = properties[" + i + "]");
                output[(i - 2) / 3, (i-2) % 3] = properties[i];
                i++;
            }
            ProcessManagerProperties = output;
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
            int length = 2;
            foreach (string s in ProcessManagerProperties) length++;
            string[] output = new string[length + 1];
            output[0] = properties[0];
            output[1] = properties[1];
            output[length] = "end Processes";
            for (int i = 0; i < ProcessManagerProperties.GetLength(0); i++)
            {
                for (int k = 0; k < ProcessManagerProperties.GetLength(1); k++)
                {
                    output[i * ProcessManagerProperties.GetLength(1) + k + 2] = ProcessManagerProperties[i, k];
                }
            }
            System.IO.File.WriteAllLines(SettingsPath, output);
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
