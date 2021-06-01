using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CTInfo.Controllers
{
    public class LoginAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //获取请求的控制器名称和action
            string controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString();
            string actionName = HttpContext.Current.Request.RequestContext.RouteData.Values["Action"].ToString();
            if (actionName != "Login" && actionName != "LoginResult")
            {
                //判断是否登录
                if (filterContext.HttpContext.Session["LoginAdmin"] == null)
                {
                    //ContentResult content = new ContentResult();
                    //content.Content = "<script type='text/javascript'>location.href='/Home/Login';</script>";
                    //filterContext.Result = content;
                    filterContext.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary(new { Controller = "Home", Action = "Login" }));
                }
                //filterContext.Result =
                //    new RedirectToRouteResult(
                //        new RouteValueDictionary(new { Controller = "Home", Action = "Login" }));


            }
            base.OnActionExecuting(filterContext);
        }
    }
}