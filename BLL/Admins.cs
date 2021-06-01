using Common;
using Model;
using Model.ReqModel;
using System;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Admins
    {
        DAL.Admins _dal = new DAL.Admins();
        public Admins()
        { }
        /// <summary>
        /// 登陆检查
        /// </summary>
        /// <param name="uid">sID</param>
        /// <param name="psw">密码</param>
        /// <param name="brmb">是否记住登陆状态</param>
        /// <param name="bencryption">是否加密密码比较(因为cookie中存储的密码已加密,所以如果是cookie传值,则不需要加密比较,否则需要加密比较)</param>
        /// <returns>1:成功;2:密码错误;3.用户名不存在</returns>
        public string CheckUser(string uid, string psw, bool brmb, bool bencryption)
        {

            Model.Admins ea = new Model.Admins();
            ea = _dal.CheckUser(uid);
            string encryptPsw = psw;
            if (ea.AdminID != null && ea.AdminPSW != null)
            {
                if (bencryption) { encryptPsw = FormsAuthentication.HashPasswordForStoringInConfigFile(psw, "md5"); }//加密比较
                #region MD5加密
                //System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
                //byte[] data = md5Hasher.ComputeHash(System.Text.Encoding.Default.GetBytes("123123"));
                //System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                //for (int i = 0; i < data.Length; i++)
                //{
                //    sBuilder.Append(data[i].ToString("x2"));
                //}
                //string aa = sBuilder.ToString().ToUpper();
                #endregion
                if (ea.AdminPSW == encryptPsw)
                {
                    BasePageAdmin.SetSessionOfLoginAdmin(ea);
                    if (brmb == true)
                    {
                        BasePageAdmin.SetCookieOfLoginAdmin(ea);
                    }
                    else
                        BasePageAdmin.ClearCookieOfLoginAdmin();
                    return "1";
                }
                else { return "2"; }
            }
            else { return "3"; }
        }


        #region JT Edit 2017-11-24

        /// <summary>
        /// 删除
        /// <para>逻辑删除，修改删除标记为true</para>
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public Tuple<bool, string> DeleteAdmins(string primaryKey)
        {
            var ouputNum = _dal.DeleteByAdminGUID(primaryKey);
            string msg = string.Empty;
            bool isSucc = ouputNum == 1;
            switch (ouputNum)
            {
                case 1:
                    msg = "删除成功"; break;
                case -1:
                    msg = "删除失败，不能删除超级管理员"; break;
                case 0:
                    msg = "删除失败，没有受影响的行数"; break;
                default:
                    msg = "删除失败，未知的删除参数"; break;
            }
            return new Tuple<bool, string>(isSucc, msg);
        }
        /// <summary>
        /// 新增管理员
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public Tuple<bool, string> InsertAdmins(Model.Admins reqModel)
        {
            reqModel.AdminGUID = System.Guid.NewGuid();
            int count = _dal.InsertAdmins(reqModel);
            return new Tuple<bool, string>(count > 0, count > 0 ? "新增成功" : "新增失败");
        }

        /// <summary>
        /// 根据主键查询一条记录，如果没有则返回默认值
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public Model.Admins SelectAdminsByGUID(string primaryKey)
        {
            return _dal.SelectAdminsByGUID(primaryKey);
        }

        public Tuple<bool, string> UpdateAdmins(Model.Admins reqModel)
        {
            int count = _dal.UpdateAdmins(reqModel);
            return new Tuple<bool, string>(count > 0, count > 0 ? "修改成功" : "修改失败");
        }
        /// <summary>
        /// 分页查询管理员列表
        /// </summary>
        /// <param name="reqModel">查询参数</param>
        /// <returns></returns>
        public DataList<Model.Admins> SelectAdmins(ReqQueryAdminList reqModel)
        {
            return _dal.SelectAdmins(reqModel);
        }

        /// <summary>
        /// 检测除当前guid外的登录账号是否重复
        /// </summary>
        /// <returns></returns>
        public bool ExistsAdminID(Guid adminGuid, string adminId)
        {
            return _dal.ExistsAdminID(adminGuid, adminId);
        }

        /// <summary>
        /// 检测除当前guid外的登录账号是否重复
        /// </summary>
        /// <returns></returns>
        public bool ExistsAdminID(string adminId)
        {
            return _dal.ExistsAdminID(adminId);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public Tuple<bool, string> ResetPwd(Model.Admins reqModel)
        {
            reqModel.AdminPSW = CryptoHelper.Md5(reqModel.AdminPSW);
            int count = _dal.ResetPwd(reqModel);
            return count > 0 ? new Tuple<bool, string>(true, "重置管理员密码成功") : new Tuple<bool, string>(false, "重置管理员密码成功，没有受影响的行");
        }
        #endregion JT Edit
    }
}
