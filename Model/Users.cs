using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Users:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Users
    {
        public Users()
        { }
        #region Model
        private int _userid;
        private int _departmentid;
        private string _username;
        private string _loginname;
        private string _loginpassword;
        private string _mobile;
        private bool _sex;
        private int _usertype;
        private DateTime _createtime = DateTime.Now;
        private DateTime? _updatetime;
        private bool _isdeleted = false;
        /// <summary>
        /// 身份证到期时间
        /// </summary>
        public DateTime? IdCardDueTime { get; set; }
        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 登陆次数
        /// </summary>
        public int? LoginPv { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 部门Id
        /// </summary>
        public int DepartmentID
        {
            set { _departmentid = value; }
            get { return _departmentid; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName
        {
            set { _loginname = value; }
            get { return _loginname; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string LoginPassword
        {
            set { _loginpassword = value; }
            get { return _loginpassword; }
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 用户类型，1表示普通用户，2表示面试用户
        /// </summary>
        public int UserType
        {
            set { _usertype = value; }
            get { return _usertype; }
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
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDeleted
        {
            set { _isdeleted = value; }
            get { return _isdeleted; }
        }
        #endregion Model

    }
}
