using System.Security.Cryptography;
using System.Text;

namespace chilco
{
    internal class Settings
    {
        private string LogPath
        {
            get { return properties[1]; }
            set
            {
                this.LogPath = value;
                SaveProperties();
            }
        }
        private readonly string SettingsPath = @".properties";
        private string[] properties = new string[2];     // @[0] is the Hash of the Password
                                                         // @[1] is the Path for the ProcessManager Log files

        public void Load()
        {
            properties = System.IO.File.ReadAllLines(SettingsPath);
        }

        public bool CheckPassword(string password)
        {
            string inputedHash = GetSha256Hash(SHA256.Create(), password);
            string savedHash = properties[0];
            return inputedHash.Equals(savedHash);// || true xD
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

        private static string GetSha256Hash(SHA256 shaHash, string input)
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