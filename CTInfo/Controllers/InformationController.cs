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
    public class InformationController : Controller
    {
        BLL.GrabInfo _bll = new BLL.GrabInfo();
        public ViewResult Index()
        {
            return View();
        }
        public ViewResult Detail(int id)
        {
            ViewBag.GrabInfo= _bll.SelectGrabInfoByID(id);
            return View();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public string Page(ReqGrabInfo reqModel)
        {
            DataList<Model.GrabInfo> rspModel = _bll.SelectGrabInfo(reqModel);
            return SerializeHelper.JsonSerialize(rspModel);
        }
    }
}