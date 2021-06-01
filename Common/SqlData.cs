/**************************************************************************************************
 * 创建人: 廖毅
 *
 * 创建时间: 2008年10月9日
 * 
 * 最后修改时间：2017年10月25日
 *
 * 描述: 
**************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Reflection;
using Common;

namespace Common
{
    public class SqlData
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader SelectDataReader(string dbname, string proc, Dictionary<string, object> dic)
        {
            SqlConnection scon = null;
            SqlCommand scom = null;
            try
            {
                scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString);
                if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                scom = new SqlCommand(proc, scon);
                scom.CommandType = CommandType.StoredProcedure;
                if (dic != null)
                {
                    foreach (KeyValuePair<string, object> kvp in dic)
                    {
                        if (kvp.Value != null) { scom.Parameters.Add(new SqlParameter(kvp.Key, kvp.Value)); }
                        else { scom.Parameters.Add(new SqlParameter(kvp.Key, DBNull.Value)); }
                    }
                }
                return scom.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex) { scon.Close(); LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }
        /// <summary>
        /// 查询数据(分页)
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param>
        /// <param name="total">总条数(输出参数)</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader SelectDataReader(string dbname, string proc, Dictionary<string, object> dic, out SqlParameter total)
        {
            SqlConnection scon = null;
            SqlCommand scom = null;
            try
            {
                scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString);
                if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                SqlParameter sparm = null;
                scom = new SqlCommand(proc, scon);
                scom.CommandType = CommandType.StoredProcedure;
                if (dic != null)
                {
                    foreach (KeyValuePair<string, object> kvp in dic)
                    {
                        if (kvp.Value != null) { sparm = new SqlParameter(kvp.Key, kvp.Value); }
                        else { sparm = new SqlParameter(kvp.Key, DBNull.Value); }
                        sparm.Direction = ParameterDirection.Input;
                        scom.Parameters.Add(sparm);
                    }
                }
                sparm = new SqlParameter("Total", -1);
                sparm.Direction = ParameterDirection.Output;
                total = sparm;
                scom.Parameters.Add(sparm);
                return scom.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex) { scon.Close(); LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }
        /// <summary>
        /// 查询数据(分页)输出两个数字类型参数
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param>
        /// <param name="total">总条数(输出参数)</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader SelectDataReader(string dbname, string proc, Dictionary<string, object> dic, out SqlParameter total, out SqlParameter total2)
        {
            SqlConnection scon = null;
            SqlCommand scom = null;
            try
            {
                scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString);
                if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                SqlParameter sparm = null;
                scom = new SqlCommand(proc, scon);
                scom.CommandType = CommandType.StoredProcedure;
                if (dic != null)
                {
                    foreach (KeyValuePair<string, object> kvp in dic)
                    {
                        if (kvp.Value != null) { sparm = new SqlParameter(kvp.Key, kvp.Value); }
                        else { sparm = new SqlParameter(kvp.Key, DBNull.Value); }
                        sparm.Direction = ParameterDirection.Input;
                        scom.Parameters.Add(sparm);
                    }
                }
                sparm = new SqlParameter("Total", -1);
                sparm.Direction = ParameterDirection.Output;
                total = sparm;
                scom.Parameters.Add(sparm);

                sparm = new SqlParameter("Total2", -1);
                sparm.Direction = ParameterDirection.Output;
                total2 = sparm;
                scom.Parameters.Add(sparm);
                return scom.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex) { scon.Close(); LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param>
        /// <returns>DataTable</returns>
        public static DataTable SelectDataTable(string dbname, string proc, Dictionary<string, object> dic)
        {
            SqlDataAdapter sda = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    DataTable dt = new DataTable();
                    sda = new SqlDataAdapter(proc, scon);
                    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (dic != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in dic)
                        {
                            if (kvp.Value != null) { sda.SelectCommand.Parameters.Add(new SqlParameter(kvp.Key, kvp.Value)); }
                            else { sda.SelectCommand.Parameters.Add(new SqlParameter(kvp.Key, DBNull.Value)); }
                        }
                    }
                    sda.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { sda.Dispose(); }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param>
        /// <returns>DataSet</returns>
        public static DataSet SelectDataSet(string dbname, string proc, Dictionary<string, object> dic)
        {
            SqlDataAdapter sda = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    DataSet ds = new DataSet();
                    sda = new SqlDataAdapter(proc, scon);
                    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (dic != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in dic)
                        {
                            if (kvp.Value != null) { sda.SelectCommand.Parameters.Add(new SqlParameter(kvp.Key, kvp.Value)); }
                            else { sda.SelectCommand.Parameters.Add(new SqlParameter(kvp.Key, DBNull.Value)); }
                        }
                    }
                    sda.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { sda.Dispose(); }
        }
        /// <summary>
        /// 查询单个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T SelectData<T>(string dbname, string sql, Dictionary<string, object> dic) where T : new()
        {
            SqlDataReader sdr = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    SqlCommand command = new SqlCommand(sql, scon);
                    command.CommandType = CommandType.Text;
                    if (dic != null && dic.Count > 0)
                    {
                        foreach (KeyValuePair<string, object> kvp in dic)
                        {
                            if (kvp.Value != null)
                            {
                                command.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                            else
                            {
                                command.Parameters.AddWithValue(kvp.Key, DBNull.Value);
                            }
                        }
                    }
                    sdr = command.ExecuteReader();
                    if (sdr == null || sdr.HasRows == false) return default(T);
                    var res = new T();
                    var propInfos = GetFieldnameFromCache<T>();
                    if (sdr.Read())
                    {
                        for (int i = 0; i < sdr.FieldCount; i++)
                        {
                            if (!Convert.IsDBNull(sdr[i]))//判断值是否为空
                            {
                                var n = sdr.GetName(i).ToLower();
                                if (propInfos.ContainsKey(n))
                                {
                                    PropertyInfo prop = propInfos[n];
                                    var IsValueType = prop.PropertyType.IsValueType;
                                    object defaultValue = null;//引用类型或可空值类型的默认值
                                    if (IsValueType)
                                    {
                                        if ((!prop.PropertyType.IsGenericType)
                                            || (prop.PropertyType.IsGenericType && !prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))))
                                        {
                                            defaultValue = 0;//非空值类型的默认值
                                        }
                                    }
                                    var v = sdr.GetValue(i);
                                    var chkType = prop.PropertyType.FullName;
                                    if (chkType.Contains("System.UInt32"))
                                    {
                                        prop.SetValue(res, (Convert.IsDBNull(v) ? defaultValue : (uint)Convert.ToInt32(v)), null);
                                    }
                                    else
                                    {
                                        prop.SetValue(res, (Convert.IsDBNull(v) ? defaultValue : v), null);
                                    }

                                }
                            }
                        }
                    }
                    return res;
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { sdr.Dispose(); }
        }
        /// <summary>
        /// 属性反射信息缓存 key:类型的hashCode,value属性信息
        /// </summary>
        private static Dictionary<int, Dictionary<string, PropertyInfo>> propInfoCache = new Dictionary<int, Dictionary<string, PropertyInfo>>();

        private static Dictionary<string, PropertyInfo> GetFieldnameFromCache<T>()
        {
            Dictionary<string, PropertyInfo> res = null;
            var hashCode = typeof(T).GetHashCode();
            if (!propInfoCache.ContainsKey(hashCode))
            {
                propInfoCache.Add(hashCode, GetFieldname<T>());
            }
            res = propInfoCache[hashCode];
            return res;
        }
        /// <summary>
        /// 获取一个类型的对应数据表的字段信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Dictionary<string, PropertyInfo> GetFieldname<T>()
        {
            var res = new Dictionary<string, PropertyInfo>();
            var props = typeof(T).GetProperties();
            foreach (PropertyInfo item in props)
            {
                res.Add(GetFiledName(item), item);
            }
            return res;
        }
        /// <summary>
        /// 获取该属性对应到数据表中的字段名称
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static string GetFiledName(PropertyInfo propInfo)
        {
            var fieldname = propInfo.Name;
            var attr = propInfo.GetCustomAttributes(false);
            foreach (var a in attr)
            {
                if (a is DataFieldAttribute)
                {
                    fieldname = (a as DataFieldAttribute).Name;
                    break;
                }
            }
            return fieldname.ToLower();
        }
        /// <summary>
        /// 添加,删除,更改数据
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param> 
        /// <returns>受到影响的行数</returns>
        public static int InsDelUpdData(string dbname, string proc, Dictionary<string, object> dic)
        {
            SqlCommand scom = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    scom = new SqlCommand(proc, scon);
                    scom.CommandType = CommandType.StoredProcedure;
                    if (dic != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in dic)
                        {
                            if (kvp.Value != null) { scom.Parameters.Add(new SqlParameter(kvp.Key, kvp.Value)); }
                            else { scom.Parameters.Add(new SqlParameter(kvp.Key, DBNull.Value)); }
                        }
                    }
                    return scom.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }

        /// <summary>
        /// 添加,删除,更改数据
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param>
        /// <returns>存储过程中自定义的返回值(默认返回值为:-1)</returns>
        public static int InsDelUpdDataOutput(string dbname, string proc, Dictionary<string, object> dic)
        {
            SqlCommand scom = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    SqlParameter sparm = null;
                    scom = new SqlCommand(proc, scon);
                    scom.CommandType = CommandType.StoredProcedure;
                    if (dic != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in dic)
                        {
                            if (kvp.Value != null) { sparm = new SqlParameter(kvp.Key, kvp.Value); }
                            else { sparm = new SqlParameter(kvp.Key, DBNull.Value); }
                            sparm.Direction = ParameterDirection.Input;
                            scom.Parameters.Add(sparm);
                        }
                    }
                    sparm = new SqlParameter("OutPut", -1);
                    sparm.Direction = ParameterDirection.Output;
                    scom.Parameters.Add(sparm);
                    scom.ExecuteNonQuery();
                    return Convert.ToInt32(scom.Parameters["OutPut"].Value.ToString());
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }
        /// <summary>
        /// 添加,删除,更改数据输出两个参数
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param>
        /// <returns>存储过程中自定义的返回值(默认返回值为:-1)</returns>
        public static void InsDelUpdDataOutput(string dbname, string proc, Dictionary<string, object> dic, out int output, out int output2)
        {
            SqlCommand scom = null;
            output = 0; output2 = 0;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    SqlParameter sparm = null;
                    SqlParameter sparm2 = null;
                    scom = new SqlCommand(proc, scon);
                    scom.CommandType = CommandType.StoredProcedure;
                    if (dic != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in dic)
                        {
                            if (kvp.Value != null) { sparm = new SqlParameter(kvp.Key, kvp.Value); }
                            else { sparm = new SqlParameter(kvp.Key, DBNull.Value); }
                            sparm.Direction = ParameterDirection.Input;
                            scom.Parameters.Add(sparm);
                        }
                    }
                    sparm = new SqlParameter("OutPut", -1);
                    sparm.Direction = ParameterDirection.Output;
                    sparm2 = new SqlParameter("OutPut2", -1);
                    sparm2.Direction = ParameterDirection.Output;
                    scom.Parameters.Add(sparm);
                    scom.Parameters.Add(sparm2);
                    scom.ExecuteNonQuery();
                    output = Convert.ToInt32(scom.Parameters["OutPut"].Value.ToString());
                    output2 = Convert.ToInt32(scom.Parameters["OutPut2"].Value.ToString());
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }
        /// <summary>
        /// 添加,删除,更改数据输出两个参数
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param>
        /// <returns>存储过程中自定义的返回值(默认返回值为:-1)</returns>
        public static void InsDelUpdDataOutput(string dbname, string proc, Dictionary<string, object> dic, out int output, out object output2)
        {
            SqlCommand scom = null;
            output = 0; output2 = 0;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    SqlParameter sparm = null;
                    SqlParameter sparm2 = null;
                    scom = new SqlCommand(proc, scon);
                    scom.CommandType = CommandType.StoredProcedure;
                    if (dic != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in dic)
                        {
                            if (kvp.Value != null) { sparm = new SqlParameter(kvp.Key, kvp.Value); }
                            else { sparm = new SqlParameter(kvp.Key, DBNull.Value); }
                            sparm.Direction = ParameterDirection.Input;
                            scom.Parameters.Add(sparm);
                        }
                    }
                    sparm = new SqlParameter("OutPut", -1);
                    sparm.Direction = ParameterDirection.Output;
                    sparm2 = new SqlParameter("OutPut2", -1);
                    sparm2.Direction = ParameterDirection.Output;
                    scom.Parameters.Add(sparm);
                    scom.Parameters.Add(sparm2);
                    scom.ExecuteNonQuery();
                    output = Convert.ToInt32(scom.Parameters["OutPut"].Value.ToString());
                    output2 =scom.Parameters["OutPut2"].Value.ToString();
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }
        /// <summary>
        /// 添加,删除,更改数据输出两个参数,返回值与返回信息
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param>
        /// <returns>存储过程中自定义的返回值(默认返回值为:-1)</returns>
        public static void CUDdataOutputHaveMSG(string dbname, string proc, Dictionary<string, object> dic, out int output, out string output2)
        {
            SqlCommand scom = null;
            output = 0; output2 = "";
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    SqlParameter sparm = null;
                    SqlParameter sparm2 = null;
                    scom = new SqlCommand(proc, scon);
                    scom.CommandType = CommandType.StoredProcedure;
                    if (dic != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in dic)
                        {
                            if (kvp.Value != null) { sparm = new SqlParameter(kvp.Key, kvp.Value); }
                            else { sparm = new SqlParameter(kvp.Key, DBNull.Value); }
                            sparm.Direction = ParameterDirection.Input;
                            scom.Parameters.Add(sparm);
                        }
                    }
                    sparm = new SqlParameter("OutPut", -1);
                    sparm.Direction = ParameterDirection.Output;
                    sparm2 = new SqlParameter("OutPut2", SqlDbType.VarChar, 8000);
                    sparm2.Direction = ParameterDirection.Output;
                    scom.Parameters.Add(sparm);
                    scom.Parameters.Add(sparm2);
                    scom.ExecuteNonQuery();
                    output = Convert.ToInt32(scom.Parameters["OutPut"].Value);
                    output2 = scom.Parameters["OutPut2"].Value.ToString();
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="sql">sql语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader SelectDataReader(string dbname, string sql)
        {
            SqlConnection scon = null;
            SqlCommand scom = null;
            try
            {
                scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString);
                if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                scom = new SqlCommand(sql, scon);
                scom.CommandType = CommandType.Text;
                return scom.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex) { scon.Close(); LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }
        /// <summary>
        /// 添加,删除,更改数据
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="sql">sql语句</param>
        /// <returns>受到影响的行数</returns>
        public static int InsDelUpdData(string dbname, string sql)
        {
            SqlCommand scom = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    scom = new SqlCommand(sql, scon);
                    scom.CommandType = CommandType.Text;
                    return scom.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="sql">sql语句</param>
        /// <returns>DataTable</returns>
        public static DataTable SelectDataTable(string dbname, string sql)
        {
            SqlDataAdapter sda = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    DataTable dt = new DataTable();
                    sda = new SqlDataAdapter(sql, scon);
                    sda.SelectCommand.CommandType = CommandType.Text;
                    sda.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { sda.Dispose(); }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static DataSet SelectDataSet(string dbname, string sql)
        {
            SqlDataAdapter sda = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    DataSet ds = new DataSet();
                    sda = new SqlDataAdapter(sql, scon);
                    sda.SelectCommand.CommandType = CommandType.Text;
                    sda.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { sda.Dispose(); }
        }
        /// <summary>
        /// 查询返回数据的第一行第一列 执行count avg max min
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param> 
        /// <returns>受到影响的行数</returns>
        public static object ExecuteData(string dbname, string proc, Dictionary<string, object> dic)
        {
            SqlCommand scom = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    scom = new SqlCommand(proc, scon);
                    scom.CommandType = CommandType.StoredProcedure;
                    if (dic != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in dic)
                        {
                            if (kvp.Value != null) { scom.Parameters.Add(new SqlParameter(kvp.Key, kvp.Value)); }
                            else { scom.Parameters.Add(new SqlParameter(kvp.Key, DBNull.Value)); }
                        }
                    }
                    return scom.ExecuteScalar();
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }
        /// <summary>
        /// 查询返回数据的第一行第一列 执行count avg max min
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="proc">存储过程</param>
        /// <param name="dic">参数</param> 
        /// <returns>受到影响的行数</returns>
        public static object ExecuteData(string dbname, string proc)
        {
            SqlCommand scom = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    scom = new SqlCommand(proc, scon);
                    scom.CommandType = CommandType.StoredProcedure;

                    return scom.ExecuteScalar();
                }
            }
            catch (Exception ex) { LogRecord.LogCatch(ex); throw; }
            finally { scom.Dispose(); }
        }
        /// <summary>
        /// 将int数组转化为以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separatorInND">转化结果末尾是否含有逗号</param>
        /// <returns></returns>
        public static string ArrayToString(IEnumerable<int> list, bool separatorInND = true)
        {
            string temp = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            if (list != null)
            {
                foreach (var item in list)
                {
                    stringBuilder.Append(item);
                    stringBuilder.Append(',');
                }
                temp = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(temp) && !separatorInND)
                {
                    temp = temp.Substring(0, temp.Length - 1);
                }
            }
            return temp;

        }
    }
}