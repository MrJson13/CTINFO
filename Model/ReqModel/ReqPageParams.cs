using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ReqModel
{
    /// <summary>
    /// 分页请求参数类
    /// </summary>
    public class ReqPageParams
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 权限ID
        /// </summary>
        public int FunctionID { get; set; }
        /// <summary>
        /// 登录人
        /// </summary>
        public int LoginUser { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string Orderby { get; set; }
        /// <summary>
        /// 是否查询所有
        /// </summary>
        public bool IsAll { get; set; }
    }
}
