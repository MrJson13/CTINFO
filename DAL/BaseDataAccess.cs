using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public class BaseDataAccess
    {
        /// <summary>
        /// Get ConnectionString&ProviderName
        /// </summary>
        public Tuple<string, string> GetConnectionString(string dbName)
        {
            if (string.IsNullOrWhiteSpace(dbName))
            {
                if (string.IsNullOrWhiteSpace(_dbName))
                    throw new Exception("db name is null or empty");
                else
                    dbName = _dbName;
            }
            var connObj = System.Configuration.ConfigurationManager.ConnectionStrings[dbName];
            return new Tuple<string, string>(connObj.ConnectionString, connObj.ProviderName);
        }
        private string _dbName = null;
        public BaseDataAccess(string dbName)
        {
            _dbName = dbName;
        }
        public BaseDataAccess()
        {
        }
        /// <summary>
        /// 执行sql，返回第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public virtual T ExecuteScalar<T>(NPoco.Sql sql, string dbName = null)
        {
            Tuple<string, string> connObj = GetConnectionString(dbName);
            using (NPoco.Database db = new NPoco.Database(connObj.Item1, connObj.Item2))
            {
                return db.ExecuteScalar<T>(sql);
            }
        }
        /// <summary>
        /// 执行sql，返回第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public virtual T ExecuteScalar<T>(string sql, string dbName = null, params object[] args)
        {
            Tuple<string, string> connObj = GetConnectionString(dbName);
            using (NPoco.Database db = new NPoco.Database(connObj.Item1, connObj.Item2))
            {
                return db.ExecuteScalar<T>(sql, args);
            }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="sql">sql语句</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public virtual DataList<T> Page<T>(long pageIndex, long pageSize, string sql, string dbName = null, params object[] args) where T : new()
        {
            NPoco.Page<T> sourcePager = null;
            Tuple<string, string> connObj = GetConnectionString(dbName);
            using (NPoco.Database db = new NPoco.Database(connObj.Item1, connObj.Item2))
            {
                sourcePager = db.Page<T>(pageIndex, pageSize, sql, args);
            }
            DataList<T> destPager = new DataList<T>()
            {
                Total = (int)sourcePager.TotalItems,
                Rows = sourcePager.Items
            };
            return destPager;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="sql">sql语句</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public virtual DataList<T> Page<T>(long pageIndex, long pageSize, NPoco.Sql sql, string dbName = null) where T : new()
        {
            NPoco.Page<T> sourcePager = null;
            Tuple<string, string> connObj = GetConnectionString(dbName);
            using (NPoco.Database db = new NPoco.Database(connObj.Item1, connObj.Item2))
            {
                sourcePager = db.Page<T>(pageIndex, pageSize, sql);
            }
            DataList<T> destPager = new DataList<T>()
            {
                Total = (int)sourcePager.TotalItems,
                Rows = sourcePager.Items
            };
            return destPager;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbName"></param>
        /// <param name="poco"></param>
        /// <returns></returns>
        public virtual object Insert<T>(T poco, string dbName = null)
        {
            Tuple<string, string> connObj = GetConnectionString(dbName);
            using (NPoco.Database db = new NPoco.Database(connObj.Item1, connObj.Item2))
            {
                return db.Insert<T>(poco);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbName"></param>
        /// <param name="poco"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public virtual int Update(object poco, string dbName = null)
        {
            Tuple<string, string> connObj = GetConnectionString(dbName);
            using (NPoco.Database db = new NPoco.Database(connObj.Item1, connObj.Item2))
            {
                return db.Update(poco);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbName"></param>
        /// <param name="poco"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public virtual int Update<T>(T poco, IEnumerable<string> columns, string dbName = null)
        {
            Tuple<string, string> connObj = GetConnectionString(dbName);
            using (NPoco.Database db = new NPoco.Database(connObj.Item1, connObj.Item2))
            {
                return db.Update(poco, columns);
            }
        }

        /// <summary>
        /// 通过主键删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pocoOrPrimaryKey"></param>
        /// <returns></returns>
        public int Delete<T>(object pocoOrPrimaryKey, string dbName = null)
        {
            Tuple<string, string> connObj = GetConnectionString(dbName);
            using (NPoco.Database db = new NPoco.Database(connObj.Item1, connObj.Item2))
            {
                return db.Delete<T>(pocoOrPrimaryKey);
            }
        }

        /// <summary>
        /// 获取单条记录对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual T SingleOrDefaultById<T>(object primaryKey, string dbName = null)
        {
            Tuple<string, string> connObj = GetConnectionString(dbName);
            using (NPoco.Database db = new NPoco.Database(connObj.Item1, connObj.Item2))
            {
                return db.SingleOrDefaultById<T>(primaryKey);
            }
        }
    }
}
