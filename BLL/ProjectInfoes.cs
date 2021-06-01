using Model;
using Model.ReqModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProjectInfoes
    {
        DAL.ProjectInfoes _dal = new DAL.ProjectInfoes();
        public Model.ProjectInfoes SelectProjectInfoesByID(string id)
        {
            return _dal.SelectProjectInfoesByID(id);
        }
        public DataList<Model.ProjectInfoes> SelectProjectInfoes(ReqProjectInfoes reqModel)
        {
            reqModel.StrWhere = " where 1=1 ";
            if (!string.IsNullOrEmpty(reqModel.BeginTime))
            {
                reqModel.StrWhere += " and Infodate > '" + reqModel.BeginTime + " 00:00:00'";
            }
            if (!string.IsNullOrEmpty(reqModel.EndTime))
            {
                reqModel.StrWhere += " and Infodate < '" + reqModel.EndTime + " 23:59:59'";
            }
            if (!string.IsNullOrEmpty(reqModel.Category))
            {
                reqModel.StrWhere += " and Categorynum = " + reqModel.Category;
            }
            if (!string.IsNullOrEmpty(reqModel.Title))
            {
                reqModel.StrWhere += " and Title like '%" + reqModel.Title + "%'";
            }
            if (!string.IsNullOrEmpty(reqModel.SuccessfulName))
            {
                reqModel.StrWhere += " and SuccessfulName like '%" + reqModel.SuccessfulName + "%'";
            }
            return _dal.SelectProjectInfoes(reqModel);
        }
    }
}
