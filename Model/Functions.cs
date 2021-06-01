using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
	/// 权限表
	/// </summary>
	[Serializable]
    public partial class Functions
    {
        public Functions()
        { }
        #region Model
        private int _functionid;
        private string _functionname;
        private string _rightsvalue;
        private int _functiontype;
        private string _btnid;
        private string _icourl;
        private int _parentid;
        private string _descriptions;
        private DateTime _createtime;
        private DateTime _updatetime;
        /// <summary>
        /// 排序
        /// </summary>
        public int? FunctionSort { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public int FunctionID
        {
            set { _functionid = value; }
            get { return _functionid; }
        }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string FunctionName
        {
            set { _functionname = value; }
            get { return _functionname; }
        }
        /// <summary>
        /// 权限值
        /// </summary>
        public string RightsValue
        {
            set { _rightsvalue = value; }
            get { return _rightsvalue; }
        }
        /// <summary>
        /// 父编号
        /// </summary>
        public int ParentID
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
		/// 权限类型
		/// </summary>
		public int FunctionType
        {
            set { _functiontype = value; }
            get { return _functiontype; }
        }
        /// <summary>
		/// 按钮id值
		/// </summary>
		public string BtnID
        {
            set { _btnid = value; }
            get { return _btnid; }
        }
        /// <summary>
		/// 图标地址
		/// </summary>
		public string IcoUrl
        {
            set { _icourl = value; }
            get { return _icourl; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Descriptions
        {
            set { _descriptions = value; }
            get { return _descriptions; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        #endregion Model

    }
}
