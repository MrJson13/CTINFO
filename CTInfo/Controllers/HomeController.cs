using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTInfo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            HttpCookie hc = BasePageAdmin.GetCookieOfLoginAdmin();
            if (hc != null)
            {
                ViewBag.Name = hc.Values["username"];
                ViewBag.Psw = hc.Values["userpsw"];
            }
            else
            {
                ViewBag.Name = "";
                ViewBag.Psw = "";
            }
            return View();
        }
        public string LoginResult(Model.Admins admin)
        {
            string loginname = admin.AdminID;
            string loginpassword = admin.AdminPSW;
            bool brmb = Request["Remember"] == "on" ? true : false;
            HttpCookie hck = BasePageAdmin.GetCookieOfLoginAdmin();
            BLL.Admins da = new BLL.Admins();
            string res = "";
            if (hck != null && hck.Values["username"] == loginname && hck.Values["userpsw"] == loginpassword)//和cookie相等直接判断
            {
                res = da.CheckUser(loginname, loginpassword, brmb, false);//不加密
            }
            else
            {
                res = da.CheckUser(loginname, loginpassword, brmb, true);//加密
            }
            ResponseParams resp = new ResponseParams();
            if (res == "1")
            {
                resp.success = true;
                resp.message = "登录成功";
            }
            else if (res == "2")
            {
                resp.success = false;
                resp.message = "密码错误";
            }
            else
            {
                resp.success = false;
                resp.message = "用户不存在";
            }
            return SerializeHelper.JsonSerialize(resp);
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            Session.Clear();
            return View("Login");
        }
    }
}