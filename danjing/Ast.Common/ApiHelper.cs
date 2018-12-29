using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ast.Common
{
    public static class ApiHelper
    {
        public static T HttpPost<T>(string url, Dictionary<string, string> postParameters)
        {
            string retString = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset:utf-8";
            //POST参数
            StringBuilder paraStrBuilder = new StringBuilder();
            foreach (string key in postParameters.Keys)
            {
                paraStrBuilder.AppendFormat("{0}={1}&", key, postParameters[key]);
            }
            string para = paraStrBuilder.ToString();
            if (para.EndsWith("&"))
                para = para.Remove(para.Length - 1, 1);
            //编码要跟服务器编码统一
            byte[] bt = Encoding.UTF8.GetBytes(para);
            string responseData = String.Empty;
            request.ContentLength = bt.Length;
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bt, 0, bt.Length);
                reqStream.Close();
            }
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream stream = response.GetResponseStream();
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        retString = streamReader.ReadToEnd().ToString();
                    }
                }
                return JsonConvert.DeserializeObject<T>(retString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string gettemper(string temp)
        {
            var str = string.Empty;
            var arr = temp.Split(',');
            foreach (var item in arr)
            {
                switch (item)
                {
                    case "1":
                        str += "正常冰" + ",";
                        break;
                    case "2":
                        str += "去冰" + ",";
                        break;
                    case "3":
                        str += "少冰" + ",";
                        break;
                    case "4":
                        str += "微冰" + ",";
                        break;
                    case "5":
                        str += "常温" + ",";
                        break;
                    case "6":
                        str += "热" + ",";
                        break;
                    case "7":
                        str += "默认" + ",";
                        break;
                }
            }
            return str.Substring(0, str.Length - 1);
        }
        public static string getsize(string temp)
        {
            var str = string.Empty;
            var arr = temp.Split(',');
            foreach (var item in arr)
            {
                switch (item)
                {
                    case "1":
                        str += "不加糖" + ",";
                        break;
                    case "2":
                        str += "五分糖" + ",";
                        break;
                    case "3":
                        str += "七分糖" + ",";
                        break;
                    case "4":
                        str += "全糖" + ",";
                        break;
                    case "5":
                        str += "默认" + ",";
                        break;
                }
            }
            return str.Substring(0, str.Length - 1);
        }
        public static string GetOrderNumber()
        {
            string num = DateTime.Now.ToString("yyMMddHHmmss");
            return num + Number(2, true).ToString();
        }
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        public static bool AIsNullOrEmptyString(this object obj)
        {
            return obj == null || (obj is string && (string)obj == "") || (obj is DBNull && (DBNull)obj == DBNull.Value);
        }
        public static int Aint(this object obj)
        {
            if (AIsNullOrEmptyString(obj)) return 0;
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return 0;
            }
        }
        public static decimal Adecimal(this object obj)
        {
            if (obj.AIsNullOrEmptyString()) return 0;

            return Convert.ToDecimal(obj);

        }
        public static string Astring(this object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        public static class ModelConvertHelper<T> where T : new()
        {
            public static IList<T> ConvertToModel(DataTable dt)
            {
                // 定义集合    
                IList<T> ts = new List<T>();

                // 获得此模型的类型   
                Type type = typeof(T);
                string tempName = "";

                foreach (DataRow dr in dt.Rows)
                {
                    T t = new T();
                    // 获得此模型的公共属性      
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;  // 检查DataTable是否包含此列    

                        if (dt.Columns.Contains(tempName))
                        {
                            // 判断此属性是否有Setter      
                            if (!pi.CanWrite) continue;

                            object value = dr[tempName];
                            if (value != DBNull.Value)
                                pi.SetValue(t, value, null);
                        }
                    }
                    ts.Add(t);
                }
                return ts;
            }
        }

        /// <summary>
        /// 记录类型
        /// </summary>
        public class TypeRecord
        {
            /// <summary>
            /// 充值
            /// </summary>
            public const int Recharge = 1;

            /// <summary>
            /// 消费
            /// </summary>
            public const int Consume = 2;
        }
        public class StateRecharge
        {
            /// <summary>
            /// 已提交
            /// </summary>
            public const int Committed = 0;

            /// <summary>
            /// 成功
            /// </summary>
            public const int Success = 1;

            /// <summary>
            /// 失败
            /// </summary>
            public const int Fail = 2;
        }
    }
}
