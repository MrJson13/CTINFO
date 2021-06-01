using System;
using System.IO;
using System.Text;
using System.Web;

namespace Common
{
    public class LogRecord
    {
        public LogRecord() { }
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        public static void LogCatch(Exception ex)
        {
            try
            {
                DateTime _time = DateTime.Now;
                string _logpath = HttpContext.Current.Server.MapPath("~/log/" + _time.ToString("yyyyMMdd") + ".txt");
                StringBuilder _log = new StringBuilder();
                if (File.Exists(_logpath))
                {
                    _log.Append(File.ReadAllText(_logpath, Encoding.UTF8));
                    _log.Append("\r\n\r\n");
                }
                _log.Append("异常时间:" + _time + "\r\n异常方法:" + ex.TargetSite + "\r\n异常编码:" + ex.HResult.ToString() + "\r\n异常消息:" + ex.Message);
                File.WriteAllText(_logpath, _log.ToString());
            }
            catch { throw; }
        }
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="operateDes">操作类型描述</param>
        public static void LogCatch(Exception ex, string operateDes)
        {
            try
            {
                DateTime _time = DateTime.Now;
                string _logpath = HttpContext.Current.Server.MapPath("~/log/" + _time.ToString("yyyyMMdd") + ".txt");
                StringBuilder _log = new StringBuilder();
                if (File.Exists(_logpath))
                {
                    _log.Append(File.ReadAllText(_logpath, Encoding.UTF8));
                    _log.Append("\r\n\r\n");
                }
                _log.Append("异常时间:" + _time + "\r\n操作类型:" + operateDes + "\r\n异常方法:" + ex.TargetSite + "\r\n异常编码:" + ex.HResult.ToString() + "\r\n异常消息:" + ex.Message);
                File.WriteAllText(_logpath, _log.ToString());
            }
            catch { throw; }
        }
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="operateDes">操作类型描述</param>
        public static void LogCatch(string operateDes)
        {
            try
            {
                DateTime _time = DateTime.Now;
                string _logpath = HttpContext.Current.Server.MapPath("~/log/" + _time.ToString("yyyyMMdd") + ".txt");
                StringBuilder _log = new StringBuilder();
                if (File.Exists(_logpath))
                {
                    _log.Append(File.ReadAllText(_logpath, Encoding.UTF8));
                    _log.Append("\r\n\r\n");
                }
                _log.Append("异常时间:" + _time + "\r\n异常提示:" + operateDes);
                File.WriteAllText(_logpath, _log.ToString());
            }
            catch { throw; }
        }
    }
}