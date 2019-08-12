using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Tests
{
    [TestClass()]
    public class SecurityTests
    {
        [TestMethod()]
        public void MD5EncryptTest()
        {
            string tmp = Utility.Security.MD5Encrypt("yuxianye");
            string tmp2 = Utility.Security.MD5Encrypt("yuxianye");
            string tmp3 = Utility.Security.MD5Encrypt("yuxianye1");
            Assert.AreEqual(tmp2, tmp);
            Assert.AreNotEqual(tmp3, tmp);
        }

        [TestMethod()]
        public void RSAEncryptTest()
        {
            string tmp = Utility.Security.RSAEncrypt("yuxianye");
            tmp = Utility.Security.RSADecrypt(tmp);
            Assert.AreEqual(tmp, "yuxianye");
        }
        [TestMethod()]
        public void RSADecryptTest()
        {
            string tmp = Utility.Security.RSAEncrypt("yuxianye");
            tmp = Utility.Security.RSADecrypt(tmp);
            Assert.AreEqual(tmp, "yuxianye");

        }

        [TestMethod()]
        public void SHA1EncryptTest()
        {
            string tmp = Utility.Security.SHA1Encrypt("yuxianye");
            string tmp2 = Utility.Security.SHA1Encrypt("yuxianye");
            string tmp3 = Utility.Security.SHA1Encrypt("yuxianye1");
            Assert.AreEqual(tmp2, tmp);
            Assert.AreNotEqual(tmp3, tmp);
        }

        [TestMethod()]
        public void SHA256EncryptTest()
        {
            string tmp = Utility.Security.SHA256Encrypt("yuxianye");
            string tmp2 = Utility.Security.SHA256Encrypt("yuxianye");
            string tmp3 = Utility.Security.SHA256Encrypt("yuxianye1");
            Assert.AreEqual(tmp2, tmp);
            Assert.AreNotEqual(tmp3, tmp);
        }

        [TestMethod()]
        public void SHA384EncryptTest()
        {
            string tmp = Utility.Security.SHA384Encrypt("yuxianye");
            string tmp2 = Utility.Security.SHA384Encrypt("yuxianye");
            string tmp3 = Utility.Security.SHA384Encrypt("yuxianye1");
            Assert.AreEqual(tmp2, tmp);
            Assert.AreNotEqual(tmp3, tmp);
        }

        [TestMethod()]
        public void SHA512EncryptTest()
        {
            string tmp = Utility.Security.SHA512Encrypt("yuxianye");
            string tmp2 = Utility.Security.SHA512Encrypt("yuxianye");
            string tmp3 = Utility.Security.SHA512Encrypt("yuxianye1");
            Assert.AreEqual(tmp2, tmp);
            Assert.AreNotEqual(tmp3, tmp);
        }

        [TestMethod()]
        public void DesEncryptTest()
        {
            string tmp = Utility.Security.DesEncrypt("yuxianye");
            tmp = Utility.Security.DesDecrypt(tmp);
            Assert.AreEqual(tmp, "yuxianye");
        }

        [TestMethod()]
        public void DesDecryptTest()
        {
            string tmp = Utility.Security.DesEncrypt("yuxianye");
            tmp = Utility.Security.DesDecrypt(tmp);
            Assert.AreEqual(tmp, "yuxianye");
        }

        [TestMethod()]
        public void AESEncryptTest()
        {
            string tmp = Utility.Security.AESEncrypt("yuxianye");
            tmp = Utility.Security.AESDecrypt(tmp);
            Assert.AreEqual(tmp, "yuxianye");
        }

        [TestMethod()]
        public void AESDecryptTest()
        {
            string tmp = Utility.Security.AESEncrypt("yuxianye");
            tmp = Utility.Security.AESDecrypt(tmp);
            Assert.AreEqual(tmp, "yuxianye");
        }
    }
}