using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Arcmatics.Authentication
{
    internal class Cryptography
    {
        string EncryptionKey = "Tpl4NCPadm1n";
        Rfc2898DeriveBytes pdb;
        byte[] salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        /// <summary>
        /// Encrypt readable text.
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns></returns>
        public string EncryptString(string encryptText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptText);
            using (Aes encryptor = Aes.Create())
            {
                pdb = new Rfc2898DeriveBytes(EncryptionKey, salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return encryptText;
        }


        /// <summary>
        /// Decrypt encrypted text to  readable form.
        /// </summary>
        /// <param name="encrytedText"></param>
        /// <returns></returns>
        public string DecryptString(string encrytedText)
        {
            byte[] cipherBytes = Convert.FromBase64String(encrytedText);
            using (Aes encryptor = Aes.Create())
            {
                pdb = new Rfc2898DeriveBytes(EncryptionKey, salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    encrytedText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return encrytedText;
        }
    }
}
