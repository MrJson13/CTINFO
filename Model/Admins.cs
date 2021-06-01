using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Admins:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    [NPoco.TableName("Admins")]
    [NPoco.PrimaryKey("AdminGUID", AutoIncrement = false)]
    public partial class Admins
    {
        public Admins()
        { }
        #region Model
        private Guid _adminguid;
        private string _adminid;
        private string _adminpsw;
        private string _adminname;
        private DateTime _addtime = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        [NPoco.Column("AdminGUID")]
        public Guid AdminGUID
        {
            set { _adminguid = value; }
            get { return _adminguid; }
        }
        /// <summary>
        /// 管理员登录名
        /// </summary>
        [NPoco.Column("AdminID")]
        public string AdminID
        {
            set { _adminid = value; }
            get { return _adminid; }
        }
        /// <summary>
        /// 管理员密码
        /// </summary>
        [NPoco.Column("AdminPSW")]
        public string AdminPSW
        {
            set { _adminpsw = value; }
            get { return _adminpsw; }
        }
        /// <summary>
        /// 管理员名称
        /// </summary>
        [NPoco.Column("AdminName")]
        public string AdminName
        {
            set { _adminname = value; }
            get { return _adminname; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        [NPoco.Column("AddTime")]
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 是否已删除
        /// </summary>
        [NPoco.Column("IsDeleted")]
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        [NPoco.Column("IsSupper")]
        public bool IsSupper { get; set; }
        #endregion
    }
}
