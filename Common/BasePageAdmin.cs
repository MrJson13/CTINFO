using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;

using Model;

namespace Common
{
    public class BasePageAdmin
    {
        public BasePageAdmin()
        { }

        //protected override void OnLoad(EventArgs e)
        //{
        //    if (Session["LoginAdmin"] == null)
        //    {
        //        Response.Redirect("~/Login.aspx");
        //    }
        //    base.OnLoad(e);
        //}
        /// <summary>
        /// 将后台登录管理员信息存入Session
        /// </summary>
        /// <param name="ecp"></param>
        public static void SetSessionOfLoginAdmin(Model.Admins ea)
        {
            HttpContext.Current.Session["LoginAdmin"] = ea;
            HttpContext.Current.Session.Timeout = Convert.ToInt16(ConfigurationManager.AppSettings["SessionTimeout"].ToString());
        }
        /// <summary>
        /// 将前端登录用户个人信息存入Session
        /// </summary>
        /// <param name="user"></param>
        public static void SetSessionOfLoginUser(Model.Users user)
        {
            HttpContext.Current.Session["LoginUser"] = user;
            //HttpContext.Current.Session.Timeout = Convert.ToInt16(ConfigurationManager.AppSettings["SessionTimeout"].ToString());
        }

        /// <summary>
        /// 取出个人信息Session
        /// </summary>
        public static Model.Admins GetSessionOfLoginAdmin()
        {
            Model.Admins ecp = null;
            if (HttpContext.Current.Session["LoginAdmin"] != null)
            {
                ecp = HttpContext.Current.Session["LoginAdmin"] as Model.Admins;
            }
            return ecp;
        }
        /// <summary>
        /// 取出个人信息Session
        /// </summary>
        public static Model.Users GetSessionOfLoginUser()
        {
            Model.Users ecp = null;
            if (HttpContext.Current.Session["LoginUser"] != null)
            {
                ecp = HttpContext.Current.Session["LoginUser"] as Model.Users;
            }
            return ecp;
        }
        /// <summary>
        /// 清除个人信息Session
        /// </summary>
        public static void ClearSessionOfLoginAdmin()
        {
            HttpContext.Current.Session.Remove("LoginAdmin");
        }
        /// <summary>
        /// 将个人信息存入Cookie
        /// </summary>
        /// <param name="ecp"></param>
        public static void SetCookieOfLoginAdmin(Model.Admins ea)
        {
            HttpCookie ckLoinAdmin = new HttpCookie("LoginAdmin");
            ckLoinAdmin.Values.Add("username", ea.AdminID);
            ckLoinAdmin.Values.Add("userpsw", ea.AdminPSW);
            ckLoinAdmin.Expires = DateTime.Now.AddDays(Convert.ToDouble(ConfigurationManager.AppSettings["CookieTimeout"].ToString()));
            HttpContext.Current.Response.AppendCookie(ckLoinAdmin);

        }
        /// <summary>
        /// 将前端登录用户个人信息存入Cookie
        /// </summary>
        /// <param name="user"></param>
        public static void SetCookieOfLoginUser(Model.Users user)
        {
            HttpCookie ckLoinUser = new HttpCookie("LoginUser");
            ckLoinUser.Values.Add("username", user.LoginName);
            ckLoinUser.Values.Add("userpsw", user.LoginPassword);
            ckLoinUser.Expires = DateTime.Now.AddDays(Convert.ToDouble(ConfigurationManager.AppSettings["CookieTimeout"].ToString()));
            HttpContext.Current.Response.AppendCookie(ckLoinUser);

        }
        /// <summary>
        /// 将前端登录用户个人信息存入Cookie,这个是session过期后，将cookie里面的数据放到session里面
        /// </summary>
        /// <param name="user"></param>
        public static void SetCookieOfLoginUser1(Model.Users user)
        {
            HttpCookie ckLoinUser = new HttpCookie("TempUser_ZhongJiao");
            ckLoinUser.Values.Add("LoginName", user.LoginName);
            ckLoinUser.Values.Add("UserID", user.UserID.ToString());           
            ckLoinUser.Expires = DateTime.Now.AddHours(Convert.ToDouble(ConfigurationManager.AppSettings["CookieTimeoutTemp"].ToString()));
            HttpContext.Current.Response.AppendCookie(ckLoinUser);

        }
        /// <summary>
        /// 获取个人信息Cookie
        /// </summary>
        /// <returns></returns>
        public static HttpCookie GetCookieOfLoginAdmin()
        {
            HttpCookie hck = null;
            HttpRequest req = HttpContext.Current.Request;
            if (req.Cookies["LoginAdmin"] != null)
            {
                hck = req.Cookies["LoginAdmin"];
            }
            return hck;
        }
        /// <summary>
        /// 获取前端个人信息Cookie，这个cookie长期保存
        /// </summary>
        /// <returns></returns>
        public static HttpCookie GetCookieOfLoginUser()
        {
            HttpCookie hck = null;
            HttpRequest req = HttpContext.Current.Request;
            if (req.Cookies["LoginUser"] != null)
            {
                hck = req.Cookies["LoginUser"];
            }
            return hck;
        }
        /// <summary>
        /// 获取前端个人信息Cookie,这个cookie只保存一天
        /// </summary>
        /// <returns></returns>
        public static HttpCookie GetCookieOfLoginUserTemp()
        {
            HttpCookie hck = null;
            HttpRequest req = HttpContext.Current.Request;
            if (req.Cookies["TempUser_ZhongJiao"] != null)
            {
                hck = req.Cookies["TempUser_ZhongJiao"];
            }
            return hck;
        }
        /// <summary>
        /// 清除个人信息Cookie
        /// </summary>
        public static void ClearCookieOfLoginAdmin()
        {
            HttpCookie ckU = HttpContext.Current.Response.Cookies["LoginAdmin"];
            ckU.Expires = DateTime.Now.AddDays(-100);
            HttpContext.Current.Response.Cookies.Add(ckU);
        }
        /// <summary>
        /// 清除前端个人信息Cookie
        /// </summary>
        public static void ClearCookieOfLoginUser()
        {
            HttpCookie ckU = HttpContext.Current.Response.Cookies["LoginUser"];
            ckU.Expires = DateTime.Now.AddDays(-100);
            HttpContext.Current.Response.Cookies.Add(ckU);
        }
        /// <summary>
        /// 通过用户登录账号和权限ID获取权限
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetPowerMenu(string LoginName,int? id) {
            List<Model.Functions> list = new List<Model.Functions>();
            List<string> btn = new List<string>();
            //读取json文件内容
            list = SerializeHelper.JsonDeserialize<List<Model.Functions>>(FileOperate.ReadFile(LoginName));
            if (list != null && id != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ParentID == id)
                    {
                        btn.Add(list[i].BtnID);
                    }
                }
            }
            return string.Join(",", btn.ToArray());
        }
        /// <summary>
        /// 通过用户登录账号和权限ID获取权限,获取权限下面德所有权限
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Model.Functions> GetPowerMenuByID(string LoginName, int? id)
        {
            List<Model.Functions> list = new List<Model.Functions>();
            List<Model.Functions> funs = new List<Model.Functions>();
            //读取json文件内容
            list = SerializeHelper.JsonDeserialize<List<Model.Functions>>(FileOperate.ReadFile(LoginName));
            if (list != null && id != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ParentID == id)
                    {
                        funs.Add(list[i]);
                    }
                }
            }
            return funs;
        }
    }
}