using System;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data.OleDb;
using Microsoft.Win32;
using System.Diagnostics;

namespace Common
{
    public class FileOperate
    {
        public FileOperate()
        { }

        #region 图片下载

        /// <summary>
        /// 图片下载
        /// </summary>
        /// <param name="httppath">远程路径</param>
        /// <param name="targetpath">下载目标路径</param>
        /// <param name="downloadfilename">文件名</param>
        public static void DownloadImg(string httppath, string targetpath, string downloadfilename)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            try
            {
                if (!httppath.Contains("http:") && !httppath.Contains("https:")) { httppath = "http:" + httppath; }
                request = (HttpWebRequest)WebRequest.Create(httppath);
                request.Method = "GET";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = true;
                request.ContentType = "image/png";
                response = (HttpWebResponse)request.GetResponse();
                // 以字符流的方式读取HTTP响应
                stream = response.GetResponseStream();
                Image.FromStream(stream).Save(targetpath + downloadfilename.Replace("\\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "").Replace("|", ""));
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex);
                //clubid:410的Stadium的第一张图挂图,判断如果是该图片则不下载
                if (httppath != "http://akacdn.transfermarkt.de/bilder/stadion/st_5743_1376301239.jpg?lm=0" && httppath != "http://akacdn.transfermarkt.de//bilder/stadion/st_5743_1376301239.jpg?lm=0" && httppath != "http://tmssl.akamaized.net//bilder/stadion/st_5743_1376301239.jpg?lm=0")
                {
                    if (httppath.IndexOf("/premiumfoto/") == -1)
                    {
                        if (ex.HResult == -2146233079 && ex.Message == "远程服务器返回错误: (404) 未找到。")
                        {
                            if (httppath.IndexOf("/normquad/") != -1) { httppath = httppath.Replace("/normquad/", "/head/"); }
                            else if (httppath.IndexOf("/head/") != -1) { httppath = httppath.Replace("/head/", "/normal/"); }
                            else if (httppath.IndexOf("/normal/") != -1) { httppath = httppath.Replace("/normal/", "/header/"); }
                            else if (httppath.IndexOf("/header/") != -1) { httppath = httppath.Replace("/header/", "/normal/"); }
                            else if (httppath.IndexOf("/small/") != -1) { httppath = httppath.Replace("/small/", "/medium/"); }
                            else if (httppath.IndexOf("/head/") != -1) { httppath = httppath.Replace("/head/", "/normquad/"); }
                        }
                        if (ex.HResult != -2147024809 && ex.Message != "参数无效。") { DownloadImg(httppath, targetpath, downloadfilename); }
                    }
                    else
                    {
                        //抓取Player大图异常
                        if (ex.HResult != -2146233079 || ex.Message != "远程服务器返回错误: (404) 未找到。") { DownloadImg(httppath, targetpath, downloadfilename); }
                    }
                }
            }
            finally
            {
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }
        }

        #endregion 图片下载

        #region 图片下载

        /// <summary>
        /// 图片下载
        /// </summary>
        /// <param name="httppath">远程路径</param>
        /// <param name="targetpath">下载目标路径</param>
        /// <param name="downloadfilename">文件名</param>
        public static void DownloadImgClient(string httppath, string targetpath, string downloadfilename)
        {
            try
            {
                Path.GetInvalidPathChars();
                DirectoryInfo di = new DirectoryInfo(targetpath);
                if (!di.Exists) { di.Create(); }
                //Uri _uri = new Uri(httppath);
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.DownloadFile(httppath, targetpath + downloadfilename);
            }
            catch
            {
                DownloadImgClient(httppath, targetpath, downloadfilename);
            }
        }

        #endregion 图片下载

        #region 文件下载

        /// <summary>
        /// 文件下载（流方式）
        /// </summary>
        /// <param name="targetpath">下载目标路径</param>
        public static void DownloadImgStream(string targetpath)
        {
            //前面可以做用户登录验证、用户权限验证等。
            string filename = targetpath.Substring(targetpath.LastIndexOf("/") + 1);//客户端保存的文件名
            string filePath = HttpContext.Current.Server.MapPath(targetpath);//要被下载的文件路径
            if (File.Exists(filePath))
            {
                HttpContext.Current.Response.ContentType = "application/octet-stream";  //二进制流
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                HttpContext.Current.Response.TransmitFile(filePath); //将指定文件写入 HTTP 响应输出流
            }
            else
            {
                HttpContext.Current.Response.Write("不存在该文件");
            }
        }

        /// <summary>
        /// 文件下载（流方式）自定义下载名称
        /// </summary>
        /// <param name="targetpath"></param>
        /// <param name="selfDownName"></param>
        public static void DownloadImgStream(string targetpath, string selfDownName)
        {
            //前面可以做用户登录验证、用户权限验证等。
            string filename = selfDownName;//客户端保存的文件名
            string filePath = HttpContext.Current.Server.MapPath(targetpath);//要被下载的文件路径
            if (File.Exists(filePath))
            {
                System.IO.FileInfo fl = new FileInfo(filePath);
                HttpContext.Current.Response.ContentType = "application/octet-stream";  //二进制流
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + fl.Extension);
                HttpContext.Current.Response.TransmitFile(filePath); //将指定文件写入 HTTP 响应输出流
            }
            else
            {
                HttpContext.Current.Response.Write("不存在该文件");
            }
        }

        #endregion 文件下载

        #region 移动文件

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="OriginalPath">源路径及文件名</param>
        /// <param name="TargetPath">目标文件及文件名</param>
        public static bool FileMove(string OriginalPath, string TargetPath)
        {
            string path = HttpContext.Current.Server.MapPath(OriginalPath);
            string newPath = HttpContext.Current.Server.MapPath(TargetPath);
            FileInfo fi = new FileInfo(path);
            try
            {
                if (fi.Exists)
                {
                    if (System.IO.File.Exists(Path.GetFullPath(newPath)))
                    {
                        File.Delete(Path.GetFullPath(newPath));
                    }
                    if (!Directory.Exists(Path.GetDirectoryName(newPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                    }

                    fi.MoveTo(newPath);
                    return true;
                }
                else { return false; }
            }
            catch { throw; }
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="path">包含文件名的路径</param>
        /// <param name="moveToPath">移动到的文件夹路径</param>
        public static string FileMove1(string path, string moveToPath)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath(moveToPath));
            if (!dir.Exists)
                dir.Create();
            System.IO.FileInfo _file = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(path));
            moveToPath += "/" + _file.Name;
            try
            {
                if (_file.Exists)
                {
                    _file.MoveTo(HttpContext.Current.Server.MapPath(moveToPath));
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "移动文件出现异常：" + moveToPath);
                return "";
            }
            return moveToPath;
        }

        #endregion 移动文件

        #region 复制文件

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="OriginalPath">源路径及文件名</param>
        /// <param name="TargetPath">目标文件及文件名</param>
        public static bool FileCope(string OriginalPath, string TargetPath)
        {
            string path = HttpContext.Current.Server.MapPath(OriginalPath);
            string newPath = HttpContext.Current.Server.MapPath(TargetPath);
            FileInfo fi = new FileInfo(path);
            try
            {
                if (fi.Exists)
                {
                    if (System.IO.File.Exists(Path.GetFullPath(newPath)))
                    {
                        File.Delete(Path.GetFullPath(newPath));
                    }
                    if (!Directory.Exists(Path.GetDirectoryName(newPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                    }

                    fi.CopyTo(newPath);
                    return true;
                }
                else { return false; }
            }
            catch { throw; }
        }

        #endregion 复制文件

        #region 根据虚拟路径获取物理路径

        /// <summary>
        /// 根据虚拟路径获取物理路径(文件夹)
        /// <param name="path">虚拟路径</param>
        /// <param name="isfile">是否是文件，true是文件路径， false是文件夹路径</param>
        /// </summary>
        public static string GetAbsolutePath(string path, bool isfile)
        {
            try
            {
                string _path = HttpContext.Current.Server.MapPath(path);
                if (!isfile)//如果是文件夹则判断是否存在
                {
                    if (!Directory.Exists(_path))
                    {
                        Directory.CreateDirectory(_path);//不存在就创建文件夹
                    }
                }
                else
                {
                    if (!File.Exists(_path))
                    {
                        _path = "";//不存在赋值为空
                    }
                }
                return _path;
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "路径获取失败");
                throw;
            }
        }

        #endregion 根据虚拟路径获取物理路径

        #region 上传文件

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">上传控件</param>
        /// <param name="sSavePath">保存路径(不含文件名)</param>
        /// <param name="sSaveFileName">文件名称(不含后缀名)</param>
        /// <returns>上传的文件名称(含后缀名)</returns>
        public static string UploadFile(HttpPostedFileBase file, string sSavePath, string sSaveFileName)
        {
            FileInfo fi = new FileInfo(file.FileName);
            try
            {
                string strExt = fi.Extension;
                string sPhyTargetPath = HttpContext.Current.Server.MapPath(sSavePath);
                if (!Directory.Exists(sPhyTargetPath))
                {
                    Directory.CreateDirectory(sPhyTargetPath);
                }
                file.SaveAs(sPhyTargetPath + "\\" + sSaveFileName + "" + strExt);
                return sSaveFileName + "" + strExt;
            }
            catch { throw; }
        }

        #endregion 上传文件

        #region 上传文件

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">上传控件</param>
        /// <param name="sSavePath">保存路径(不含文件名)</param>
        public static bool UploadFile(HttpPostedFileBase file, string sSavePath)
        {
            try
            {
                string sPhyTargetPath = HttpContext.Current.Server.MapPath(sSavePath);//获取文件地址
                if (!Directory.Exists(Path.GetDirectoryName(sPhyTargetPath)))//文件夹不存在
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(sPhyTargetPath));//创建文件夹
                }
                file.SaveAs(sPhyTargetPath);
                return true;
            }
            catch { throw; }
        }

        #endregion 上传文件

        #region 断点续传

        /// <summary>
        /// 断点续传
        /// </summary>
        /// <param name="file"></param>
        /// <param name="sSavePath"></param>
        /// <param name="sSaveFileName"></param>
        public static void UploadPointFile(HttpPostedFile file, string sSavePath, string sSaveFileName)
        {
            long _startpos = 0;
            System.IO.FileStream fs;
            if (file != null)
            {
                if (!Directory.Exists(sSavePath))
                {
                    Directory.CreateDirectory(sSavePath);
                }
            }
            sSaveFileName = sSavePath + sSaveFileName;
            if (System.IO.File.Exists(sSaveFileName))
            {
                fs = System.IO.File.OpenWrite(sSaveFileName);
                _startpos = fs.Length;
                fs.Seek(_startpos, System.IO.SeekOrigin.Current);   //移动文件流中的当前指针
            }
            else
            {
                fs = new System.IO.FileStream(sSaveFileName, System.IO.FileMode.Create);
                _startpos = 0;
            }
            //打开网络连接
            try
            {
                //向服务器请求，获得服务器回应数据流
                System.IO.Stream ns = file.InputStream;
                byte[] nbytes = new byte[512];
                int nReadSize = 0;
                nReadSize = ns.Read(nbytes, 0, 512);
                while (nReadSize > 0)
                {
                    fs.Write(nbytes, 0, nReadSize);
                    nReadSize = ns.Read(nbytes, 0, 512);
                }
                fs.Close();
                ns.Close();
            }
            catch { }
        }

        #endregion 断点续传

        #region 写文件

        /// <summary>
        /// 写文件
        /// <param name="str">要写入文件的字符串</param>
        /// <param name="path">文件存储路径</param>
        /// <param name="fileName">文件名含后缀</param>
        /// </summary>
        public static void WriteFile(string str, string path, string fileName)
        {
            FileStream fs = null;
            string _path = HttpContext.Current.Server.MapPath(path);
            try
            {
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);//不存在就创建文件夹
                }
                fs = new FileStream(_path + fileName, FileMode.Create);
                byte[] _content = new System.Text.UTF8Encoding(true).GetBytes(str);
                fs.Write(_content, 0, _content.Length);
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "写入json权限文件:" + _path + fileName);
                throw;
            }
            finally { fs.Close(); }
        }

        /// <summary>
        /// 保存配置文件json里面数据
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public static bool SaveFileConfig(Newtonsoft.Json.Linq.JObject jObject)
        {
            bool bl = false;
            string path = ConfigurationManager.AppSettings["ConfigPath"].ToString();
            FileStream fs = null;
            try
            {
                path = HttpContext.Current.Server.MapPath(path);
                fs = new FileStream(path, FileMode.Create);
                byte[] _content = new System.Text.UTF8Encoding(true).GetBytes(jObject.ToString());
                fs.Write(_content, 0, _content.Length);
                bl = true;
                fs.Dispose();
                fs.Close();
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Dispose();
                    fs.Close();
                }
                LogRecord.LogCatch(ex, " 保存配置文件json里面数据:" + path);
                throw;
            }
            return bl;
        }

        #endregion 写文件

        #region 读权限json文件

        /// <summary>
        /// 读文件
        /// <param name="fileName">文件名</param>
        /// </summary>
        public static string ReadFile(string fileName)
        {
            string path = ConfigurationManager.AppSettings["FunctionRelativePath"].ToString() + fileName + ".json";
            string _path = HttpContext.Current.Server.MapPath(path);
            try
            {
                if (File.Exists(_path))
                {
                    return System.IO.File.ReadAllText(_path);
                }
                return "";
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "读取json权限文件:" + _path);
                throw;
            }
        }

        /// <summary>
        /// 读文件
        /// <param name="fileName">文件名</param>
        /// </summary>
        public static string ReadAppFile(string fileName)
        {
            string path = ConfigurationManager.AppSettings["AppFunctionsPath"].ToString() + fileName + ".json";
            string _path = HttpContext.Current.Server.MapPath(path);
            try
            {
                if (File.Exists(_path))
                {
                    return System.IO.File.ReadAllText(_path);
                }
                return "";
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "读取json权限文件:" + _path);
                throw;
            }
        }

        /// <summary>
        /// 读取配置文件json里面数据
        /// </summary>
        /// <returns></returns>
        public static string ReadFileConfig()
        {
            string path = ConfigurationManager.AppSettings["ConfigPath"].ToString();

            try
            {
                path = HttpContext.Current.Server.MapPath(path);
                if (File.Exists(path))
                {
                    return System.IO.File.ReadAllText(path, System.Text.Encoding.GetEncoding("UTF-8"));
                }
                return "";
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, " 读取配置文件json里面数据:" + path);
                throw;
            }
        }

        #endregion 读权限json文件

        #region 读工资配置json文件

        /// <summary>
        /// 读文件
        /// <param name=""></param>
        /// </summary>
        public static string ReadWagesFile()
        {
            string path = "/file/wages/wages.json";
            string _path = HttpContext.Current.Server.MapPath(path);
            try
            {
                if (File.Exists(_path))
                {
                    return System.IO.File.ReadAllText(_path);
                }
                return "";
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "读工资配置json文件:" + _path);
                throw;
            }
        }

        /// <summary>
        /// 得到json 配置对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelName">对象名称</param>
        /// <returns></returns>
        public static System.Collections.Generic.List<T> SetJsonModel<T>(string modelName)
        {
            System.Collections.Generic.List<T> list = null;
            Newtonsoft.Json.Linq.JObject jObject = SerializeHelper.JsonJObjectParse(ReadFileConfig());
            list = jObject[modelName].ToObject<System.Collections.Generic.List<T>>();
            return list;
        }

        #endregion 读工资配置json文件

        #region 删除文件

        /// <summary>
        /// 删除文件
        /// <param name="sPath">文件路径</param>
        /// </summary>
        public static void DeleteFile(string sPath)
        {
            //删除单个文件
            string strPath = HttpContext.Current.Server.MapPath(sPath);
            FileInfo file = new FileInfo(strPath);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        #endregion 删除文件

        #region 删除文件(临时文件夹中文件)

        /// <summary>
        /// 删除临时文件夹中文件
        /// <param name="sPath">文件路径</param>
        /// </summary>
        public static bool DeleteFileUrl(string sPath)
        {
            //删除单个文件
            bool result = true;
            try
            {
                System.IO.FileInfo fldel = null;
                if (sPath.IndexOf("file/UserTemporary") > -1)
                {
                    fldel = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(sPath));
                    fldel.Delete();
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        #endregion 删除文件(临时文件夹中文件)

        #region 删除权限json文件

        /// <summary>
        /// 删除文件
        /// <param name="sPath">文件路径</param>
        /// </summary>
        public static void DeleteFunctionFile(string sPath)
        {
            string strPath = "";
            try
            {
                string _targetpagepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DeleteFunctionsPath"].ToString());
                //删除单个文件
                strPath = _targetpagepath + sPath;
                FileInfo file = new FileInfo(strPath);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "删除json权限文件:" + strPath);
                throw;
            }
        }

        /// <summary>
        /// 删除App权限文件
        /// <param name="sPath">文件路径</param>
        /// </summary>
        public static void DeleteAppFunctionFile(string sPath)
        {
            string strPath = "";
            try
            {
                string _targetpagepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AppFunctionsPath"].ToString());
                //删除单个文件
                strPath = _targetpagepath + sPath;
                FileInfo file = new FileInfo(strPath);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "删除json权限文件:" + strPath);
                throw;
            }
        }

        #endregion 删除权限json文件

        #region 生成缩略图

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径</param>
        /// <param name="thumbnailPath">缩略图路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）
                    break;

                case "W"://指定宽，高按比例
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;

                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;

                case "Cut"://指定高宽裁减（不变形）
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;

                default:
                    break;
            }
            //新建一个bmp图片
            Image bitmap = new Bitmap(towidth, toheight);
            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, ImageFormat.Jpeg);
            }
            catch { throw; }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        #endregion 生成缩略图

        #region 时间文件名

        /// <summary>
        /// true 文件夹名  false 文件名
        /// </summary>
        /// <param name="floder"></param>
        /// <returns></returns>
        public static string GetTimeName(bool floder)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.Year);
            sb.Append(DateTime.Now.Month);
            sb.Append(DateTime.Now.Day);
            if (!floder)
            {
                sb.Append(DateTime.Now.Hour);
                sb.Append(DateTime.Now.Minute);
                sb.Append(DateTime.Now.Second);
                sb.Append(DateTime.Now.Millisecond);
            }
            return sb.ToString();
        }

        #endregion 时间文件名

        #region//邮件发送

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="MessageFrom">发送人邮箱</param>
        /// <param name="MessageTo">发送对象邮箱</param>
        /// <param name="MessageSubject">标题</param>
        /// <param name="MessageBody">内容</param>
        /// <returns></returns>
        public static bool SendMail(string MessageTo, string MessageSubject, string MessageBody)  //发送验证邮件
        {
            Newtonsoft.Json.Linq.JObject jObject = SerializeHelper.JsonJObjectParse(Common.FileOperate.ReadFileConfig());
            string strEmail = jObject["Email"]["email"].ToString();
            string strPwd = jObject["Email"]["password"].ToString();

            MailMessage message = new MailMessage();
            message.To.Add(MessageTo);
            message.From = new MailAddress(strEmail);
            message.Subject = MessageSubject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Body = MessageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true; //是否为html格式
            message.Priority = MailPriority.High; //发送邮件的优先等级
            SmtpClient sc = null;
            if (strEmail.ToLower().IndexOf("@qq.com") > -1)
                sc = new SmtpClient("smtp.qq.com", 25);//腾讯个人邮箱
            else
                sc = new SmtpClient("smtp.exmail.qq.com", 25);//腾讯企业邮箱

            sc.Timeout = 2000000;
            sc.EnableSsl = true;//是否SSL加密
            sc.Credentials = new System.Net.NetworkCredential(strEmail, strPwd); //指定登录服务器的用户名和密码(注意：这里的密码是开通上面的pop3/smtp服务提供给你的授权密码，不是你的qq密码)

            try
            {
                sc.Send(message); //发送邮件
            }
            catch (Exception e)
            {
                LogRecord.LogCatch(e, "发送邮件失败" + MessageTo);
                return false;
            }
            return true;
        }

        #endregion

        #region 读取Excel

        /// <summary>
        /// 将excel导入到datatable
        /// </summary>
        /// <param name="filePath">excel路径</param>
        /// <param name="isColumnName">第一行是否是列名</param>
        /// <param name="startRow">从下标为startRow的行开始填充</param>
        /// <returns>返回datatable</returns>
        public static DataTable ExcelToDataTable(string filePath, bool isColumnName, int startRow)
        {
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    // 2007版本
                    if (filePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本
                    else if (filePath.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0);//读取第一个sheet，也能循环读取每个sheet
                        dataTable = new DataTable();
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum + 1;//+1 包括列名的行，总行数
                            if (rowCount > 0)
                            {
                                IRow firstRow = sheet.GetRow(0);//第一行
                                int cellCount = firstRow.LastCellNum;//列数

                                //构建datatable的列
                                if (isColumnName) //如果是列表则直接获取
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        cell = firstRow.GetCell(i);
                                        if (cell != null)
                                        {
                                            if (cell.StringCellValue != null)
                                            {
                                                column = new DataColumn(cell.StringCellValue);
                                                dataTable.Columns.Add(column);
                                            }
                                        }
                                    }
                                }
                                else //如果不是列表就自己构建列名
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        column = new DataColumn("column" + (i + 1));
                                        dataTable.Columns.Add(column);
                                    }
                                }

                                //填充行（要读取列名则i<rowCount 否则i <= rowCount）
                                for (int i = startRow; i < rowCount; ++i)
                                {
                                    row = sheet.GetRow(i);
                                    if (row == null) continue;

                                    dataRow = dataTable.NewRow();
                                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                                    {
                                        cell = row.GetCell(j);
                                        if (cell == null)
                                        {
                                            dataRow[j] = "";
                                        }
                                        else
                                        {
                                            //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)
                                            dataRow[j] = GetValueTypeForXLS(cell);
                                            //switch (cell.CellType)
                                            //{
                                            //    case CellType.Blank:
                                            //        dataRow[j] = "";
                                            //        break;
                                            //    case CellType.Numeric:
                                            //        short format = cell.CellStyle.DataFormat;
                                            //        //对时间格式2017.12.12、2017/12/12、2017-12-12等的处理
                                            //        if (format == 14 || format == 31 || format == 57 || format == 58)
                                            //            dataRow[j] = cell.DateCellValue;
                                            //        else
                                            //            dataRow[j] = cell.NumericCellValue;
                                            //        break;
                                            //    case CellType.String:
                                            //        dataRow[j] = cell.StringCellValue;
                                            //        break;
                                            //}
                                        }
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "导入excel到DataTable");
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }
        /// <summary>
        /// 将excel导入到datatable
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isColumnName"></param>
        /// <param name="startRow"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable2(string filePath, bool isColumnName, int startRow)
        {
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    // 2007版本
                    if (filePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本
                    else if (filePath.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0);//读取第一个sheet，也能循环读取每个sheet
                        dataTable = new DataTable();
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum + 1;//+1 包括列名的行，总行数
                            if (rowCount > 0)
                            {
                                IRow firstRow = sheet.GetRow(startRow - 1);//列名行
                                int cellCount = firstRow.LastCellNum;//列数

                                //构建datatable的列
                                if (isColumnName) //如果是列表则直接获取
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        cell = firstRow.GetCell(i);
                                        if (cell != null)
                                        {
                                            if (cell.StringCellValue != null)
                                            {
                                                column = new DataColumn(cell.StringCellValue);
                                                dataTable.Columns.Add(column);
                                            }
                                        }
                                    }
                                }
                                else //如果不是列表就自己构建列名
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        column = new DataColumn("column" + (i + 1));
                                        dataTable.Columns.Add(column);
                                    }
                                }

                                //填充行（要读取列名则i<rowCount 否则i <= rowCount）
                                for (int i = startRow; i < rowCount; ++i)
                                {
                                    row = sheet.GetRow(i);
                                    if (row == null) continue;

                                    dataRow = dataTable.NewRow();
                                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                                    {
                                        cell = row.GetCell(j);
                                        if (cell == null)
                                        {
                                            dataRow[j] = "";
                                        }
                                        else
                                        {
                                            dataRow[j] = GetValueTypeForXLS(cell);
                                        }
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "导入excel到DataTable");
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }
        public static DataSet GetAllExcelSheetToDataTable(string filePath, bool isColumnName, int startRow)
        {
            DataSet ds = null;
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    // 2007版本
                    if (filePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本
                    else if (filePath.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        ds = new DataSet();
                        for (var sIndex = 0; sIndex < workbook.NumberOfSheets; sIndex++)
                        {
                            sheet = workbook.GetSheetAt(sIndex);//读取第一个sheet，也能循环读取每个sheet
                            dataTable = new DataTable(sheet.SheetName);
                            if (sheet != null)
                            {
                                int rowCount = sheet.LastRowNum + 1;//+1 包括列名的行，总行数
                                if (rowCount > 0)
                                {
                                    IRow firstRow = sheet.GetRow(0);//第一行
                                    int cellCount = firstRow.LastCellNum;//列数

                                    //构建datatable的列
                                    if (isColumnName) //如果是列表则直接获取
                                    {
                                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                        {
                                            cell = firstRow.GetCell(i);
                                            if (cell != null)
                                            {
                                                if (cell.StringCellValue != null)
                                                {
                                                    column = new DataColumn(cell.StringCellValue);
                                                    dataTable.Columns.Add(column);
                                                }
                                            }
                                        }
                                    }
                                    else //如果不是列表就自己构建列名
                                    {
                                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                        {
                                            column = new DataColumn("column" + (i + 1));
                                            dataTable.Columns.Add(column);
                                        }
                                    }

                                    //填充行（要读取列名则i<rowCount 否则i <= rowCount）
                                    for (int i = startRow; i < rowCount; ++i)
                                    {
                                        row = sheet.GetRow(i);
                                        if (row == null) continue;

                                        dataRow = dataTable.NewRow();
                                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                                        {
                                            j = j < 0 ? 0 : j;
                                            cell = row.GetCell(j);
                                            if (cell == null)
                                            {
                                                dataRow[j] = "";
                                            }
                                            else
                                            {
                                                dataRow[j] = GetValueTypeForXLS(cell);
                                            }
                                        }
                                        dataTable.Rows.Add(dataRow);
                                    }
                                }
                                ds.Tables.Add(dataTable);
                            }
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "导入excel到DataSet");
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }

        /// <summary>
        /// 根据单元格将内容返回为对应类型的数据
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>数据</returns>
        private static object GetValueTypeForXLS(ICell cell)
        {
            if (cell == null)
                return null;

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return "";

                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();

                case CellType.Numeric://数字
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    return cell.NumericCellValue;

                case CellType.Error:
                    return cell.ErrorCellValue.ToString();

                case CellType.Formula:
                    return cell.NumericCellValue;

                case CellType.String:
                default:
                    return cell.StringCellValue;
            }
        }

        /// <summary>
        /// Ole读取数据Excel数据
        /// </summary>
        /// <param name="FileFullPath">绝对的路径哈</param>
        /// <param name="SheetName"></param>
        /// <returns></returns>
        public static System.Data.DataTable GetExcelToDataTableBySheet(string FileFullPath, string SheetName)
        {
            try
            {
                string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + FileFullPath + ";Extended Properties='Excel 12.0; HDR=1; IMEX=1'"; //此连接可以操作xls与.xlsx文件
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataSet ds = new DataSet();
                OleDbDataAdapter odda = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", SheetName), conn);                    //("select * from [Sheet1$]", conn);
                odda.Fill(ds, SheetName);
                conn.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "工资条模板导入"); throw;
            }
        }

        /// <summary>
        /// Ole读取数据Excel数据（林强）
        /// </summary>
        /// <param name="FileFullPath">绝对的路径哈</param>
        /// <param name="SheetName"></param>
        /// <param name="isColumnName">第一行是否作为列名</param>
        /// <returns></returns>
        public static System.Data.DataTable GetExcelToDataTableBySheet(string FileFullPath, string SheetName, bool isColumnName)
        {
            try
            {
                string yesno = "NO";
                if (isColumnName)
                {
                    yesno = "YES";
                }
                string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + FileFullPath + ";Extended Properties='Excel 12.0; HDR=" + yesno + "; IMEX=1'"; //此连接可以操作xls与.xlsx文件
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataSet ds = new DataSet();
                OleDbDataAdapter odda = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", SheetName), conn);                    //("select * from [Sheet1$]", conn);
                odda.Fill(ds, SheetName);
                conn.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                LogRecord.LogCatch(ex, "工资条模板导入"); throw;
            }
        }

        #endregion
        #region//利用winrar解压或者压缩

        /// <summary>
        /// 判断是否安装有winrar-没有提示下载安装
        /// </summary>
        /// <returns></returns>
        public static string ExistsWinRar()
        {
            string result = string.Empty;

            string key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key);
            if (registryKey != null)
            {
                result = registryKey.GetValue("").ToString();
            }
            registryKey.Close();

            return result;
        }

        /// <summary>
        /// 解压RAR和ZIP文件(需存在Winrar.exe(只要自己电脑上可以解压或压缩文件就存在Winrar.exe))-支持winrar支持得格式
        /// </summary>
        /// <param name="UnPath">解压后文件保存目录</param>
        /// <param name="rarPathName">待解压文件存放绝对路径（包括文件名称）</param>
        /// <param name="IsCover">所解压的文件是否会覆盖已存在的文件(如果不覆盖,所解压出的文件和已存在的相同名称文件不会共同存在,只保留原已存在文件)</param>
        /// <param name="PassWord">解压密码(如果不需要密码则为空)</param>
        /// <returns>true(解压成功);false(解压失败)</returns>
        public static bool UnRarOrZip(ref string UnPath, string rarPathName, bool IsCover, string PassWord)
        {
            //rarPathName = HttpContext.Current.Server.MapPath(rarPathName);
            System.IO.FileInfo file = new FileInfo(rarPathName);

            UnPath = UnPath.Trim() == "" ? "/file/CompressedFile" : UnPath;
            UnPath = HttpContext.Current.Server.MapPath(UnPath + "/" + file.Name.Substring(0, file.Name.Length - file.Extension.Length) + "/");

            if (!Directory.Exists(UnPath))
                Directory.CreateDirectory(UnPath);
            else
            {
                //如果存在解压就停止
                return true;
            }
            Process Process1 = new Process();
            Process1.StartInfo.FileName = "Winrar.exe";
            Process1.StartInfo.CreateNoWindow = true;
            string cmd = "";
            if (!string.IsNullOrEmpty(PassWord) && IsCover)
                //解压加密文件且覆盖已存在文件( -p密码 )
                cmd = string.Format(" x -p{0} -o+ {1} {2} -y", PassWord, rarPathName, UnPath);
            else if (!string.IsNullOrEmpty(PassWord) && !IsCover)
                //解压加密文件且不覆盖已存在文件( -p密码 )
                cmd = string.Format(" x -p{0} -o- {1} {2} -y", PassWord, rarPathName, UnPath);
            else if (IsCover)
                //覆盖命令( x -o+ 代表覆盖已存在的文件)
                cmd = string.Format(" x -o+ {0} {1} -y", rarPathName, UnPath);
            else
                //不覆盖命令( x -o- 代表不覆盖已存在的文件)
                cmd = string.Format(" x -o- {0} {1} -y", rarPathName, UnPath);
            //命令
            Process1.StartInfo.Arguments = cmd;
            Process1.Start();
            Process1.WaitForExit();//无限期等待进程 winrar.exe 退出
                                   //Process1.ExitCode==0指正常执行，Process1.ExitCode==1则指不正常执行
            if (Process1.ExitCode == 0)
            {
                Process1.Close();
                return true;
            }
            else
            {
                Process1.Close();
                return false;
            }
        }

        /// <summary>
        /// 压缩文件成RAR或ZIP文件(需存在Winrar.exe(只要自己电脑上可以解压或压缩文件就存在Winrar.exe))
        /// </summary>
        /// <param name="filesPath">将要压缩的文件夹或文件的绝对路径</param>
        /// <param name="rarPathName">压缩后的压缩文件保存绝对路径（包括文件名称）</param>
        /// <param name="IsCover">所压缩文件是否会覆盖已有的压缩文件(如果不覆盖,所压缩文件和已存在的相同名称的压缩文件不会共同存在,只保留原已存在压缩文件)</param>
        /// <param name="PassWord">压缩密码(如果不需要密码则为空)</param>
        /// <returns>true(压缩成功);false(压缩失败)</returns>
        public static bool CondenseRarOrZip(string filesPath, string rarPathName, bool IsCover, string PassWord)
        {
            string rarPath = Path.GetDirectoryName(rarPathName);
            if (!Directory.Exists(rarPath))
                Directory.CreateDirectory(rarPath);
            Process Process1 = new Process();
            Process1.StartInfo.FileName = "Winrar.exe";
            Process1.StartInfo.CreateNoWindow = true;
            string cmd = "";
            if (!string.IsNullOrEmpty(PassWord) && IsCover)
                //压缩加密文件且覆盖已存在压缩文件( -p密码 -o+覆盖 )
                cmd = string.Format(" a -ep1 -p{0} -o+ {1} {2} -r", PassWord, rarPathName, filesPath);
            else if (!string.IsNullOrEmpty(PassWord) && !IsCover)
                //压缩加密文件且不覆盖已存在压缩文件( -p密码 -o-不覆盖 )
                cmd = string.Format(" a -ep1 -p{0} -o- {1} {2} -r", PassWord, rarPathName, filesPath);
            else if (string.IsNullOrEmpty(PassWord) && IsCover)
                //压缩且覆盖已存在压缩文件( -o+覆盖 )
                cmd = string.Format(" a -ep1 -o+ {0} {1} -r", rarPathName, filesPath);
            else
                //压缩且不覆盖已存在压缩文件( -o-不覆盖 )
                cmd = string.Format(" a -ep1 -o- {0} {1} -r", rarPathName, filesPath);
            //命令
            Process1.StartInfo.Arguments = cmd;
            Process1.Start();
            Process1.WaitForExit();//无限期等待进程 winrar.exe 退出
                                   //Process1.ExitCode==0指正常执行，Process1.ExitCode==1则指不正常执行
            if (Process1.ExitCode == 0)
            {
                Process1.Close();
                return true;
            }
            else
            {
                Process1.Close();
                return false;
            }
        }

        #endregion
        #region//小写转换大写钱
        private const string DXSZ = "零壹贰叁肆伍陆柒捌玖";
        private const string DXDW = "毫厘分角元拾佰仟萬拾佰仟亿拾佰仟萬兆拾佰仟萬亿京拾佰仟萬亿兆垓";
        private const string SCDW = "元拾佰仟萬亿京兆垓";

        /// <summary>
        /// 转换整数为大写金额
        /// <param name="capValue">整数值</param>
        /// <returns>返回大写金额</returns>
        private static string ConvertIntToUppercaseAmount(string capValue)
        {
            string currCap = "";    //当前金额
            string capResult = "";  //结果金额
            string currentUnit = "";//当前单位
            string resultUnit = ""; //结果单位
            int prevChar = -1;      //上一位的值
            int currChar = 0;       //当前位的值
            int posIndex = 4;       //位置索引，从"元"开始
            if (Convert.ToDouble(capValue) == 0) return "";
            for (int i = capValue.Length - 1; i >= 0; i--)
            {
                currChar = Convert.ToInt16(capValue.Substring(i, 1));
                if (posIndex > 30)
                {
                    //已超出最大精度"垓"。注：可以将30改成22，使之精确到兆亿就足够了
                    break;
                }
                else if (currChar != 0)
                {
                    //当前位为非零值，则直接转换成大写金额
                    currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
                }
                else
                {
                    //防止转换后出现多余的零,例如：3000020
                    switch (posIndex)
                    {
                        case 4: currCap = "元"; break;
                        case 8: currCap = "萬"; break;
                        case 12: currCap = "亿"; break;
                        case 17: currCap = "兆"; break;
                        case 23: currCap = "京"; break;
                        case 30: currCap = "垓"; break;
                        default: break;
                    }
                    if (prevChar != 0)
                    {
                        if (currCap != "")
                        {
                            if (currCap != "元") currCap += "零";
                        }
                        else
                        {
                            currCap = "零";
                        }
                    }
                }
                //对结果进行容错处理
                if (capResult.Length > 0)
                {
                    resultUnit = capResult.Substring(0, 1);
                    currentUnit = DXDW.Substring(posIndex, 1);
                    if (SCDW.IndexOf(resultUnit) > 0)
                    {
                        if (SCDW.IndexOf(currentUnit) > SCDW.IndexOf(resultUnit))
                        {
                            capResult = capResult.Substring(1);
                        }
                    }
                }
                capResult = currCap + capResult;
                prevChar = currChar;
                posIndex += 1;
                currCap = "";
            }
            return capResult;
        }

        /// <summary>
        /// 转换小数为大写金额
        /// </summary>
        /// <param name="capValue">小数值</param>
        /// <param name="addZero">是否增加零位</param>
        /// <returns>返回大写金额</returns>
        private static string ConvertDecToUppercaseAmount(string capValue, bool addZero)
        {
            string currCap = "";
            string capResult = "";
            int prevChar = addZero ? -1 : 0;
            int currChar = 0;
            int posIndex = 3;
            if (Convert.ToInt16(capValue) == 0) return "";
            for (int i = 0; i < capValue.Length; i++)
            {
                currChar = Convert.ToInt16(capValue.Substring(i, 1));
                if (currChar != 0)
                {
                    currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
                }
                else
                {
                    if (Convert.ToInt16(capValue.Substring(i)) == 0)
                    {
                        break;
                    }
                    else if (prevChar != 0)
                    {
                        currCap = "零";
                    }
                }
                capResult += currCap;
                prevChar = currChar;
                posIndex -= 1;
                currCap = "";
            }
            return capResult;
        }

        ///<summary>
        /// 人民币大写金额
        /// </summary>
        /// <param name="value">人民币数字金额值</param>
        /// <returns>返回人民币大写金额</returns>
        public static string DaXieMoney(decimal? value)
        {
            if (value == 0)
            {
                return "零元整";
            }
            string capResult = "";
            string capValue = string.Format("{0:f4}", value);
            //格式化
            int dotPos = capValue.IndexOf(".");
            //小数点位置
            bool addInt = (Convert.ToInt32(capValue.Substring(dotPos + 1)) == 0);//是否在结果中加"整"
            bool addMinus = (capValue.Substring(0, 1) == "-");      //是否在结果中加"负"
            int beginPos = addMinus ? 1 : 0;
            //开始位置
            string capInt = capValue.Substring(beginPos, dotPos);
            //整数
            string capDec = capValue.Substring(dotPos + 1);
            //小数
            if (dotPos > 0)
            {
                capResult = ConvertIntToUppercaseAmount(capInt) + ConvertDecToUppercaseAmount(capDec, Convert.ToDecimal(capInt) != 0 ? true : false);
            }
            else
            {
                capResult = ConvertIntToUppercaseAmount(capDec);
            }
            if (addMinus) capResult = "负" + capResult;
            if (addInt) capResult += "整"; return capResult;
        }

        /// <summary>
        /// 省略小数后面多余的0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string OmitDecimalMoreZero(decimal? value)
        {
            string str = "";
            if (value != null)
            {
                string strWord = value.ToString();
                if (strWord.IndexOf(".") > -1)
                {
                    string[] arr = strWord.Split('.');
                    string strDecimal = arr[1];
                    string word = "";
                    int wordindex = -1;
                    for (int i = strDecimal.Length - 1; i > -1; i--)
                    {
                        word = strDecimal.Substring(i, 1);
                        if (word != "0")
                        {
                            wordindex = i;
                            break;
                        }
                    }
                    str = arr[0] + (wordindex > -1 ? ("." + strDecimal.Substring(0, wordindex + 1)) : "");
                }
                else
                    str = strWord;
            }

            return str;
        }

        #endregion
        #region 获取版本号

        /// <summary>
        /// 得到系统的版本号(处理) 0是带参数的版本号，例：ver=?? 1是版本号
        /// </summary>
        /// <returns></returns>
        public static string[] GetVersion()
        {
            string[] readinfo = new string[] { "", "" };
            try
            {
                using (System.IO.StreamReader streamReader = new StreamReader(HttpContext.Current.Server.MapPath("/version.txt")))
                {
                    readinfo[1] = streamReader.ReadToEnd();
                    streamReader.Dispose();
                    streamReader.Close();
                }
            }
            catch (Exception ex)
            {
                readinfo[1] = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            if (readinfo[1].Trim() == "")
                readinfo[1] = "-1";
            readinfo[0] = "ver=" + readinfo[1];
            return readinfo;
        }

        #endregion
        #region 转换账号格式

        /// <summary>
        /// 转换账号格式
        /// </summary>
        /// <param name="account">账号信息</param>
        /// <param name="num">间隔数</param>
        /// <returns></returns>
        public static string GetAccountFormat(string account, int num, int? knum)
        {
            string str = "";
            account = account == null ? "" : account.Replace(" ", "");
            if (account != "")
                for (int i = 0; i < account.Length; i++)
                {
                    str += account.Substring(i, 1);
                    if (i % num == (num - 1))
                    {
                        if (knum != null && knum > 1)
                        {
                            for (int j = 0; j < knum; j++)
                                str += "&nbsp;";
                        }
                        else
                            str += "&nbsp;";
                    }
                }
            return str;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resp"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<ResponseParams> ResponseParamsToList(ResponseParams resp)
        {
            System.Collections.Generic.List<ResponseParams> list = new System.Collections.Generic.List<ResponseParams>();
            list.Add(resp);
            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resp"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<T> ModelToList<T>(T resp)
        {
            System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
            list.Add(resp);
            return list;
        }

        #endregion
        #region NPOI
        /// <summary>
        /// 设置Npoi导出excel的数据格式
        /// </summary>
        /// <param name="isBold">是否加粗</param>
        /// <param name="isThin">是否有边框</param>
        /// <param name="alignType">文字类型 1居中 2左 3右</param>
        /// <param name="book">表单</param>
        /// <param name="fontsize">文字大小</param>
        /// <param name="fontcolor">文字颜色</param>
        /// <returns></returns>
        public static ICellStyle NpoiSetCellStyle(NPOI.HSSF.UserModel.HSSFWorkbook book, bool isBold, bool isThin, int alignType, object fontsize, object fontcolor)
        {
            ICellStyle cellStyle = null;

            cellStyle = book.CreateCellStyle();
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            if (alignType == 2)
                cellStyle.Alignment = HorizontalAlignment.Left;
            else if (alignType == 3)
                cellStyle.Alignment = HorizontalAlignment.Right;
            else
                cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.WrapText = true;
            //边框
            if (isThin)
            {
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderTop = BorderStyle.Thin;
            }
            HSSFFont font = (HSSFFont)book.CreateFont();
            //文字加粗
            if (isBold)
                font.IsBold = true;
            //文字大小
            if (fontsize != null && Convert.ToInt32(fontsize) > 0)
            {

            }
            //文字颜色
            if (fontcolor != null && Convert.ToInt32(fontcolor) > 0)
            {

            }
            cellStyle.SetFont(font);

            return cellStyle;
        }
        #endregion
    }
}