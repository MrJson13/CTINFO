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
    public class ProjectInfoes : BaseDataAccess
    {
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.ProjectInfoes SelectProjectInfoesByID(string id)
        {
            try
            {
                Model.ProjectInfoes model = new Model.ProjectInfoes();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Id", id);
                using (SqlDataReader sdr = SqlData.SelectDataReader(DBAccess.DBName.Admins.ToString(), "ProjectInfoes_SelectByID", dic))
                {
                    if (sdr.Read())
                    {
                        model.Id = id;
                        if (sdr["Sysclicktimes"] != DBNull.Value)
                            model.Sysclicktimes = Convert.ToInt32(sdr["Sysclicktimes"].ToString());
                        if (sdr["Title"] != DBNull.Value)
                            model.Title = sdr["Title"].ToString();
                        if (sdr["Content"] != DBNull.Value)
                            model.Content = sdr["Content"].ToString();
                        if (sdr["Webdate"] != DBNull.Value)
                            model.Webdate = Convert.ToDateTime(sdr["Webdate"].ToString());
                        if (sdr["Syscategory"] != DBNull.Value)
                            model.Syscategory = sdr["Syscategory"].ToString();
                        if (sdr["Syscollectguid"] != DBNull.Value)
                            model.Syscollectguid = sdr["Syscollectguid"].ToString();
                        if (sdr["Linkurl"] != DBNull.Value)
                            model.Linkurl = sdr["Linkurl"].ToString();
                        if (sdr["Sysscore"] != DBNull.Value)
                            model.Sysscore = sdr["Sysscore"].ToString();
                        if (sdr["Infodate"] != DBNull.Value)
                            model.Infodate = Convert.ToDateTime(sdr["Infodate"].ToString());
                        if (sdr["Details"] != DBNull.Value)
                            model.Details = sdr["Details"].ToString();
                        if (sdr["Project"] != DBNull.Value)
                            model.Project = sdr["Project"].ToString();
                        if (sdr["PublicityPeriod"] != DBNull.Value)
                            model.PublicityPeriod = sdr["PublicityPeriod"].ToString();
                        if (sdr["SuccessfulName"] != DBNull.Value)
                            model.SuccessfulName = sdr["SuccessfulName"].ToString();
                        if (sdr["SuccessfulPrice"] != DBNull.Value)
                            model.SuccessfulPrice = sdr["SuccessfulPrice"].ToString();
                        if (sdr["SuccessfulReviewPrice"] != DBNull.Value)
                            model.SuccessfulReviewPrice = sdr["SuccessfulReviewPrice"].ToString();
                        if (sdr["BreakNo"] != DBNull.Value)
                            model.BreakNo = Convert.ToInt32(sdr["BreakNo"].ToString());
                        if (sdr["TenderSectionName"] != DBNull.Value)
                            model.TenderSectionName = sdr["TenderSectionName"].ToString();
                        if (sdr["Categorynum"] != DBNull.Value)
                            model.Categorynum = sdr["Categorynum"].ToString();
                    }
                    return model;
                }
            }
            catch { throw; }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="reqModel">查询参数</param>
        /// <returns></returns>
        public DataList<Model.ProjectInfoes> SelectProjectInfoes(ReqProjectInfoes reqModel)
        {
            DataList<Model.ProjectInfoes> list = new DataList<Model.ProjectInfoes>();
            int[] _pageStr = Pagination.CountStartEnd(reqModel.PageIndex, reqModel.PageSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("start", _pageStr[0]);
            dic.Add("end", _pageStr[1]);
            dic.Add("strWhere", reqModel.StrWhere);
            SqlParameter total = new SqlParameter();
            using (SqlDataReader sdr = SqlData.SelectDataReader(DBAccess.DBName.Admins.ToString(), "ProjectInfoes_SelectPage", dic, out total))
            {
                while (sdr.Read())
                {
                    Model.ProjectInfoes model = new Model.ProjectInfoes();
                    if (sdr["Id"] != DBNull.Value)
                        model.Id = sdr["Id"].ToString();
                    if (sdr["Sysclicktimes"] != DBNull.Value)
                        model.Sysclicktimes = Convert.ToInt32(sdr["Sysclicktimes"].ToString());
                    if (sdr["Title"] != DBNull.Value)
                        model.Title = sdr["Title"].ToString();
                    if (sdr["Content"] != DBNull.Value)
                        model.Content = sdr["Content"].ToString();
                    if (sdr["Webdate"] != DBNull.Value)
                        model.Webdate = Convert.ToDateTime(sdr["Webdate"].ToString());
                    if (sdr["Syscategory"] != DBNull.Value)
                        model.Syscategory = sdr["Syscategory"].ToString();
                    if (sdr["Syscollectguid"] != DBNull.Value)
                        model.Syscollectguid = sdr["Syscollectguid"].ToString();
                    if (sdr["Linkurl"] != DBNull.Value)
                        model.Linkurl = sdr["Linkurl"].ToString();
                    if (sdr["Sysscore"] != DBNull.Value)
                        model.Sysscore = sdr["Sysscore"].ToString();
                    if (sdr["Infodate"] != DBNull.Value)
                        model.Infodate = Convert.ToDateTime(sdr["Infodate"].ToString());
                    if (sdr["Details"] != DBNull.Value)
                        model.Details = sdr["Details"].ToString();
                    if (sdr["Project"] != DBNull.Value)
                        model.Project = sdr["Project"].ToString();
                    if (sdr["PublicityPeriod"] != DBNull.Value)
                        model.PublicityPeriod = sdr["PublicityPeriod"].ToString();
                    if (sdr["SuccessfulName"] != DBNull.Value)
                        model.SuccessfulName = sdr["SuccessfulName"].ToString();
                    if (sdr["SuccessfulPrice"] != DBNull.Value)
                        model.SuccessfulPrice = sdr["SuccessfulPrice"].ToString();
                    if (sdr["SuccessfulReviewPrice"] != DBNull.Value)
                        model.SuccessfulReviewPrice = sdr["SuccessfulReviewPrice"].ToString();
                    if (sdr["BreakNo"] != DBNull.Value)
                        model.BreakNo = Convert.ToInt32(sdr["BreakNo"].ToString());
                    if (sdr["TenderSectionName"] != DBNull.Value)
                        model.TenderSectionName = sdr["TenderSectionName"].ToString();
                    if (sdr["Categorynum"] != DBNull.Value)
                        model.Categorynum = sdr["Categorynum"].ToString();
                    list.Rows.Add(model);
                }
            }
            list.Total = Convert.ToInt32(total.Value);
            return list;
        }
    }
}
