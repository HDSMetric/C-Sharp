using System.Security.Cryptography;
using System.Text;

namespace APIProject.Helper
{
    public class Crypt
    {
        public static string getMac(string key, string content)
        {
            StringBuilder sb = new StringBuilder(content);
            sb.Append(key);
            return sha256_hash(sb.ToString());
        }

        public static byte[] parseHex(string hex)
        {
            return Convert.FromHexString(hex);
        }
        
        public static string toHex(byte[] byteStr)
        {
            return Convert.ToHexString(byteStr);
        }

        public static byte[] encrypt(string key, string plainText)
        {
            return encryptStringToBytes_Aes(sha256HashBase64(key), plainText);
        }

        public static string decrypt(string key, string encryptedHex)
        {
            return decryptStringFromBytes_Aes(sha256HashBase64(key), parseHex(encryptedHex));
        }

        public static string sha256HashBase64(string value)
        {
            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));
                return Convert.ToBase64String(result);
            }
        }

        public static String sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static byte[] sha256_hash(byte[] value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(value);
                return result;
            }
        }

        private static string decryptStringFromBytes_Aes(string key, byte[] cipherText)
        {
            string plaintext = "";

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.KeySize = 128;
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        private static byte[] encryptStringToBytes_Aes(string key, string plainText)
        {
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.KeySize = 128;
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}
