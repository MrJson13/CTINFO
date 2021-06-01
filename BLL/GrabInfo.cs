using Model;
using Model.ReqModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class GrabInfo
    {
        DAL.GrabInfo _dal = new DAL.GrabInfo();
        public Model.GrabInfo SelectGrabInfoByID(int id)
        {
            return _dal.SelectGrabInfoByID(id);
        }
        public DataList<Model.GrabInfo> SelectGrabInfo(ReqGrabInfo reqModel)
        {
            return _dal.SelectGrabInfo(reqModel);
        }
    }
}
