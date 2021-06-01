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
    public class AdminController : Controller
    {
        BLL.Admins _bAdmins = new BLL.Admins();
        private const string _defaultStrGUID = "00000000000000000000000000000000";
        #region 视图加载
        /// <summary>
        /// 加载首页
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            return View();
        }
        /// <summary>
        /// 加载新增页面（部分页）
        /// </summary>
        /// <returns></returns>
        public PartialViewResult InsertPartial()
        {
            return PartialView();
        }
        /// <summary>
        /// 加载编辑页面（部分页）
        /// </summary>
        /// <param name="primaryKey">主键值</param>
        /// <returns></returns>
        public PartialViewResult EditPartial(string primaryKey)
        {
            Model.Admins model = null;
            if (string.IsNullOrWhiteSpace(primaryKey) && model == null)
                model = new Model.Admins();
            else
                model = new BLL.Admins().SelectAdminsByGUID(primaryKey);
            return PartialView(model);
        }
        /// <summary>
        /// 加载重置密码页面（部分页）
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public PartialViewResult ResetPwdPartial(string primaryKey)
        {
            Model.Admins model = new Model.Admins
            {
                AdminGUID = new Guid(primaryKey)
            };
            return PartialView(model);
        }
        #endregion 视图加载

        #region Ajax操作请求
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public string Page(ReqQueryAdminList reqModel)
        {
            DataList<Model.Admins> rspModel = _bAdmins.SelectAdmins(reqModel);
            return SerializeHelper.JsonSerialize(rspModel);
        }
        /// <summary>
        /// 执行新增
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public string Insert(Model.Admins reqModel)
        {
            Tuple<bool, string> result = _bAdmins.InsertAdmins(reqModel);
            return SerializeHelper.JsonSerialize(new ResponseParams
            {
                success = result.Item1,
                message = result.Item2
            });
        }

        /// <summary>
        /// 执行更新
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public string Edit(Model.Admins reqModel)
        {
            Tuple<bool, string> result = _bAdmins.UpdateAdmins(reqModel);
            return SerializeHelper.JsonSerialize(new ResponseParams
            {
                success = result.Item1,
                message = result.Item2
            });
        }

        /// <summary>
        /// 执行删除
        /// </summary>
        /// <param name="primaryKey">主键值</param>
        /// <returns></returns>
        public string Delete(string primaryKey)
        {
            var result = _bAdmins.DeleteAdmins(primaryKey);
            return SerializeHelper.JsonSerialize(new ResponseParams
            {
                success = result.Item1,
                message = result.Item2
            });
        }
        /// <summary>
        /// 执行重置密码
        /// </summary>
        /// <param name="primaryKey">主键值</param>
        /// <returns></returns>
        public string ResetPwd(Model.Admins reqModel)
        {
            var result = _bAdmins.ResetPwd(reqModel);
            return SerializeHelper.JsonSerialize(new ResponseParams
            {
                success = result.Item1,
                message = result.Item2
            });
        }
        /// <summary>
        /// 验证新增时的用户名是否可用
        /// </summary>
        /// <param name="adminID">登录账号</param>
        /// <returns></returns>
        public string CheckInsertAdminID(string adminID)
        {
            bool result = !_bAdmins.ExistsAdminID(adminID);
            return SerializeHelper.JsonSerialize(new { valid = result });
        }
        /// <summary>
        /// 验证编辑时的用户名是否可用
        /// </summary>
        /// <param name="adminGUID">管理员GUID</param>
        /// <param name="adminID">登录账号</param>
        /// <returns></returns>
        public string CheckEditAdminID(Guid adminGUID, string adminID)
        {
            bool result = !_bAdmins.ExistsAdminID(adminGUID, adminID);
            return SerializeHelper.JsonSerialize(new { valid = result });
        }
        #endregion  Ajax操作请求
    }
}