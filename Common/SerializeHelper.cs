/***********************************************
* 创 建 者：江腾
* 创建日期：2017/11/06 15:37:04
* 类 名 称：SerializeHelper
* 版 本 号：v1.0.0.0
* 功能说明：
* 命名空间：Common
* 修改历史：
* **********************************************
* 修 改 者：
* 修改说明：
* **********************************************
* Copyright  All right reserved
* **********************************************/
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Xml.Serialization;

namespace Common
{
    public class SerializeHelper
    {
        private SerializeHelper() { }
        #region Json
        private static JsonSerializerSettings JsonDefaultSetting()
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            var jsonSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            jsonSettings.Converters.Add(datetimeConverter);
            return jsonSettings;
        }

        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns></returns>
        public static string JsonSerialize(object obj, JsonSerializerSettings setting = null)
        {
            setting = setting ?? JsonDefaultSetting();
            try
            {
                return JsonConvert.SerializeObject(obj, Formatting.None, setting);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将指定的 JSON 数据反序列化成指定对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">JSON 数据。</param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string json, JsonSerializerSettings setting = null)
        {
            setting = setting ?? JsonDefaultSetting();
            return JsonConvert.DeserializeObject<T>(json, setting);
        }
        /// <summary>
        /// 将指定的 JSON 数据反序列化成JObject对象。
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JObject JsonJObjectParse(string json)
        {
            return JObject.Parse(json);
        }

        /// <summary>
        /// 获得某个json对象下一层指定值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="subJObjectValue"></param>
        /// <returns></returns>
        public static string JsonJObjectValue(string json, string subJObjectValue)
        {
            return JsonJObjectParse(json).GetValue(subJObjectValue).ToString();
        }
        #endregion Json

        #region XML
        /// <summary>
        /// XML序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T t)
        {
            XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
            xsn.Add(string.Empty, string.Empty);
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StringWriter sw = new StringWriter();
            xs.Serialize(sw, t, xsn);
            string str = sw.ToString();
            sw.Close();
            sw.Dispose();
            return str;
        }
        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StringReader sr = new StringReader(xml);
            T obj = (T)xs.Deserialize(sr);
            sr.Close();
            sr.Dispose();
            return obj;
        }
        #endregion XML

        #region Protobufer
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static byte[] ProtoSerialize<T>(T t)
        {
            try
            {
                byte[] data;
                using (var ms = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(ms, t);
                    data = new byte[(int)ms.Length];
                    ms.Position = 0L;
                    ms.Read(data, 0, data.Length);
                }
                return data;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="buffer">字节数据</param>
        /// <returns></returns>
        public static T ProtoDeSerialize<T>(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return default(T);
            try
            {
                using (var ms = new MemoryStream())
                {
                    //将消息写入流中  
                    ms.Write(buffer, 0, buffer.Length);
                    //将流的位置归0  
                    ms.Position = 0;
                    //使用工具反序列化对象  
                    var result = ProtoBuf.Serializer.Deserialize<T>(ms);
                    return result;
                }
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="stream">流对象</param>
        /// <returns></returns>
        public static T ProtoDeSerialize<T>(Stream stream)
        {
            if (stream == null) return default(T);
            try
            {
                //使用工具反序列化对象  
                var result = ProtoBuf.Serializer.Deserialize<T>(stream);
                return result;
            }
            catch
            {
                return default(T);
            }
        }
        #endregion Protobufer
    }
}
