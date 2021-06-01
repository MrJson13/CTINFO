using Common;
using Model;
using Model.ReqModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTInfo.Controllers
{
    public class ProjectInfoesController : Controller
    {
        BLL.ProjectInfoes _bll = new BLL.ProjectInfoes();
        public ViewResult Index()
        {
            return View();
        }
        public ViewResult Detail(string id)
        {
            ViewBag.ProjectInfo = _bll.SelectProjectInfoesByID(id);
            return View();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public string Page(ReqProjectInfoes reqModel)
        {
            DataList<Model.ProjectInfoes> rspModel = _bll.SelectProjectInfoes(reqModel);
            return SerializeHelper.JsonSerialize(rspModel);
        }
    }
}