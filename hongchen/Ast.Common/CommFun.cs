using Newtonsoft.Json;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Globalization;
using System.Security.Cryptography;

namespace Ast.Common
{
    public static class CommFun
    {
        static char[] character = { '1', '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'z' };
        static Random rnd = new Random();
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5(string str)
        {
            str = "OTAMS" + str;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var encoding = Encoding.UTF8;
                var encryptedBytes = md5.ComputeHash(encoding.GetBytes(str));
                var sb = new StringBuilder(32);
                foreach (var bt in encryptedBytes)
                {
                    sb.Append(bt.ToString("x").PadLeft(2, '0'));
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 安全的字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Get_Safe_Str(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            else
            {
                string str = obj.ToString();
                str = str.Replace("'", "‘");
                str = Regex.Replace(str, "<a ", "<a target=_blank ", RegexOptions.IgnoreCase);
                str = StringHelper.FilterScript(str, "body,script,iframe,object");
                return str.Trim();
            }
        }
        /// <summary>
        /// 获取session的用户信息 By KeHen 2018.4.12
        /// </summary>
        /// <returns></returns>
        public static object GetSession(string name)
        {
            return HttpContext.Current.Session[name];
        }

        /// <summary>
        /// 转换成 json By Xuwx Add 201703071346
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }


        /// <summary>
        /// 随机文件名称 By KeHen Add 201703092241
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetRandomFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                System.Threading.Thread.Sleep(100);
                string fileType = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                Random rand = new Random();
                DateTime now = DateTime.Now;
                string str = string.Empty;
                str += now.Year.ToString(CultureInfo.InvariantCulture);
                str += now.Month.ToString(CultureInfo.InvariantCulture);
                str += now.Day.ToString(CultureInfo.InvariantCulture);
                str += now.Hour.ToString(CultureInfo.InvariantCulture);
                str += now.Minute.ToString(CultureInfo.InvariantCulture);
                str += now.Second.ToString(CultureInfo.InvariantCulture);
                str += rand.Next(0, 1000);
                return str + fileType;
            }
            else
            {
                return "";
            }
        }


        public static string GetRandomCode(int len)
        {
            string chkCode = string.Empty;
            if (len <= 0)
                return chkCode;
            for (int i = 0; i < len; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            return chkCode;
        }

        public static string GetIPAddress()
        {
            string IP = string.Empty;
            HttpRequest Request = System.Web.HttpContext.Current.Request; // 如果使用代理，获取真实IP  
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                IP = Request.ServerVariables["REMOTE_ADDR"];
            else
                IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (IP == null || IP == "")
                IP = Request.UserHostAddress;
            return IP;
        }
    }
}
