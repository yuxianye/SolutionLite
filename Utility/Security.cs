using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    /// <summary>
    /// 加密安全
    /// </summary>
    public static class Security
    {
        #region MD5加密，不可逆

        /// <summary>
        /// MD5加密，不可逆
        /// </summary>
        /// <param name="normalTxt"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string normalTxt)
        {
            var bytes = Encoding.Unicode.GetBytes(normalTxt);//求Byte[]数组
            var Md5 = new MD5CryptoServiceProvider().ComputeHash(bytes);//求哈希值
            return Convert.ToBase64String(Md5);//将Byte[]数组转为净荷明文(其实就是字符串)
        }

        #endregion

        #region RSA加解密，可逆

        /// <summary>
        /// RSA加密，可逆
        /// </summary>
        /// <param name="normaltxt"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string normaltxt)
        {
            try
            {
                byte[] dataToEncrypt = Encoding.Unicode.GetBytes(normaltxt);
                byte[] encryptedData;
                //using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                //{
                encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);
                //}
                return Convert.ToBase64String(encryptedData);

            }
            catch (Exception)
            {
                return null;
            }
        }

        private static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        /// <summary>
        /// RSA解密，可逆
        /// </summary>
        /// <param name="securityTxt"></param>
        /// <returns></returns>
        public static string RSADecrypt(string securityTxt)
        {
            try
            {
                byte[] dataToDecrypt = Convert.FromBase64String(securityTxt);
                byte[] decryptedData;
                //using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                //{
                decryptedData = RSADecrypt(dataToDecrypt, RSA.ExportParameters(true), false);
                //}
                return Encoding.Unicode.GetString(decryptedData);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (Exception)
            {
                return null;
            }

        }

        private static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region SHA加密，不可逆

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="normalTxt"></param>
        /// <returns></returns>
        public static string SHA1Encrypt(string normalTxt)
        {
            var bytes = Encoding.Unicode.GetBytes(normalTxt);
            var SHA = new SHA1CryptoServiceProvider();
            var encryptbytes = SHA.ComputeHash(bytes);
            return Convert.ToBase64String(encryptbytes);
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="normalTxt"></param>
        /// <returns></returns>
        public static string SHA256Encrypt(string normalTxt)
        {
            var bytes = Encoding.Unicode.GetBytes(normalTxt);
            var SHA256 = new SHA256CryptoServiceProvider();
            var encryptbytes = SHA256.ComputeHash(bytes);
            return Convert.ToBase64String(encryptbytes);
        }

        /// <summary>
        /// SHA384加密
        /// </summary>
        /// <param name="normalTxt"></param>
        /// <returns></returns>
        public static string SHA384Encrypt(string normalTxt)
        {
            var bytes = Encoding.Unicode.GetBytes(normalTxt);
            var SHA384 = new SHA384CryptoServiceProvider();
            var encryptbytes = SHA384.ComputeHash(bytes);
            return Convert.ToBase64String(encryptbytes);
        }

        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="normalTxt"></param>
        /// <returns></returns>
        public static string SHA512Encrypt(string normalTxt)
        {
            var bytes = Encoding.Unicode.GetBytes(normalTxt);
            var SHA512 = new SHA512CryptoServiceProvider();
            var encryptbytes = SHA512.ComputeHash(bytes);
            return Convert.ToBase64String(encryptbytes);
        }

        #endregion

        /// <summary>
        /// 加密向量
        /// </summary>
        private static readonly byte[] iv = new byte[] { 0x32, 0x72, 0x30, 0x58, 0x25, 0x59, 0x10, 0x06 };

        #region DES加解密，可逆

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="normalTxt"></param>
        /// <param name="EncryptKey"></param>
        /// <returns></returns>
        public static string DesEncrypt(string normalTxt, string encryptKey = "YjEi0vU8")
        {
            var bytes = Encoding.Unicode.GetBytes(normalTxt);
            var key = Encoding.Unicode.GetBytes(encryptKey.PadLeft(4, '0').Substring(0, 4));
            using (MemoryStream ms = new MemoryStream())
            {
                var encry = new DESCryptoServiceProvider();
                CryptoStream cs = new CryptoStream(ms, encry.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="securityTxt"></param>
        /// <param name="EncryptKey"></param>
        /// <returns></returns>
        public static string DesDecrypt(string securityTxt, string encryptKey = "YjEi0vU8")
        {
            try
            {
                var bytes = Convert.FromBase64String(securityTxt);
                var key = Encoding.Unicode.GetBytes(encryptKey.PadLeft(4, '0').Substring(0, 4));
                using (MemoryStream ms = new MemoryStream())
                {
                    var descrypt = new DESCryptoServiceProvider();
                    CryptoStream cs = new CryptoStream(ms, descrypt.CreateDecryptor(key, iv), CryptoStreamMode.Write);
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    return Encoding.Unicode.GetString(ms.ToArray());
                }

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion

        #region AES加解密，可逆

        ///// <summary>
        ///// 加密向量
        ///// </summary>
        private static readonly byte[] aESiv = new byte[] { 0x32, 0x72, 0x30, 0x58, 0x25, 0x59, 0x10, 0x06, 0x32, 0x72, 0x30, 0x58, 0x25, 0x59, 0x10, 0x06 };


        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="normalTxt"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public static string AESEncrypt(string normalTxt, string encryptKey = "YjEi0vU8")
        {
            var bytes = Encoding.Unicode.GetBytes(normalTxt);
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.Unicode.GetBytes(encryptKey);
            des.IV = aESiv;
            using (MemoryStream ms = new MemoryStream())
            {
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="securityTxt"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public static string AESDecrypt(string securityTxt, string encryptKey = "YjEi0vU8")
        {
            try
            {
                var bytes = Convert.FromBase64String(securityTxt);
                SymmetricAlgorithm des = Rijndael.Create();
                des.Key = Encoding.Unicode.GetBytes(encryptKey);
                des.IV = aESiv;
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    return Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
