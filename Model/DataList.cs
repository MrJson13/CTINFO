using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DataList<T> where T : new()
    {
        private int _total;
        //private int _integraltotle;
        private List<T> _rows;
        /// <summary>
        /// 数据总条数
        /// </summary>
        [Newtonsoft.Json.JsonProperty("total")]
        public int Total
        {
            get { return _total; }
            set { _total = value; }
        }
        /// <summary>
        /// 主要用于数据库输出第二个数字类型的参数，已在档案管理使用
        /// </summary>
        public int Total2 { get; set; }
        /// <summary>
        /// 钱币显示（图标时使用）
        /// </summary>
        public decimal? Money { get; set; }
        /// <summary>
        /// 总数量（图标时使用）
        /// </summary>
        public int? TotalCount { get; set; }
        /// <summary>
        /// 统计-工作管理
        /// </summary>
        public string Statistical { get; set; }
        /// <summary>
        /// 数据行
        /// </summary>
        [Newtonsoft.Json.JsonProperty("rows")]
        public List<T> Rows
        {
            get
            {
                if (_rows == null) { _rows = new List<T>(); }
                return _rows;
            }
            set { _rows = value; }
        }
    }
}
