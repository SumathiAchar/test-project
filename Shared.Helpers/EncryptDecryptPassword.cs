using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SSI.ContractManagement.Shared.Helpers
{
    public static class EncryptDecryptPassword
    {
        /// <summary>
        /// Encrypt Input String
        /// </summary>
        /// <param name="bytesToBeEncrypted"></param>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes;
            byte[] saltBytes = { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = Constants.AesKeySize;
                    aes.BlockSize = Constants.AesBlockSize;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }

        /// <summary>
        /// Decrypt Input String
        /// </summary>
        /// <param name="bytesToBeDecrypted"></param>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes;
            byte[] saltBytes = { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = Constants.AesKeySize;
                    aes.BlockSize = Constants.AesBlockSize;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
            return decryptedBytes;
        }

        /// <summary>
        /// Encrypt Input String
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string EncryptText(string inputString)
        {
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(inputString);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Constants.PasswordBytes);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);
            string result = Convert.ToBase64String(bytesEncrypted);
            return result;
        }

        /// <summary>
        /// Decrypt Input String
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string DecryptText(string inputString)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(inputString);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Constants.PasswordBytes);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);
            string result = Encoding.UTF8.GetString(bytesDecrypted);
            return result;
        }

        /// <summary>
        /// Generate Random Salt
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt()
        {
            var random = new RNGCryptoServiceProvider();
            byte[] salt = new byte[Constants.SaltMaxLength];
            random.GetNonZeroBytes(salt);
            return Convert.ToBase64String(salt);
        }
    }
}
