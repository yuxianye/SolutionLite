using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Utility
{


    //private void ftpTest()
    //{
    //    ConfigHelper.GetAppSetting("ServerIP");
    //    int.TryParse(ConfigHelper.GetAppSetting("ServerPort"), out int serverPort);
    //    ConfigHelper.GetAppSetting("Username");
    //    ConfigHelper.GetAppSetting("Password");
    //    ConfigHelper.GetAppSetting("EnableSsl");


    //    FTPHelper fTPHelper = new FTPHelper("192.168.1.106", 21, "yuxianye", "yuxianye");
    //    bool result = fTPHelper.Put(@"C:\Users\yuxianye\Pictures\Screenshots\屏幕截图 2024-01-31 092731.png", @"test.png");
    //    Debug.Print(result.ToString());
    //}


    public class FTPHelper
    {
        #region 变量
        /// <summary>
        /// FTP请求对象
        /// </summary>
        FtpWebRequest request = null;
        /// <summary>
        /// FTP响应对象
        /// </summary>
        FtpWebResponse response = null;

        /// <summary>
        /// FTP服务器长地址
        /// </summary>
        public string FtpURI { get; private set; }
        /// <summary>
        /// FTP服务器IP
        /// </summary>
        public string ServerIP { get; private set; }
        /// <summary>
        /// FTP端口
        /// </summary>
        public int ServerPort { get; private set; }
        /// <summary>
        /// FTP用户
        /// </summary>
        public string Username { get; private set; }
        /// <summary>
        /// FTP密码
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// 是否启用SSL
        /// </summary>
        public bool EnableSsl { get; private set; }
        #endregion

        #region 构造
        /// <summary>  
        /// 初始化
        /// </summary>  
        /// <param name="FtpServerIP">IP</param> 
        /// <param name="ftpServerPort">端口</param> 
        /// <param name="FtpUserID">用户名</param> 
        /// <param name="FtpPassword">密码</param> 
        public FTPHelper(string ftpServerIP, int ftpServerPort, string ftpUsername, string ftpPassword, bool ftpEnableSsl = false)
        {
            ServerIP = ftpServerIP;
            ServerPort = ftpServerPort;
            Username = ftpUsername;
            Password = ftpPassword;
            EnableSsl = ftpEnableSsl;
            FtpURI = string.Format("ftp://{0}:{1}/", ftpServerIP, ftpServerPort);
        }
        ~FTPHelper()
        {
            if (response != null)
            {
                response.Close();
                response = null;
            }
            if (request != null)
            {
                request.Abort();
                request = null;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 建立FTP链接,返回响应对象
        /// </summary>
        /// <param name="uri">FTP地址</param>
        /// <param name="ftpMethod">操作命令</param>
        private FtpWebResponse Open(Uri uri, string ftpMethod)
        {
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(uri);
                request.Method = ftpMethod;
                request.UseBinary = true;
                request.KeepAlive = false;
                request.UsePassive = true;//被动模式
                request.EnableSsl = EnableSsl;
                request.Credentials = new NetworkCredential(Username, Password);
                request.Timeout = 30000;
                //首次连接FTP Server时，会有一个证书分配过程。
                //根据验证过程，远程证书无效。
                ServicePoint sp = request.ServicePoint;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                return (FtpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 建立FTP链接,返回请求对象
        /// </summary>
        /// <param name="uri">FTP地址</param>
        /// <param name="ftpMethod">操作命令</param>
        private FtpWebRequest OpenRequest(Uri uri, string ftpMethod)
        {
            try
            {
                request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = ftpMethod;
                request.UseBinary = true;
                request.KeepAlive = false;
                request.UsePassive = true;//被动模式
                request.EnableSsl = EnableSsl;
                request.Credentials = new NetworkCredential(Username, Password);
                request.Timeout = 30000;

                ServicePoint sp = request.ServicePoint;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                return request;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 证书验证回调
        /// </summary>
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="remoteFileName">远程文件</param>
        /// <param name="localFileName">本地文件</param>
        public bool Get(string remoteFileName, string localFileName)
        {
            response = Open(new Uri(FtpURI + remoteFileName), WebRequestMethods.Ftp.DownloadFile);
            if (response == null) return false;

            try
            {
                using (FileStream outputStream = new FileStream(localFileName, FileMode.Create))
                {
                    using (Stream ftpStream = response.GetResponseStream())
                    {
                        long length = response.ContentLength;
                        int bufferSize = 2048;
                        int readCount;
                        byte[] buffer = new byte[bufferSize];
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                        while (readCount > 0)
                        {
                            outputStream.Write(buffer, 0, readCount);
                            readCount = ftpStream.Read(buffer, 0, bufferSize);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="localFileName">本地文件</param>
        /// <param name="localFileName">远程文件</param>
        public bool Put(string localFileName, string remoteFileName)
        {
            FileInfo fi = new FileInfo(localFileName);
            if (fi.Exists == false) return false;
            request = OpenRequest(new Uri(FtpURI + remoteFileName), WebRequestMethods.Ftp.UploadFile);
            if (request == null) return false;

            request.ContentLength = fi.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            try
            {
                using (var fs = fi.OpenRead())
                {
                    using (var strm = request.GetRequestStream())
                    {
                        contentLen = fs.Read(buff, 0, buffLength);
                        while (contentLen != 0)
                        {
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        public bool DeleteFile(string fileName)
        {
            response = Open(new Uri(FtpURI + fileName), WebRequestMethods.Ftp.DeleteFile);
            return response == null ? false : true;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        public bool CreateDirectory(string dirName)
        {
            response = Open(new Uri(FtpURI + dirName), WebRequestMethods.Ftp.MakeDirectory);
            return response == null ? false : true;
        }
        /// <summary>
        /// 删除目录(包括下面所有子目录和子文件)
        /// </summary>
        public bool DeleteDirectory(string dirName)
        {
            var listAll = GetDirectoryAndFiles(dirName);
            if (listAll == null) return false;

            foreach (var m in listAll)
            {
                if (m.IsDirectory)
                    DeleteDirectory(m.Path);
                else
                    DeleteFile(m.Path);
            }
            response = Open(new Uri(FtpURI + dirName), WebRequestMethods.Ftp.RemoveDirectory);
            return response == null ? false : true;
        }

        /// <summary>
        /// 获取目录的文件和一级子目录信息
        /// </summary>
        public List<FileStruct> GetDirectoryAndFiles(string dirName)
        {
            var fileList = new List<FileStruct>();
            response = Open(new Uri(FtpURI + dirName), WebRequestMethods.Ftp.ListDirectoryDetails);
            if (response == null) return fileList;

            try
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream, Encoding.Default))
                    {
                        string line = null;
                        while ((line = sr.ReadLine()) != null)
                        {
                            //line的格式如下：serv-u(文件夹为第1位为d)
                            //drw-rw-rw-   1 user     group           0 Jun 10  2019 BStatus
                            //-rw-rw-rw-   1 user     group         625 Dec  7  2018 FTP文档.txt
                            string[] arr = line.Split(' ');
                            if (arr.Length < 12) continue;//remotePath不为空时，第1行返回值为：total 10715

                            var model = new FileStruct()
                            {
                                IsDirectory = line.Substring(0, 3) == "drw" ? true : false,
                                Name = arr[arr.Length - 1],
                                Path = dirName + "/" + arr[arr.Length - 1]
                            };

                            if (model.Name != "." && model.Name != "..")//排除.和..
                            {
                                fileList.Add(model);
                            }
                        }
                    }
                }
                return fileList;
            }
            catch
            {
                return fileList;
            }
        }
        /// <summary>
        /// 获取目录的文件
        /// </summary>
        public List<FileStruct> GetFiles(string dirName)
        {
            var fileList = new List<FileStruct>();
            response = Open(new Uri(FtpURI + dirName), WebRequestMethods.Ftp.ListDirectory);
            if (response == null) return fileList;

            try
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream, Encoding.Default))
                    {
                        string line = null;
                        while ((line = sr.ReadLine()) != null)
                        {
                            var model = new FileStruct()
                            {
                                Name = line,
                                Path = dirName + "/" + line
                            };
                            fileList.Add(model);
                        }
                    }
                }
                return fileList;
            }
            catch
            {
                return fileList;
            }
        }

        /// <summary>
        /// 获得远程文件大小
        /// </summary>
        public long GetFileSize(string fileName)
        {
            response = Open(new Uri(FtpURI + fileName), WebRequestMethods.Ftp.GetFileSize);
            return response == null ? -1 : response.ContentLength;
        }
        /// <summary>
        /// 文件是否存在
        /// </summary>
        public bool FileExist(string fileName)
        {
            long length = GetFileSize(fileName);
            return length == -1 ? false : true;
        }
        /// <summary>
        /// 目录是否存在
        /// </summary>
        public bool DirectoryExist(string dirName)
        {
            var list = GetDirectoryAndFiles(Path.GetDirectoryName(dirName));
            return list.Count(m => m.IsDirectory == true && m.Name == dirName) > 0 ? true : false;
        }
        /// <summary>
        /// 更改目录或文件名
        /// </summary>
        /// <param name="oldName">老名称</param>
        /// <param name="newName">新名称</param>
        public bool ReName(string oldName, string newName)
        {
            request = OpenRequest(new Uri(FtpURI + oldName), WebRequestMethods.Ftp.Rename);
            request.RenameTo = newName;
            try
            {
                response = (FtpWebResponse)request.GetResponse();
                return response == null ? false : true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// FTP文件类
    /// </summary>
    public class FileStruct
    {
        /// <summary>
        /// 是否为目录
        /// </summary>
        public bool IsDirectory { get; set; }
        /// <summary>
        /// 创建时间(FTP上无法获得时间)
        /// </summary>
        //public DateTime CreateTime { get; set; }
        /// <summary>
        /// 文件或目录名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
    }
}
