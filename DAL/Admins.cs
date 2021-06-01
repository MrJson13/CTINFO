using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model;
using Model.ReqModel;

namespace DAL
{
    public class Admins : BaseDataAccess
    {
        public Admins() : base(Common.DBAccess.DBName.Admins.ToString())
        { }
        /// <summary>
        /// 登陆检查
        /// </summary>
        /// <param name="sID">sID</param>
        /// <returns></returns>
        public Model.Admins CheckUser(string uid)
        {
            try
            {
                Model.Admins ea = new Model.Admins();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("AdminID", uid);
                using (SqlDataReader sdr = SqlData.SelectDataReader(DBAccess.DBName.Admins.ToString(), "Admins_SelectByID", dic))
                {
                    while (sdr.Read())
                    {
                        ea.AdminGUID = Guid.Parse(sdr["AdminGUID"].ToString());
                        ea.AdminID = sdr["AdminID"].ToString();
                        ea.AdminPSW = sdr["AdminPSW"].ToString();
                        ea.AdminName = sdr["AdminName"].ToString();
                        ea.AddTime = Convert.ToDateTime(sdr["AddTime"].ToString());
                    }
                    return ea;
                }
            }
            catch { throw; }
        }

        /*jt edit 2017-11-24*/
        /// <summary>
        /// 通过adminGuid删除
        /// </summary>
        /// <param name="adminGuid">管理员guid</param>
        /// <returns></returns>
        public virtual int DeleteByAdminGUID(string adminGuid)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("AdminGUID", adminGuid);
            return SqlData.InsDelUpdDataOutput(DBAccess.DBName.Admins.ToString(), "Admins_DeleteByGUID", dic);
        }
        /// <summary>
        /// 新增管理员
        /// </summary>
        /// <param name="adminGuid">管理员guid</param>
        /// <returns></returns>
        public virtual int InsertAdmins(Model.Admins entity)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("AdminGUID", entity.AdminGUID.ToByteArray());
            dic.Add("AdminID", entity.AdminID);
            dic.Add("AdminPSW", entity.AdminPSW);
            dic.Add("AdminName", entity.AdminName);
            return SqlData.InsDelUpdData(DBAccess.DBName.Admins.ToString(), "Admins_Insert", dic);
        }

        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.Admins SelectAdminsByGUID(string guid)
        {
            try
            {
                Model.Admins mdl = null;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("AdminGUID", guid);
                using (SqlDataReader sdr = SqlData.SelectDataReader(DBAccess.DBName.Admins.ToString(), "Admins_SelectByGUID", dic))
                {
                    if (sdr != null && sdr.HasRows && sdr.Read())
                    {
                        mdl = CreateModelByDataReader(sdr);
                    }
                    return mdl;
                }
            }
            catch { throw; }
        }
        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="readr"></param>
        /// <returns></returns>
        private Model.Admins CreateModelByDataReader(IDataReader readr)
        {
            Model.Admins entity = new Model.Admins();
            entity.AdminGUID = new Guid(readr["AdminGUID"].ToString());
            entity.AdminID = readr["AdminID"].ToString();
            entity.AdminPSW = readr["AdminPSW"].ToString();
            entity.AdminName = readr["AdminName"].ToString();
            entity.AddTime = DateTime.Parse(readr["AddTime"].ToString());
            if (readr["IsDeleted"] != DBNull.Value)
            {
                entity.IsDeleted = bool.Parse(readr["IsDeleted"].ToString());
            }
            if (readr["IsSupper"] != DBNull.Value)
            {
                entity.IsSupper = bool.Parse(readr["IsSupper"].ToString());
            }
            return entity;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mdl"></param>
        /// <returns></returns>
        public int UpdateAdmins(Model.Admins mdl)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("AdminGUID", mdl.AdminGUID);
            dic.Add("AdminID", mdl.AdminID);
            //dic.Add("AdminPSW", mdl.AdminPSW);
            dic.Add("AdminName", mdl.AdminName);
            return SqlData.InsDelUpdData(DBAccess.DBName.Admins.ToString(), "Admins_Update", dic);
        }
        /// <summary>
        /// 分页查询管理员列表
        /// </summary>
        /// <param name="reqModel">查询参数</param>
        /// <returns></returns>
        public DataList<Model.Admins> SelectAdmins(ReqQueryAdminList reqModel)
        {
            DataList<Model.Admins> list = new DataList<Model.Admins>();
            int[] _pageStr = Pagination.CountStartEnd(reqModel.PageIndex, reqModel.PageSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("start", _pageStr[0]);
            dic.Add("end", _pageStr[1]);
            dic.Add("key", reqModel.AdminName);
            SqlParameter total = new SqlParameter();
            using (SqlDataReader sdr = SqlData.SelectDataReader(DBAccess.DBName.Admins.ToString(), "Admins_SelectPage", dic, out total))
            {
                while (sdr.Read())
                {
                    Model.Admins mdl = CreateModelByDataReader(sdr);
                    list.Rows.Add(mdl);
                }
            }
            list.Total = Convert.ToInt32(total.Value);
            return list;
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public int ResetPwd(Model.Admins reqModel)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("AdminGUID", reqModel.AdminGUID.ToString());
            dic.Add("AdminPSW", reqModel.AdminPSW);
            return SqlData.InsDelUpdData(DBAccess.DBName.Admins.ToString(), "Admins_ResetPwd", dic);
        }

        /// <summary>
        /// 判断指定登录名的管理员是否存在
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public bool ExistsAdminID(string adminId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("AdminID", adminId);
            using (SqlDataReader sdr = SqlData.SelectDataReader(DBAccess.DBName.Admins.ToString(), "Admins_ExistsAdminId", dic))
            {

                string adminGUID = null;
                if (sdr != null && sdr.HasRows && sdr.Read() && sdr["AdminGUID"] != DBNull.Value)
                {
                    adminGUID = sdr["AdminGUID"].ToString();
                }
                return !string.IsNullOrWhiteSpace(adminGUID);
            }
        }
        /// <summary>
        /// 判断指定登录名的管理员是否存在
        /// </summary>
        /// <param name="adminGuid"></param>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public bool ExistsAdminID(Guid adminGuid, string adminId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("AdminGUID", adminGuid);
            dic.Add("AdminID", adminId);
            using (SqlDataReader sdr = SqlData.SelectDataReader(DBAccess.DBName.Admins.ToString(), "Admins_ExistsAdminId", dic))
            {
                string adminGUID = null;
                if (sdr != null && sdr.HasRows && sdr.Read() && sdr["AdminGUID"] != DBNull.Value)
                {
                    adminGUID = sdr["AdminGUID"].ToString();
                }
                return !string.IsNullOrWhiteSpace(adminGUID);
            }
        }

        /*jt edit 2017-11-24*/
    }
}
