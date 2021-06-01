using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class Pagination
    {
        public Pagination()
        { }
        /// <summary>
        /// 计算分页开始/结束条数
        /// </summary>
        /// <param name="_nowpage">当前页</param>
        /// <param name="_perpage">每页条数</param>
        /// <returns></returns>
        public static int[] CountStartEnd(int nowpage, int perpage)
        {
            int[] _arr = new int[2];
            _arr[0] = (nowpage - 1) * perpage + 1;
            _arr[1] = _arr[0] + perpage - 1;
            return _arr;
        }
        /// <summary>
        /// 计算当前实际的最大页数
        /// </summary>
        /// <param name="_total">总条数</param>
        /// <param name="_perpage">每页条数</param>
        /// <returns></returns>
        public static int CountMaxPage(int total, int perpage)
        {
            int _maxpage = 0;
            if (total % perpage == 0) { _maxpage = total / perpage; }
            else { _maxpage = total / perpage + 1; }
            return _maxpage;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="total">总条数</param>
        /// <param name="now">当前页</param>
        /// <param name="per">每页显示条数</param>
        /// <param name="url">页面url</param>
        /// <param name="showpage">显示总页数</param>
        /// <param name="param">参数链接</param>
        /// <returns>分页代码</returns>
        public static string Paging(int total, int now, int per, string url, int showpage, string param)
        {
            total = (total - 1) / per + 1;
            int offset = showpage / 2;
            if (now < 1) now = 1;
            if (now > total) now = total;
            int start = now - offset;
            if (start < 1) start = 1;
            int end = start + showpage - 1;
            if (end > total)
            {
                end = total;
                if (end < showpage) start = 1;
                else start = end - showpage + 1;
            }
            if (total == 1) return "";
            string rs = string.Empty;
            if (now > 1) rs += "<a href=\"" + url + (now - 1) + param + "\" class=\"arrow_up\" style=\"padding-left:20px;\">上一页</a>";
            for (int i = start; i <= end; i++)
            {
                if (i == now)
                {
                    rs += "<span class=\"pageon\">" + i + "</span>";
                }
                else
                {
                    rs += "<a href=\"" + url + i + param + "\">" + i + "</a>";
                }
            }
            if (now < total) rs += "<a href=\"" + url + (now + 1) + param + "\" class=\"arrow_down\" style=\"padding-left:20px;\">下一页</a>";
            return rs;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="total">总条数</param>
        /// <param name="now">当前页</param>
        /// <param name="per">每页显示条数</param>
        /// <param name="url">页面url</param>
        /// <param name="showpage">显示总页数</param>
        /// <param name="param">参数链接</param>
        /// <returns>分页代码</returns>
        public static string PagingOnlyFirstLast(int total, int now, int per, string url, int showpage, string param)
        {
            total = (total - 1) / per + 1;
            int offset = showpage / 2;
            if (now < 1) now = 1;
            if (now > total) now = total;
            int start = now - offset;
            if (start < 1) start = 1;
            int end = start + showpage - 1;
            if (end > total)
            {
                end = total;
                if (end < showpage) start = 1;
                else start = end - showpage + 1;
            }
            if (total == 1) return "";
            string rs = string.Empty;
            if (now > 1) { rs += "<li><a href=\"" + url + (now - 1) + param + "\" class=\"type_but ico_type3\">上一页</a></li>"; }
            else { rs += "<li><span class=\"type_but ico_type3\" style=\"color:#ccc;\">上一页</span></li>"; }
            if (now < total) { rs += "<li><a href=\"" + url + (now + 1) + param + "\" class=\"type_but ico_type4\">下一页</a></li>"; }
            else { rs += "<li><span class=\"type_but ico_type4\" style=\"color:#ccc;\">下一页</span></li>"; }
            return rs;
        }
        /// <summary>
        /// 分页(包含“首页/尾页”按钮)
        /// </summary>
        /// <param name="total">总条数</param>
        /// <param name="now">当前页</param>
        /// <param name="per">每页显示条数</param>
        /// <param name="url">页面url</param>
        /// <param name="showpage">显示总页数</param>
        /// <param name="param">参数链接</param>
        /// <returns>分页代码</returns>
        public static string PagingFirstLast(int total, int now, int per, string url, int showpage, string param)
        {
            total = (total - 1) / per + 1;
            int offset = showpage / 2;
            if (now < 1) now = 1;
            if (now > total) now = total;
            int start = now - offset;
            if (start < 1) start = 1;
            int end = start + showpage - 1;
            if (end > total)
            {
                end = total;
                if (end < showpage) start = 1;
                else start = end - showpage + 1;
            }
            if (total == 1) return "";
            string rs = "", style = "";
            if (now > 1)
            {
                rs += "<li><a href=\"" + url + (1) + param + "\">首页</a></li><li><a href=\"" + url + (now - 1) + param + "\">上一页</a></li>";
            }
            for (int i = start; i <= end; i++)
            {
                if (i == now)
                {
                    style = "style=\"color:#f00;font-weight:bold;\"";
                    rs += "<li><span " + style + ">" + i + "</span></li>";
                }
                else
                {
                    style = "";
                    rs += "<li><a href=\"" + url + i + param + "\" " + style + ">" + i + "</a></li>";
                }
            }
            if (now < total) rs += "<li><a href=\"" + url + (now + 1) + param + "\">下一页</a></li><li><a href=\"" + url + (total) + param + "\">尾页</a></li>";
            return rs;
        }
        /// <summary>
        /// 分页(静态,包含“首页/尾页”按钮)
        /// </summary>
        /// <param name="total">总条数</param>
        /// <param name="now">当前页</param>
        /// <param name="per">每页显示条数</param>
        /// <param name="url">页面url</param>
        /// <param name="showpage">显示总页数</param>
        /// <param name="suffix">链接后缀</param>
        /// <returns>分页代码</returns>
        public static string PagingFirstLastStatic(int total, int now, int per, string url, int showpage, string suffix)
        {
            total = (total - 1) / per + 1;
            int offset = showpage / 2;
            if (now < 1) now = 1;
            if (now > total) now = total;
            int start = now - offset;
            if (start < 1) start = 1;
            int end = start + showpage - 1;
            if (end > total) { end = total; if (end < showpage) start = 1; else start = end - showpage + 1; }
            if (total == 1) return "";
            string rs = "";
            if (now > 1)
            {
                rs += "<a href=\"" + url + (now - 1) + suffix + "\" class=\"prev\">上一页</a>";
                if (start >= 2) { rs += "<a href=\"" + url + 1 + suffix + "\">1</a>"; if (start > 2) { rs += "<span>...</span>"; } }
            }
            else { rs += "<span class=\"current prev\">上一页</span>"; }
            for (int i = start; i <= end; i++)
            {
                if (i == now) { rs += "<span class=\"current\">" + i + "</span>"; }
                else { rs += "<a href=\"" + url + i + suffix + "\">" + i + "</a>"; }
            }
            if (now < total)
            {
                if (end <= total - 1) { if (end < total - 1) { rs += "<span>...</span>"; } rs += "<a href=\"" + url + total + suffix + "\">" + total + "</a>"; }
                rs += "<a href=\"" + url + (now + 1) + suffix + "\" class=\"next\">下一页</a>";
            }
            else { rs += "<span class=\"current next\">下一页</span>"; }
            return rs;
        }
        /// <summary>
        /// 分页(包含“首页/尾页”按钮)+选中样式控制
        /// </summary>
        /// <param name="total">总条数</param>
        /// <param name="now">当前页</param>
        /// <param name="per">每页显示条数</param>
        /// <param name="url">页面url 可以指定参数0页码 1给定参数</param>
        /// <param name="showpage">显示总页数</param>
        /// <param name="param">参数链接</param>
        /// <param name="selectedclass">选中样式</param>
        /// <returns>分页代码</returns>
        public static string PagingFirstLast(int total, int now, int per, string url, int showpage, string param, string selectedclass)
        {
            total = (total - 1) / per + 1;
            int offset = showpage / 2;
            if (now < 1) now = 1;
            if (now > total) now = total;
            int start = now - offset;
            if (start < 1) start = 1;
            int end = start + showpage - 1;
            if (end > total)
            {
                end = total;
                if (end < showpage) start = 1;
                else start = end - showpage + 1;
            }
            if (total == 1) return "";
            string rs = "";
            if (now > 1)
            {
                rs += string.Format("<li><a href=\"" + url + "\">首页</a></li>", 1, param);//0 页码 1参数
                rs += string.Format("<li><a href=\"" + url + "\">上一页</a></li>", (now - 1), param);//0 页码 1参数
            }
            for (int i = start; i <= end; i++)
            {
                if (i == now)
                {
                    rs += string.Format("<li class=\"" + selectedclass + "\"><span >" + i + "</span></li>");
                }
                else
                {

                    rs += string.Format("<li><a href=\"" + url + "\" >" + i + "</a></li>", i, param);//0 页码 1参数
                }
            }
            if (now < total)
            {
                rs += string.Format("<li><a href=\"" + url + "\">下一页</a></li>", (now + 1), param);//0 页码 1参数
                rs += string.Format("<li><a href=\"" + url + "\">尾页</a></li>", (total), param);//0 页码 1参数
            }
            return rs;
        }

    }
}