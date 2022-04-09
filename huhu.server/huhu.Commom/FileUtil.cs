using System;
using System.IO;
using System.Text;
using System.Web;

namespace huhu.Commom
{
    /// <summary>
    /// 目录及文件操作
    /// </summary>
    public static class FileUtil
    {
        #region 创建目录
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dir">此地路径相对站点而言</param>
        public static void CreateDir(string dir)
        {
            if (dir.Length == 0) return;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(dir)))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(dir));
        }
        #endregion

        #region 创建目录路径
        /// <summary>
        /// 创建目录路径
        /// </summary>
        /// <param name="folderPath">物理路径</param>
        public static void CreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }
        #endregion

        #region 删除目录
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir">此地路径相对站点而言</param>
        public static void DeleteDir(string dir)
        {
            if (dir.Length == 0) return;
            if (Directory.Exists(HttpContext.Current.Server.MapPath(dir)))
                Directory.Delete(HttpContext.Current.Server.MapPath(dir), true);
        }
        #endregion

        #region 判断文件是否存在
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="file">格式:相对根目录</param>
        /// <returns></returns>
        public static bool FileExists(string file)
        {
            if (File.Exists(file))
                return true;
            else
                return false;
        }
        #endregion

        #region 读取文件内容
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="file">格式:a/b.htm,相对根目录</param>
        /// <returns></returns>
        public static string ReadFile(string file)
        {
            if (!FileExists(HttpContext.Current.Server.MapPath(file)))
                return "";
            try {
                StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(file), Encoding.UTF8);
                string str = sr.ReadToEnd();
                sr.Close();
                return str;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        #endregion

        #region 保存文件内容,自动创建目录
        /// <summary>
        /// 保存文件内容,自动创建目录
        /// </summary>
        /// <param name="tempDir">格式:a/b.htm,相对根目录</param>
        /// <returns></returns>
        public static void SaveFile(string TxtStr, string tempDir)
        {
            SaveFile(TxtStr, tempDir, true);
        }
        #endregion

        #region 保存为不带Bom的文件
        /// <summary>
        /// 保存为不带Bom的文件
        /// </summary>
        /// <param name="TxtStr"></param>
        /// <param name="tempDir"></param>
        /// <param name="noBom"></param>
        public static void SaveFile(string TxtStr, string tempDir, bool noBom)
        {
            try {
                CreateDir(GetFolderPath(true, tempDir));
                StreamWriter sw;
                if (noBom)
                    sw = new StreamWriter(HttpContext.Current.Server.MapPath(tempDir), false, new UTF8Encoding(false));
                else
                    sw = new StreamWriter(HttpContext.Current.Server.MapPath(tempDir), false, Encoding.UTF8);

                sw.Write(TxtStr);
                sw.Close();
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        #endregion

        #region 复制文件
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <param name="overwrite">如果已经存在是否覆盖？</param>
        public static void CopyFile(string file1, string file2, bool overwrite)
        {
            if (File.Exists(HttpContext.Current.Server.MapPath(file1)))
                File.Copy(HttpContext.Current.Server.MapPath(file1), HttpContext.Current.Server.MapPath(file2), overwrite);
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file">此地路径相对程序路径而言</param>
        public static void DeleteFile(string file)
        {
            if (file.Length == 0) return;
            if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                File.Delete(HttpContext.Current.Server.MapPath(file));
        }
        #endregion

        #region 获得文件的目录路径
        /// <summary>
        /// 获得文件的目录路径
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFolderPath(string filePath)
        {
            return GetFolderPath(false, filePath);
        }
        #endregion

        #region 获得文件的目录路径
        /// <summary>
        /// 获得文件的目录路径
        /// </summary>
        /// <param name="isUrl">是否是网址</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFolderPath(bool isUrl, string filePath)
        {
            if (isUrl)
                return filePath.Substring(0, filePath.LastIndexOf("/") + 1);
            else
                return filePath.Substring(0, filePath.LastIndexOf("\\") + 1);
        }
        #endregion

        #region 获得文件的名称
        /// <summary>
        /// 获得文件的名称
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            return GetFileName(false, filePath);
        }
        #endregion

        #region 获得文件的名称
        /// <summary>
        /// 获得文件的名称
        /// </summary>
        /// <param name="isUrl">是否是网址</param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(bool isUrl, string filePath)
        {
            if (isUrl)
                return filePath.Substring(filePath.LastIndexOf("/") + 1, filePath.Length - filePath.LastIndexOf("/") - 1);
            else
                return filePath.Substring(filePath.LastIndexOf("\\") + 1, filePath.Length - filePath.LastIndexOf("\\") - 1);
        }
        #endregion

        #region 获得文件的后缀
        /// <summary>
        /// 获得文件的后缀
        /// 不带点，小写
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileExt(string filePath)
        {
            return filePath.Substring(filePath.LastIndexOf(".") + 1, filePath.Length - filePath.LastIndexOf(".") - 1).ToLower();
        }
        #endregion

        #region 目录拷贝
        /// <summary>
        /// 目录拷贝
        /// </summary>
        /// <param name="OldDir"></param>
        /// <param name="NewDir"></param>
        public static void CopyDir(string OldDir, string NewDir)
        {
            DirectoryInfo OldDirectory = new DirectoryInfo(OldDir);
            DirectoryInfo NewDirectory = new DirectoryInfo(NewDir);
            CopyDir(OldDirectory, NewDirectory);
        }

        private static void CopyDir(DirectoryInfo OldDirectory, DirectoryInfo NewDirectory)
        {
            string NewDirectoryFullName = NewDirectory.FullName + "\\" + OldDirectory.Name;
            if (!Directory.Exists(NewDirectoryFullName))
                Directory.CreateDirectory(NewDirectoryFullName);

            FileInfo[] OldFileAry = OldDirectory.GetFiles();
            foreach (FileInfo aFile in OldFileAry)
                File.Copy(aFile.FullName, NewDirectoryFullName + "\\" + aFile.Name, true);

            DirectoryInfo[] OldDirectoryAry = OldDirectory.GetDirectories();
            foreach (DirectoryInfo aOldDirectory in OldDirectoryAry) {
                DirectoryInfo aNewDirectory = new DirectoryInfo(NewDirectoryFullName);
                CopyDir(aOldDirectory, aNewDirectory);
            }
        }
        #endregion

        #region 目录删除
        /// <summary>
        /// 目录删除
        /// </summary>
        /// <param name="OldDir"></param>
        public static void DelDir(string OldDir)
        {
            DirectoryInfo OldDirectory = new DirectoryInfo(OldDir);
            OldDirectory.Delete(true);
        }
        #endregion

        #region 目录剪切
        /// <summary>
        /// 目录剪切
        /// </summary>
        /// <param name="OldDirectory"></param>
        /// <param name="NewDirectory"></param>
        public static void CopyAndDelDir(string OldDirectory, string NewDirectory)
        {
            CopyDir(OldDirectory, NewDirectory);
            DelDir(OldDirectory);
        }
        #endregion

        #region 文件下载
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="_Request"></param>
        /// <param name="_Response"></param>
        /// <param name="_fullPath">源文件路径</param>
        /// <param name="_speed"></param>
        /// <returns></returns>
        public static bool DownloadFile(HttpRequest _Request, HttpResponse _Response, string _fullPath, long _speed)
        {
            string _fileName = GetFileName(false, _fullPath);
            try {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    double pack = 10240; //10K bytes
                    //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                    int sleep = (int)Math.Floor(1000 * pack / _speed) + 1;
                    if (_Request.Headers["Range"] != null) {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;

                    for (int i = 0; i < maxCount; i++) {
                        if (_Response.IsClientConnected) {
                            _Response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                            System.Threading.Thread.Sleep(sleep);
                        }
                        else {
                            i = maxCount;
                        }
                    }
                }
                catch {
                    return false;
                }
                finally {
                    br.Close();
                    myFile.Close();
                }
            }
            catch {
                return false;
            }
            return true;
        }
        #endregion

    }
}