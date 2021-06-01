using Common;
using Model;
using Model.ReqModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GrabInfo : BaseDataAccess
    {

        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.GrabInfo SelectGrabInfoByID(int id)
        {
            try
            {
                Model.GrabInfo mdl = new Model.GrabInfo();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("GrabID", id);
                using (SqlDataReader sdr = SqlData.SelectDataReader(DBAccess.DBName.Admins.ToString(), "GrabInfo_SelectByID", dic))
                {
                    if (sdr.Read())
                    {
                        mdl.GrabID = id;
                        mdl.ProName = sdr["ProName"].ToString();
                        mdl.ProPrice = sdr["ProPrice"].ToString();
                        mdl.WinCompany = sdr["WinCompany"].ToString();
                        mdl.URL = sdr["URL"].ToString();
                        mdl.CreateTime = DateTime.Parse(sdr["CreateTime"].ToString());
                    }
                    return mdl;
                }
            }
            catch { throw; }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="reqModel">查询参数</param>
        /// <returns></returns>
        public DataList<Model.GrabInfo> SelectGrabInfo(ReqGrabInfo reqModel)
        {
            DataList<Model.GrabInfo> list = new DataList<Model.GrabInfo>();
            int[] _pageStr = Pagination.CountStartEnd(reqModel.PageIndex, reqModel.PageSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("start", _pageStr[0]);
            dic.Add("end", _pageStr[1]);
            dic.Add("key", reqModel.ProName);
            SqlParameter total = new SqlParameter();
            using (SqlDataReader sdr = SqlData.SelectDataReader(DBAccess.DBName.Admins.ToString(), "GrabInfo_SelectPage", dic, out total))
            {
                while (sdr.Read())
                {
                    Model.GrabInfo mdl = new Model.GrabInfo();
                    mdl.GrabID = int.Parse(sdr["GrabID"].ToString());
                    mdl.ProName = sdr["ProName"].ToString();
                    mdl.ProPrice = sdr["ProPrice"].ToString();
                    mdl.WinCompany = sdr["WinCompany"].ToString();
                    mdl.URL = sdr["URL"].ToString();
                    mdl.CreateTime = DateTime.Parse(sdr["CreateTime"].ToString());
                    list.Rows.Add(mdl);
                }
            }
            list.Total = Convert.ToInt32(total.Value);
            return list;
        }
    }
}
