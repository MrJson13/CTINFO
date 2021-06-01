using System;
using System.Text.RegularExpressions;

namespace Common
{
    public class Commonality
    {
        public Commonality()
        { }
        /// <summary>
        /// 判断指定长度的字符串是否为正整数/0
        /// </summary>
        /// <param name="num">要判断的字符串</param>
        /// <param name="maxlength">最大位数(一般Byte:3;Int16:5;Int32:10;Int64:19)</param>
        /// <returns>true:正整数/0</returns>
        public static bool JudgeNumber(string num, byte maxlength)
        {
            bool _result = false;
            if (!string.IsNullOrEmpty(num) && num.Length < maxlength)
            {
                if (Regex.IsMatch(num, @"^\d+$")) { _result = true; } else { _result = false; }
            }
            return _result;
        }
        /// <summary>
        /// 判断指定长度的字符串是否为正数/0
        /// </summary>
        /// <param name="num">要判断的字符串</param>
        /// <param name="maxlength">最大位数</param>
        /// <returns>true:正数/0</returns>
        public static bool JudgeFloat(string num, byte maxlength)
        {
            bool _result = false;
            if (!string.IsNullOrEmpty(num) && num.Length < maxlength)
            {
                if (Regex.IsMatch(num, @"^\d+(\.\d+)?$")) { _result = true; } else { _result = false; }
            }
            return _result;
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="cutcount">要截取的字符串长度</param>
        /// <returns></returns>
        public string CutString(string str, int cutcount)
        {
            if (str.Length > cutcount) { str = str.Substring(0, cutcount); }
            return str;
        }
        /// <summary>
        /// 移出URL中的指定参数
        /// </summary>
        /// <param name="url">原始url</param>
        /// <param name="param">需要移出的参数</param>
        /// <returns></returns>
        public static string RemoveParam(string url, string param)
        {
            if (url.Length < 1) return "";
            int _start = url.IndexOf(param + "=");
            if (_start < 0) return url;
            int _end = url.IndexOf("&", _start);
            if (_end < 0) { return url.Remove(_start - 1); }
            int count = _end - _start + 1;
            return url.Remove(_start, count);
        }
        /// <summary>
        /// 计算年龄(周岁)
        /// </summary>
        /// <param name="borndate">出生日期</param>
        /// <returns></returns>
        public static int GetAge(DateTime borndate)
        {
            int _year = DateTime.Today.Year - borndate.Year;
            if (DateTime.Today.Month < borndate.Month) { _year = _year - 1; }
            else if (DateTime.Today.Month == borndate.Month && DateTime.Today.Day < borndate.Day) { _year = _year - 1; }
            return _year;
        }
        /// <summary>
        /// 根据身份证号获取生日
        /// </summary>
        /// <param name="IdCard"></param>
        /// <returns></returns>
        public static string GetBrithdayFromIdCard(string IdCard)
        {
            string rtn = "1900-01-01";
            if (IdCard.Length == 15)
            {
                rtn = IdCard.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            }
            else if (IdCard.Length == 18)
            {
                rtn = IdCard.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            }
            return rtn;
        }
        ///// <summary>
        ///// 替换特殊字符
        ///// </summary>
        ///// <param name="IdCard"></param>
        ///// <returns></returns>
        //public static string ReplaceString(string content)
        //{
        //    if (!string.IsNullOrEmpty(content))
        //    {
        //        content = content.Replace("&amp;", "&");
        //        content = content.Replace("&lt;", "<");
        //        content = content.Replace("&gt;", ">");
        //        content = content.Replace("&nbsp;", " ");
        //        content = content.Replace("&#39;", "'");
        //        content = content.Replace("&quot;", "\"");
        //        content = content.Replace("<br>", "\n");
        //    }
        //    return content;
        //}
        /// <summary>
        /// 替换特殊字符
        /// </summary>
        /// <param name="IdCard"></param>
        /// <returns></returns>
        public static string ReplaceString(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = content.Replace(" ", "&nbsp;");
                content = content.Replace("\n", "<br>");
            }
            return content;
        }
        /// <summary>
        /// 去除html格式
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string NoHtml(string html)
        {
            //删除脚本
            html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", "",
                RegexOptions.IgnoreCase);
            //删除HTML
            html = Regex.Replace(html, @"<(.[^>]*)>", "",
                RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"([\r\n])[\s]+", "",
                RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(quot|#34);", "\"",
                RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(amp|#38);", "&",
                RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(lt|#60);", "<",
                RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(gt|#62);", ">",
                RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(nbsp|#160);", "   ",
                RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1",
                RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(cent|#162);", "\xa2",
                RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(pound|#163);", "\xa3",
                RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(copy|#169);", "\xa9",
                RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#(\d+);", "",
                RegexOptions.IgnoreCase);

            html.Replace("<", "");
            html.Replace(">", "");
            html.Replace("\r\n", "");
            return html;
        }
        /// <summary>
        /// 静态截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cutcount"></param>
        /// <returns></returns>
        public static string CutString1(string str, int cutcount)
        {
            if (str.Length > cutcount) { str = str.Substring(0, cutcount) + "..."; }
            return str;
        }
        //// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }
        /// <summary>
        /// 获取月份天数
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static int GetDay(DateTime datetime)
        {
            int day = DateTime.DaysInMonth(datetime.Year, datetime.Month);
            return day;
        }
    }
}