using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// sql执行扩展
    /// </summary>
    public static class SqlDataExtension
    {
        /// <summary>
        /// 查询单个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T SelectData<T>(string dbname, string sql, Dictionary<string, List<string>> paramDic) where T : new()
        {
            SqlDataReader sdr = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings[dbname].ConnectionString))
                {
                    if (scon.State.Equals(ConnectionState.Closed)) { scon.Open(); }
                    SqlCommand command = new SqlCommand(sql, scon);
                    command.CommandType = CommandType.Text;
                    if (paramDic != null && paramDic.Count > 0)
                    {
                        List<string> keyList = null;
                        List<string> valueList = null;
                        paramDic.TryGetValue("paramKey", out keyList);
                        paramDic.TryGetValue("paramValue", out valueList);
                        for (var i = 0; i < keyList.Count; i++)
                        {
                            command.Parameters.AddWithValue(keyList[i], valueList[i]);
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
                                        prop.SetValue(res,(Convert.IsDBNull(v) ? defaultValue :(uint)Convert.ToInt32(v)), null);
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

        /// <summary>
        /// 将SqlDataReader转成T类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T To<T>(this SqlDataReader reader)
          where T : new()
        {
            if (reader == null || reader.HasRows == false) return default(T);

            var res = new T();
            var propInfos = GetFieldnameFromCache<T>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var n = reader.GetName(i).ToLower();
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
                    var v = reader.GetValue(i);
                    prop.SetValue(res, (Convert.IsDBNull(v) ? defaultValue : v), null);
                }
            }

            return res;
        }

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
                res.Add(item.GetFiledName(), item);
            }
            return res;
        }



        /// <summary>
        /// 将SqlDataReader转成List<T>类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this SqlDataReader reader)
            where T : new()
        {
            if (reader == null || reader.HasRows == false) return null;
            var res = new List<T>();
            while (reader.Read())
            {
                res.Add(reader.To<T>());
            }
            return res;
        }

        /// <summary>
        /// 获取该属性对应到数据表中的字段名称
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static string GetFiledName(this PropertyInfo propInfo)
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
    }
}
