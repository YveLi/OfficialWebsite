using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Ast.Common
{
    public class WebUtils : System.Web.UI.Page
    {
        /// <summary>
        ///   获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetReuqestIp()
        {
            var result = HttpContext.Current.Request.ServerVariables["HTTP_CDN_SRC_IP"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(result) || !IsIP(result))
                return "127.0.0.1";

            return result;
        }

        /// <summary>
        /// 获取物理路径
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static new string MapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用 
            {
                strPath = strPath.TrimStart('~');
                strPath = strPath.Replace("/", "\\");
                strPath = strPath.Replace("\\", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }


        #region 时间相关
        /// <summary> 
        /// 取指定日期是一年中的第几周 
        /// </summary> 
        /// <param name="dtime">给定的日期</param> 
        /// <returns>数字 一年中的第几周</returns> 
        public static int WeekOfYear(DateTime dtime)
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            return gc.GetWeekOfYear(dtime, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

        }


        /// <summary>
        /// 获取某一年有多少周
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>该年周数</returns>
        public static int GetWeekAmount(int year)
        {
            DateTime end = new DateTime(year, 12, 31);  //该年最后一天
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            return gc.GetWeekOfYear(end, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);  //该年星期数
        }

        public enum DateInterval
        {
            Second, Minute, Hour, Day, Week, Month, Quarter, Year
        }

        public static long DateDiff(DateInterval Interval, System.DateTime StartDate, System.DateTime EndDate)
        {
            long lngDateDiffValue = 0;
            System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (Interval)
            {
                case DateInterval.Second:
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case DateInterval.Minute:
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case DateInterval.Hour:
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case DateInterval.Day:
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case DateInterval.Week:
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case DateInterval.Month:
                    lngDateDiffValue = (long)(TS.Days / 30);
                    break;
                case DateInterval.Quarter:
                    lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    break;
                case DateInterval.Year:
                    lngDateDiffValue = (long)(TS.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }
        #endregion


        #region Request操作类

        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }
        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            //
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }

        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch { }

            if (retVal == null)
                return "";

            return retVal;

        }

        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
            {
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            return request.Url.Host;
        }

        /// <summary>
        /// 得到主机头
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }


        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        public static string CombineRawUrl(string url)
        {
            if ((url[0] != '~') && (url.IndexOf(':') < 0))
            {
                string rawUrl = HttpContext.Current.Request.RawUrl;
                if (rawUrl.IndexOf('?') > 0)
                {
                    rawUrl = rawUrl.Split(new char[] { '?' })[0];
                }
                if (url.IndexOf('?') > 0)
                {
                    string[] strArray = url.Split(new char[] { '?' });
                    url = VirtualPathUtility.Combine(rawUrl, strArray[0]) + "?" + strArray[1];
                    return url;
                }
                url = VirtualPathUtility.Combine(rawUrl, url);
            }
            return url;
        }



        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }


        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }


        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            if ("".Equals(GetQueryString(strName)))
            {
                return GetFormString(strName);
            }
            else
            {
                return GetQueryString(strName);
            }
        }


        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值,并过滤非法字符
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetSafeString(string strName)
        {
            string val = "";
            if ("".Equals(GetQueryString(strName)))
            {
                val = GetFormString(strName);
            }
            else
            {
                val = GetQueryString(strName);
            }

            val = val.Replace("'", "‘").Trim();
            val = StringHelper.FilterScript(val, "a,body,script,iframe,object");
            return val;
        }


        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName, int defValue)
        {
            return DataConversion.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获得指定Url参数的long类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的long类型值</returns>
        public static long GetQueryLong(string strName)
        {
            return GetQueryLong(strName, 0);
        }
        /// <summary>
        /// 获得指定Url参数的long类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static long GetQueryLong(string strName, long defValue)
        {
            return DataConversion.StrToLong(HttpContext.Current.Request.QueryString[strName], defValue);
        }


        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return DataConversion.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的long类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的long类型值</returns>
        public static long GetFormLong(string strName, long defValue)
        {
            return DataConversion.StrToLong(HttpContext.Current.Request.Form[strName], defValue);
        }



        /// <summary>
        /// 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static int GetInt(string strName)
        {
            return GetInt(strName, 0);
        }
        /// <summary>
        /// 获得指定Url或表单参数的long类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的long类型值</returns>
        public static long GetLong(string strName, long defValue)
        {
            if (GetQueryLong(strName, defValue) == defValue)
            {
                return GetFormLong(strName, defValue);
            }
            else
            {
                return GetQueryLong(strName, defValue);
            }

        }

        /// <summary>
        /// 获得指定Url或表单参数的long类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <returns>Url或表单参数的long类型值</returns>
        public static long GetLong(string strName)
        {
            return GetLong(strName, 0);
        }
        /// <summary>
        /// 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static int GetInt(string strName, int defValue)
        {
            if (GetQueryInt(strName, defValue) == defValue)
            {
                return GetFormInt(strName, defValue);
            }
            else
            {
                return GetQueryInt(strName, defValue);
            }
        }



        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return DataConversion.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        }


        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return DataConversion.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static float GetFloat(string strName, float defValue)
        {
            if (GetQueryFloat(strName, defValue) == defValue)
            {
                return GetFormFloat(strName, defValue);
            }
            else
            {
                return GetQueryFloat(strName, defValue);
            }
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            return GetIP(HttpContext.Current);

        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP(HttpContext context)
        {
            string result = String.Empty;
            if (null == context)
            {
                return "0.0.0.0";
            }
            result = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = context.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = context.Request.UserHostAddress;
            }

            if (null == result || result == String.Empty || !WebUtils.IsIP(result))
            {
                return "0.0.0.0";
            }

            return result;

        }


        /// <summary>
        /// 获取Form的参数和值拼接后的字段串
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetFormString(HttpContext context)
        {
            string re = context.Request.Form.ToString();
            return re;
        }

        /// <summary>
        /// 获取request的参数和值拼接后的字段串
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetQueryString(HttpContext context)
        {
            string re = context.Request.QueryString.ToString();
            return re;
        }


        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }


        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);

        }



        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);

        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }

            return "";
        }


        /// <summary>
        /// 读Session值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>Session值</returns>
        public static string GetSession(string strName)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[strName] != null)
            //if ( HttpContext.Current.Session[strName] != null)
            {
                string value = HttpContext.Current.Session[strName].ToString();
                return value;
            }

            return WriteSession(strName, "");
        }

        /// <summary>
        /// 写入Session值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>Session值</returns>
        public static string WriteSession(string strName, string value)
        {
            if (HttpContext.Current == null)
            {
                return "";
            }
            HttpContext.Current.Session[strName] = value;
            return value;
        }

        /// <summary>
        /// 清除Session
        /// </summary>
        /// <param name="name">name of Session</param>
        public static void RemoveSession(string name)
        {
            if (HttpContext.Current.Session[name] != null)
            {
                HttpContext.Current.Session.Remove(name);
            }
        }


        /// <summary>
        ///  读Session值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="defaultValue">缺省值</param>
        /// <returns>Session值</returns>
        public static int GetSessionInt(string strName, int defaultValue)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[strName] != null)
            {
                return DataConversion.StrToInt(HttpContext.Current.Session[strName]);
            }

            return defaultValue;
        }



        /// <summary>
        /// 清除cookie
        /// </summary>
        /// <param name="name">name of cookie</param>
        public static void RemoveCookie(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                HttpContext.Current.Request.Cookies.Remove(name);
            }
        }


        #endregion

        #region Ip类

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        #endregion

        #region Application

        /// <summary>
        /// 写入Application
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        public static string WriteApplication(string strName, string strValue)
        {

            HttpContext.Current.Application[strName] = strValue;
            return strValue;
        }

        /// <summary>
        /// ReadApplication
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string ReadApplication(string strName)
        {

            if (HttpContext.Current.Application[strName] != null)
                return HttpContext.Current.Application[strName].ToString();
            return WriteApplication(strName, "");
        }

        /// <summary>
        /// 清除Application
        /// </summary>
        /// <param name="strName"></param>
        public static void RemoveApplication(string strName)
        {
            if (HttpContext.Current.Application[strName] != null)
                HttpContext.Current.Application.Remove(strName);
        }

        #endregion

        #region 上传类

        /// <summary>
        /// 抓取远程图片保存到本地
        /// </summary>
        /// <param name="url">远程地地址</param>
        /// <param name="destDirPath">保存目录地址(物理路径)</param>
        /// <param name="destPath">保存地址</param>
        /// <param name="msg">返回信息</param>
        /// <returns>是否成功</returns>
        public static bool CatchRemoteImage(string url, string destDirPath, out string destPath, out string msg)
        {
            if (!destDirPath.EndsWith("\\"))
                destDirPath = destDirPath + "\\";
            destPath = "";
            msg = "没有图片";

            bool re = false;
            //自动保存远程图片
            WebClient client = new WebClient();



            Regex regName = new Regex(@"\w+.(?:jpg|gif|bmp|png)", RegexOptions.IgnoreCase);

            destPath = destDirPath + regName.Match(url).ToString();

            try
            {
                //保存图片
                client.DownloadFile(url, destPath);
                msg = "远程图片保存成功"; ;
                re = true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                re = false;
            }
            finally
            {
                client.Dispose();
            }

            return re;
        }


        /// <summary>
        /// 获取图片并保存到本地
        /// </summary>
        /// <param name="imgUrl">要抓取的图片Url eg. http://operatorchan.org/v/src/v44446_f35-cutaway.jpg</param>
        /// <param name="path">要保存的文件夹路径 eg. d:\</param>
        /// <param name="fileName">要保存的文件名</param>
        /// <param name="imgType">返回生成图片的文件扩展名eg. ".jpg", ".jpeg", ".png", ".gif", ".bmp"</param>
        /// <returns>成功为1,失败为0</returns>
        public static int SaveImageFromWeb(string imgUrl, string path, string fileName, ref string imgType)
        {
            if (path.Equals(""))
                return 0;

            string imgName = imgUrl.ToString().Substring(imgUrl.ToString().LastIndexOf("/") + 1);
            string defaultType = ".jpg";
            string[] imgTypes = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            imgType = imgUrl.ToString().Substring(imgUrl.ToString().LastIndexOf("."));
            foreach (string it in imgTypes)
            {
                if (imgType.ToLower().Equals(it))
                    break;
                if (it.Equals(".bmp"))
                    imgType = defaultType;
            }
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
                request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
                request.Timeout = 3000;

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();

                if (response.ContentType.ToLower().StartsWith("image/"))
                {
                    byte[] arrayByte = new byte[1024];
                    int imgLong = (int)response.ContentLength;
                    int l = 0;

                    if (fileName == "")
                        fileName = imgName;

                    FileStream fso = new FileStream(path + fileName + imgType, FileMode.Create);
                    while (l < imgLong)
                    {
                        int i = stream.Read(arrayByte, 0, 1024);
                        fso.Write(arrayByte, 0, i);
                        l += i;
                    }

                    fso.Close();
                    stream.Close();
                    response.Close();

                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }


        #endregion

        #region Helper
        /// <summary>
        /// 向页面输出内容
        /// </summary>
        /// <param name="sb">内容</param>
        public static void ResponseHTML(System.Text.StringBuilder sb)
        {
            ResponseHTML(sb.ToString());
        }
        /// <summary>
        /// 向页面输出内容
        /// </summary>
        /// <param name="sb">内容</param>
        public static void ResponseHTML(string content)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "text/html";
            System.Web.HttpContext.Current.Response.Expires = 0;
            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Response.Write(content);
            System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 向页面输出内容
        /// </summary>
        /// <param name="obj">对象</param>
        public static void ResponseJson(object obj)
        {
            string content = JsonConvert.SerializeObject(obj);
            ResponseHTML(content);
        }

        #endregion


        #region 下载文件



        /// <summary>
        /// 下载远程文件并输出到客户端
        /// </summary>
        /// <param name="urlString">远程文件地址</param>
        /// <param name="fileName">文件名</param>
        public static void RemoteFileDownload(string urlString, string fileName)
        {
            byte[] fileData;
            using (WebClient client = new WebClient())
            {
                fileData = client.DownloadData(urlString);
            }
            if (fileName == string.Empty)
            {
                fileName = Path.GetFileName(urlString);
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
            HttpContext.Current.Response.AppendHeader("Content-Length", fileData.Length.ToString());
            HttpContext.Current.Response.BinaryWrite(fileData);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        #endregion

        //#region 改变淘宝或者拍拍的图片大小
        ///// <summary>
        ///// 根据图片的路径判断是否该片图片的大小
        ///// </summary>
        ///// <param name="picurl">图片的路径</param>
        ///// <returns></returns>
        //public static string GetImg(object picurl)
        //{
        //    string Sys_Url_Host = "";
        //    try
        //    {
        //        Sys_Url_Host = Config;//获取网站的域名
        //    }
        //    catch (Exception)
        //    {
        //        ;
        //    }
        //    string url = Sys_Url_Host + "/upload/";
        //    string pic = "";
        //    if (picurl != null)
        //    {
        //        pic = picurl.ToString();
        //        if (pic.IndexOf("taobao") != -1)
        //        {
        //            if (pic.IndexOf("120x120.jpg") != -1)
        //            {
        //                pic = pic.Replace("120x120.jpg", "310x310.jpg");
        //            }
        //        }
        //        else if (pic.IndexOf("paipai") != -1)
        //        {
        //            if (pic.IndexOf("120x120.jpg") != -1)
        //            {
        //                pic = pic.Replace("120x120.jpg", "300x300.jpg");
        //            }
        //        }
        //        else if (pic == "pro_Simg.jpg")
        //        {
        //            pic = "pro_Simg.jpg";
        //        }
        //        else
        //        {
        //            ;
        //        }
        //    }
        //    else
        //    {
        //        url += "pro_Simg.jpg";
        //    }
        //    return url + pic;
        //}
        //#endregion

        #region 为图片加前缀适应不同域名
        public static string AddPicPrefix(object pic)
        {
            string resultPic = "";
            if (pic != null)
            {
                resultPic = pic.ToString();
                if (resultPic.Length > 2)
                {
                    if ((resultPic.IndexOf("http") == -1) && (resultPic.Substring(0, 2) != ".."))
                    {
                        if (resultPic.Substring(0, 1) == "/")
                            resultPic = ".." + resultPic;
                        else
                            resultPic = "../" + resultPic;
                    }
                }
            }
            return resultPic;
        }
        #endregion

        #region 创建自定义表
        /// <summary>
        /// 创建DataTable
        /// by 郑盛表 2012-12-20
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Fields">自定义字段</param>
        /// <returns></returns>
        public static DataTable CreateCustomTable(string TableName, string Fields)
        {
            return CreateCustomTable(TableName, Fields.Split(','));
        }
        /// <summary>
        /// 创建DataTable
        /// by 郑盛表 2012-12-20
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Fields">自定义字段</param>
        /// <returns></returns>
        /// 
        public static DataTable CreateCustomTable(string TableName, string[] Fields)
        {
            DataTable dt = new DataTable(TableName);
            for (int i = 0; i < Fields.Length; i++)
            {
                DataColumn addcol = new DataColumn(Fields[i], Type.GetType("System.String"));
                dt.Columns.Add(addcol);

            }
            return dt;
        }
        #endregion

        #region 识别对象是否为空-郑盛表

        public static bool IsNull(string obj)
        {
            if (null == obj || 0 >= obj.Trim().Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNull(char obj)
        {
            if (0 >= obj.ToString().Trim().Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNull(Object obj)
        {
            if (null == obj)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNull(System.Web.UI.HtmlControls.HtmlInputText obj)
        {
            if (null == obj || 0 >= obj.Value.Trim().Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNull(System.Web.UI.HtmlControls.HtmlInputHidden obj)
        {
            if (null == obj || 0 >= obj.Value.Trim().Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNull(System.Web.UI.WebControls.TextBox obj)
        {
            if (null == obj || 0 >= obj.Text.Trim().Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNull(DataSet obj)
        {
            return IsNull(obj, false);
        }

        public static bool IsNull(DataSet obj, bool b)
        {
            if (b)
            {
                if (null == obj || 0 >= obj.Tables.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (null == obj || 0 >= obj.Tables.Count || 0 >= obj.Tables[0].Rows.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsNull(DataTable obj)
        {
            return IsNull(obj, false);
        }

        public static bool IsNull(DataTable obj, bool b)
        {
            if (b)
            {
                if (null == obj)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (null == obj || 0 >= obj.Rows.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsNull(DataRowCollection obj)
        {
            if (null == obj)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion

        #region HTML

        /// <summary>
        /// 移除HTML标签
        /// 郑盛表 2013-02-27
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string RemoveHtml(string strHtml)
        {
            string[] aryReg ={
     @"<script[^>]*?>.*?</script>",
     @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(file://[""'tbnr]|[^/7])*?/7|/w+)|.{0})|/s)*?(///s*)?>",
     @"([\r\n])[\s]+",
     @"&(quot|#34);",
     @"&(amp|#38);",
     @"&(lt|#60);",
     @"&(gt|#62);",
     @"&(nbsp|#160);",
     @"&(iexcl|#161);",
     @"&(cent|#162);",
     @"&(pound|#163);",
     @"&(copy|#169);",
     @"&#(\d+);",
     @"-->",
     @"<!--.*\n",
     @"<[^>]*>"
    };

            string[] aryRep = {
      "",
      "",
      "",
      "\"",
      "&",
      "<",
      ">",
      " ",
      "\xa1",//chr(161),   
      "\xa2",//chr(162),   
      "\xa3",//chr(163),   
      "\xa9",//chr(169),   
      "",
      "\r\n",
      "",
      ""
     };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");


            return strOutput;
        }

        #endregion

        #region 去除单引号
        public static string ChkSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("'", "''");
                str2 = str;
            }
            return str2;
        }

        #endregion


        #region 静态的哈希表

        /// <summary>
        /// 定义一个静态的哈希表，存储下载的进度
        /// 2012-2-24 by jiewen
        /// </summary>
        public static Hashtable ht_Down_Task_Progress = new Hashtable();
        /// <summary>
        /// 添加进度，每生成一条数据，进度+1
        /// </summary>
        /// <param name="TaskId">任务ID</param>
        public static void add_Down_Task_Progress(string TaskId)
        {
            ht_Down_Task_Progress[TaskId] = DataConversion.StrToInt(ht_Down_Task_Progress[TaskId], 0) + 1;
        }
        /// <summary>
        /// 读取任务进度
        /// </summary>
        /// <param name="TaskId">任务ID</param>
        /// <returns>返回进度</returns>
        public static int get_Down_Task_Progress(string TaskId)
        {
            return DataConversion.StrToInt(ht_Down_Task_Progress[TaskId], 0);
        }
        /// <summary>
        /// 读取该任务总的记录数
        /// </summary>
        /// <param name="TaskId"></param>
        public static int get_Down_Task_Total(string TaskId)
        {
            return DataConversion.StrToInt(ht_Down_Task_Progress[TaskId + "_Total"], 0);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="TaskId">任务ID</param>
        public static void clear_Down_Task_Progress(string TaskId)
        {
            ht_Down_Task_Progress.Remove(TaskId);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="TaskId">任务ID</param>
        public static void clear_Down_Task_Total(string TaskId)
        {
            ht_Down_Task_Progress.Remove(TaskId + "_Total");
        }
        #endregion

        /// <summary>
        /// 将枚举转换成字典
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static Dictionary<string, string> Add_RightCode(Type enumType)
        {
            if (enumType.IsEnum == false) { return null; }

            var list = new Dictionary<string, string>();

            System.Reflection.FieldInfo[] fields = enumType.GetFields();

            foreach (FieldInfo field in fields)
            {
                string text = string.Empty, value = string.Empty;
                if (field.IsSpecialName) continue;
                value = field.GetRawConstantValue().ToString();
                text = field.Name;
                list.Add(text, value);
            }
            return list;
        }

        #region Post And Get
        public static string WebRequestGet(string url)
        {
            HttpWebRequest request = GetWebRequest(url, "GET");
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse rsp = (HttpWebResponse)request.GetResponse();
            Encoding encoding = System.Text.Encoding.UTF8;
            if (!string.IsNullOrEmpty(rsp.CharacterSet))
            {
                encoding = Encoding.GetEncoding(rsp.CharacterSet);
            }
            return GetResponseString(rsp, encoding);
        }

        /// <summary>
        /// 提交Get请求并获取输出内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters">提交参数</param>
        /// <returns></returns>
        public static string WebRequestGet(string url, IDictionary<string, string> parameters)
        {
            if ((parameters != null) && (parameters.Count > 0))
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildPostData(parameters);
                }
                else
                {
                    url = url + "?" + BuildPostData(parameters);
                }
            }

            return WebRequestGet(url);
        }

        /// <summary>
        /// 提交Post请求并获取输出内容
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="formDataDict">提交参数</param>
        /// <returns></returns>
        public static string WebRequestPost(string url, IDictionary<string, string> formDataDict)
        {
            string strRequestData = BuildPostData(formDataDict);
            return WebRequestPost(url, strRequestData);
        }

        /// <summary>
        /// 提交Post请求并获取输出内容
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="param">提交参数。示例 key1=value1&key2=value2</param>
        /// <returns></returns>
        public static string WebRequestPost(string url, string param)
        {
            if (url == "")
            {
                return "url未配置";
            }

            //请求远程HTTP
            string strResult = "";
            try
            {
                //填充POST数据
                HttpWebRequest webRequest = GetWebRequest(url, "POST");
                webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                byte[] bytes = Encoding.UTF8.GetBytes(param);//把数组转换成流中所需字节数组类型
                Stream requestStream = webRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();//释放
                HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();//发送POST数据请求服务器
                Encoding encoding = System.Text.Encoding.UTF8;
                if (!string.IsNullOrEmpty(rsp.CharacterSet))
                {
                    encoding = Encoding.GetEncoding(rsp.CharacterSet);
                }
                strResult = GetResponseString(rsp, encoding);//获取服务器返回信息
            }
            catch (Exception exp)
            {
                strResult = "报错：" + exp.Message;
            }
            return strResult;
        }


        /// <summary>
        /// 获取输出字符串内容
        /// </summary>
        /// <param name="rsp"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static string GetResponseString(HttpWebResponse rsp, Encoding encoding)
        {
            string strResult = string.Empty;
            Stream responseStream = null;
            StreamReader reader = null;
            try
            {
                responseStream = rsp.GetResponseStream();
                reader = new StreamReader(responseStream, encoding);
                strResult = reader.ReadToEnd();
                reader.Close();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
            return strResult;
        }
        /// <summary>
        ///  把参数添加到post请求流里面
        /// </summary>
        /// <param name="memStream">MemoryStream</param>
        /// <param name="formDataDict"></param>
        /// <param name="boundary"></param>
        private static void AddFormData(MemoryStream memStream, IDictionary<string, string> formDataDict, string boundary)
        {
            //没有form参数的情况下，直接返回
            if (formDataDict == null || formDataDict.Keys.Count == 0)
            {
                return;
            }

            // 写入字符串的Key  
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}";

            //将需要提交的form的参数信息，设置到post请求流里面
            foreach (byte[] formitembytes in from string key in formDataDict.Keys
                                             select string.Format(stringKeyHeader, key, formDataDict[key])
                                                 into formitem
                                             select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

        }

        private static bool CheckValidationResult(object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors errors)
        {
            return true;// Always accept
        }

        /// <summary>
        /// 生成请求字符串.格式为 key1=value1&key2=value2 .
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static string BuildPostData(IDictionary<string, string> parameters)
        {
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            IEnumerator<KeyValuePair<string, string>> enumerator = parameters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                string key = current.Key;
                string str2 = current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(str2))
                {
                    if (flag)
                    {
                        builder.Append("&");
                    }
                    builder.Append(key);
                    builder.Append("=");
                    builder.Append(HttpUtility.UrlEncode(str2));
                    flag = true;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取一个HttpWebRequest对象
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="method">GET 或 POST</param>
        /// <returns></returns>
        public static HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest request = null;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create(url);
            }
            request.ServicePoint.Expect100Continue = false;
            request.Method = method;
            request.KeepAlive = true;
            request.UserAgent = "U1city";
            request.Timeout = 100 * 1000;
            return request;
        }


        #endregion

        #region URL
        /// <summary>
        /// 是否为URL，为空返回false
        /// </summary>
        /// <param name="urlString"></param>
        /// <returns></returns>
        public static bool IsURL(string urlString)
        {
            if (String.IsNullOrEmpty(urlString)) return false;
            Uri uri;
            return Uri.TryCreate(urlString, UriKind.Absolute, out uri)
                && (uri.Scheme == Uri.UriSchemeHttp
                || uri.Scheme == Uri.UriSchemeHttps
                || uri.Scheme == Uri.UriSchemeFtp);
        }
        #endregion
    }
}
