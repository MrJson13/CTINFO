using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ReqModel
{
    /// <summary>
    /// 查询管理员列表请求参数
    /// </summary>
    public class ReqQueryAdminList : ReqPageParams
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string AdminName { get; set; }
    }
}
