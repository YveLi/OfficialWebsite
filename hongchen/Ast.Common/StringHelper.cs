using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ast.Common
{
    public class StringHelper
    {

        /// <summary>
        /// 使用逗号分割符,扩充字符串
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="append"></param>
        public static void AppendString(StringBuilder sb, string append)
        {
            AppendString(sb, append, ",");
        }
        /// <summary>
        /// 使用逗号分割符,扩充字符串
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="append"></param>
        /// <param name="split"></param>
        public static void AppendString(StringBuilder sb, string append, string split)
        {
            if (sb.Length == 0)
            {
                sb.Append(append);
            }
            else
            {
                sb.Append(split);
                sb.Append(append);
            }
        }

        /// <summary>
        /// 从Base64字符串中还原字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Base64StringDecode(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(bytes);
        }
        /// <summary>
        /// 将字符串保存为Base64编码序列
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Base64StringEncode(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }


        public static bool FoundCharInArr(string checkStr, string findStr)
        {
            return FoundCharInArr(checkStr, findStr, ",");
        }

        public static bool FoundCharInArr(string checkStr, string findStr, string split)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(split))
            {
                split = ",";
            }
            if (string.IsNullOrEmpty(checkStr))
            {
                return false;
            }
            if (checkStr.IndexOf(split) != -1)
            {
                if (findStr.IndexOf(split) != -1)
                {
                    string[] strArray = checkStr.Split(new char[] { Convert.ToChar(split) });
                    string[] strArray2 = findStr.Split(new char[] { Convert.ToChar(split) });
                    foreach (string str in strArray)
                    {
                        foreach (string str2 in strArray2)
                        {
                            if (string.Compare(str, str2) == 0)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            return flag;
                        }
                    }
                    return flag;
                }
                foreach (string str3 in checkStr.Split(new char[] { Convert.ToChar(split) }))
                {
                    if (string.Compare(str3, findStr) == 0)
                    {
                        return true;
                    }
                }
                return flag;
            }
            if (string.Compare(checkStr, findStr) == 0)
            {
                flag = true;
            }
            return flag;
        }


        /// <summary>
        /// 检查一个数组中所有的元素是否有包含于指定字符串的元素
        /// </summary>
        /// <param name="arr">存储数据数据的字串</param>
        /// <param name="toFind">要查找的字符串</param>
        /// <param name="separator">数组的分隔符</param>
        /// <returns></returns>
        public static bool FoundStringInArr(string arr, string toFind, char separator)
        {
            if (arr.IndexOf(separator) >= 0)
            {
                string[] arrTemp = arr.Split(separator);
                for (int i = 0; i < arrTemp.Length; i++)
                {
                    if ((toFind.ToLower().IndexOf(arrTemp[i].ToLower()) >= 0) && (arrTemp[i].ToLower() != ""))
                        return true;
                }

            }
            else
            {
                if ((toFind.ToLower().IndexOf(arr.ToLower())) >= 0 && (arr.ToLower() != ""))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (string.IsNullOrEmpty(strContent))
            {
                strContent = "";
            }
            int i = strContent.IndexOf(strSplit);
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = { strContent };
                return tmp;
            }

            return Regex.Split(strContent, @strSplit.Replace(".", @"\.").Replace("|", @"\|").Replace("$", @"\$").Replace("{", @"\{").Replace("}", @"\}").Replace("^", @"\^").Replace("(", @"\(").Replace(")", @"\)").Replace("]", @"\]").Replace("[", @"\[").Replace("*", @"\*").Replace("+", @"\+").Replace("?", @"\?"));
        }
        /// <summary>
        /// 是否包含中文
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsIncludeChinese(string inputData)
        {
            Regex regex = new Regex("[一-龥]");
            return regex.Match(inputData).Success;
        }

        /// <summary>
        /// 计算MD5散列
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = "";
            }

            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        /// <summary>
        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        /// </summary>
        /// <param name="SourceString"></param>
        /// <param name="SearchString"></param>
        /// <param name="ReplaceString"></param>
        /// <param name="IsCaseInsensetive">true 为指定不区分大小写的匹配。</param>
        /// <returns></returns>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        /// <summary>
        /// 计算哈散列
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SHA1(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                input = "";
            }

            using (SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider())
            {
                return BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 过滤使用使用尖括号括起来的标签,但会保留标签中间的内容。eg. <tr>11</tr>,执行为11
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripTags(string input)
        {
            Regex regex = new Regex("<([^<]|\n)+?>");
            return regex.Replace(input, "");
        }
        /// <summary>
        /// 返回指定长度的子字符串,计算时一个汉字的长度为2；反之为1。
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubString(string sourceString, int length)
        {
            return SubString(sourceString, length, "");
        }

        /// <summary>
        /// 返回指定长度的子字符串
        /// </summary>
        /// <param name="sourceString">源字符</param>
        /// <param name="length">长度</param>
        /// <param name="chineseTo2">为true时，一个汉字的长度为2；反之为1</param>
        /// <returns>字符串</returns>
        public static string SubString(string sourceString, int length, bool chineseTo2)
        {
            return SubString(sourceString, length, "", chineseTo2);
        }

        /// <summary>
        /// 返回指定长度的子字符串,计算时一个汉字的长度为2；反之为1。
        /// </summary>
        /// <param name="sourceString">源字符</param>
        /// <param name="length">长度</param>
        /// <param name="substitute">省略后替代的字符如:"..."等</param>
        /// <returns>字符串</returns>
        public static string SubString(string sourceString, int length, string substitute)
        {
            return SubString(sourceString, length, substitute, true);
        }

        /// <summary>
        /// 返回指定长度的子字符串
        /// </summary>
        /// <param name="sourceString">源字符</param>
        /// <param name="length">长度</param>
        /// <param name="substitute">省略后替代的字符如:"..."等</param>
        /// <param name="chineseTo2">为true时，一个汉字的长度为2；反之为1</param>
        /// <returns>字符串</returns>
        public static string SubString(string sourceString, int length, string substitute, bool chineseTo2)
        {
            if (string.IsNullOrEmpty(sourceString))
                return "";
            if (Encoding.Default.GetBytes(sourceString).Length <= length)
            {
                return sourceString;
            }
            ASCIIEncoding encoding = new ASCIIEncoding();
            length -= Encoding.Default.GetBytes(substitute).Length;
            int num = 0;
            StringBuilder builder = new StringBuilder();
            byte[] bytes = encoding.GetBytes(sourceString);
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0x3f && chineseTo2)
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                if (num > length)
                {
                    break;
                }
                builder.Append(sourceString.Substring(i, 1));
            }
            builder.Append(substitute);
            return builder.ToString();
        }

        /// <summary>
        /// 返回字串长度,中文字符为计2,其他计1
        /// 使用ASCII将字符串转化为字节数组时(byte[]),数组长度跟字符串长度相同
        /// 非中文字符取其ASCII码,中文ASCII码为63即?(十六进制为0x3F)
        /// </summary>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            return Encoding.Default.GetBytes(str).Length;
        }
        /// <summary>
        /// 去除字串两边的空格,当字串为Null Or Empty时,返回 String.Empty
        /// </summary>
        /// <param name="returnStr"></param>
        /// <returns></returns>
        public static string Trim(string returnStr)
        {
            if (!string.IsNullOrEmpty(returnStr))
            {
                return returnStr.Trim();
            }
            return string.Empty;
        }

        #region DateTime 方法

        /// <summary>
        /// 返回相差的秒数
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="Sec"></param>
        /// <returns></returns>
        public static int StrDateDiffSeconds(string Time, int Sec)
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse(Time).AddSeconds(Sec);
            if (ts.TotalSeconds > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalSeconds < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalSeconds;
        }

        /// <summary>
        /// 返回相差的分钟数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static int StrDateDiffMinutes(string time, int minutes)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddMinutes(minutes);
            if (ts.TotalMinutes > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalMinutes < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalMinutes;
        }

        /// <summary>
        /// 返回相差的小时数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static int StrDateDiffHours(string time, int hours)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddHours(hours);
            if (ts.TotalHours > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalHours < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalHours;
        }

        #endregion


        /// <summary>
        /// 替换回车换行符为html换行符
        /// </summary>
        public static string StrFormat(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
                str2 = str;
            }
            return str2;
        }


        /// <summary>
        /// 转义json字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeJsonString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = str.Replace("\r\n", "<br/>");
            str = str.Replace("\r", "<br/>");
            return str.Replace("\"", "\\\"");
        }
        /// <summary>
        /// 在生成CSV时替换字符串中特殊字符
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ReplaceCSVBadString(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;
            return content.Replace(",", "，").Replace("\r\n", "").Replace("\n", "").Replace("\"", "“");
        }


        /// <summary>
        /// 金额转大写
        /// </summary>
        /// <param name="LowerMoney">金额可正负</param>
        /// <returns></returns>
        public static string MoneyToChinese(string LowerMoney)
        {
            if (string.IsNullOrEmpty(LowerMoney))
                return string.Empty;
            string functionReturnValue = null;
            bool IsNegative = false; // 是否是负数
            if (LowerMoney.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                LowerMoney = LowerMoney.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49　　123.4→123.4
            LowerMoney = Math.Round(double.Parse(LowerMoney), 2).ToString();
            if (LowerMoney.IndexOf(".") > 0)
            {
                if (LowerMoney.IndexOf(".") == LowerMoney.Length - 2)
                {
                    LowerMoney = LowerMoney + "0";
                }
            }
            else
            {
                LowerMoney = LowerMoney + ".00";
            }
            strLower = LowerMoney;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            functionReturnValue = strUpper;

            if (IsNegative == true)
            {
                return "负" + functionReturnValue;
            }
            else
            {
                return functionReturnValue;
            }
        }


        /// <summary>
        /// 过滤标签,正则匹配时使用非贪婪模式
        /// </summary>
        /// <param name="conStr">待处理的文本数据</param>
        /// <param name="tagName">标签名称如,html,Script等</param>
        /// <param name="fType">过滤方式,可以取(1|2|3)
        /// 1:是单个标签如img等,
        /// 2:表示配对出现的标签如div等将清除此标签以及标签内的全部文本,
        /// 3:表示也是针对配对出现的标签,但是保留标签内的内容.
        /// </param>
        /// <returns></returns>
        public static string CollectionFilter(string conStr, string tagName, int fType)
        {
            if (string.IsNullOrEmpty(conStr))
                return string.Empty;

            string input = conStr;
            switch (fType)
            {
                case 1:
                    return Regex.Replace(input, "<" + tagName + "([^>])*>", "", RegexOptions.IgnoreCase);

                case 2:
                    return Regex.Replace(input, "<" + tagName + "([^>])*>.*?</" + tagName + "([^>])*>", "", RegexOptions.IgnoreCase);

                case 3:
                    return Regex.Replace(Regex.Replace(input, "<" + tagName + "([^>])*>", "", RegexOptions.IgnoreCase), "</" + tagName + "([^>])*>", "", RegexOptions.IgnoreCase);
            }
            return input;
        }

        /// <summary>
        /// 过滤标签
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="filterItem">标签项，多个用英文逗号隔开</param>
        /// <returns></returns>
        public static string FilterScript(string conStr, string filterItem)
        {
            if (string.IsNullOrEmpty(conStr))
                return string.Empty;
            filterItem = filterItem.ToLower();
            string str = conStr.Replace("\r", "{$Chr13}").Replace("\n", "{$Chr10}");
            foreach (string str2 in filterItem.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                switch (str2)
                {
                    case "body":
                        str = CollectionFilter(str, str2, 3);
                        break;
                    case "iframe":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "object":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "script":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "style":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "div":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "span":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "table":
                        str = CollectionFilter(CollectionFilter(CollectionFilter(CollectionFilter(CollectionFilter(str, str2, 3), "Tbody", 3), "Tr", 3), "Td", 3), "Th", 3);
                        break;

                    case "img":
                        str = CollectionFilter(str, str2, 1);
                        break;

                    case "font":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "a":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "p":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "html":
                        str = StripTags(str);
                        break;
                }
            }
            return str.Replace("{$Chr13}", "\r").Replace("{$Chr10}", "\n");
        }

        /// <summary>
        /// xml编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string XmlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("&", "&amp;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("'", "&apos;");
                str = str.Replace("\"", "&quot;");
            }
            return str;
        }

        /// <summary>
        /// 过滤编辑器的危险文本
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetEditorSafeContent(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            else
            {
                str = StringHelper.FilterScript(str, "Script,Iframe,Object,Body");
                return str.Trim();
            }
        }
        #region 返回字串的拼音首字母序列

        /// <summary>
        /// 返回字串的拼音首字母序列(字串中的英文直接返回)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetInitial(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                builder.Append(GetOneIndex(str.Substring(i, 1)));
            }
            return builder.ToString();
        }

        private static string GetOneIndex(string testTxt)
        {
            if (string.IsNullOrEmpty(testTxt))
            {
                return "";
            }
            if ((Convert.ToChar(testTxt) >= '\0') && (Convert.ToChar(testTxt) < 'Ā'))
            {
                return testTxt;
            }
            return GetGbkX(testTxt);
        }
        /// <summary>
        /// 返回字符串首字符,如果字符串首字符是汉字则返回其拼音的第一个字符(A-Z)
        /// </summary>
        /// <param name="testTxt"></param>
        /// <returns></returns>
        private static string GetGbkX(string testTxt)
        {
            if (string.IsNullOrEmpty(testTxt))
            {
                return "";
            }

            if (testTxt.CompareTo("吖") >= 0)
            {
                if (testTxt.CompareTo("八") < 0)
                {
                    return "A";
                }
                if (testTxt.CompareTo("嚓") < 0)
                {
                    return "B";
                }
                if (testTxt.CompareTo("咑") < 0)
                {
                    return "C";
                }
                if (testTxt.CompareTo("妸") < 0)
                {
                    return "D";
                }
                if (testTxt.CompareTo("发") < 0)
                {
                    return "E";
                }
                if (testTxt.CompareTo("旮") < 0)
                {
                    return "F";
                }
                if (testTxt.CompareTo("铪") < 0)
                {
                    return "G";
                }
                if (testTxt.CompareTo("讥") < 0)
                {
                    return "H";
                }
                if (testTxt.CompareTo("咔") < 0)
                {
                    return "J";
                }
                if (testTxt.CompareTo("垃") < 0)
                {
                    return "K";
                }
                if (testTxt.CompareTo("嘸") < 0)
                {
                    return "L";
                }
                if (testTxt.CompareTo("拏") < 0)
                {
                    return "M";
                }
                if (testTxt.CompareTo("噢") < 0)
                {
                    return "N";
                }
                if (testTxt.CompareTo("妑") < 0)
                {
                    return "O";
                }
                if (testTxt.CompareTo("七") < 0)
                {
                    return "P";
                }
                if (testTxt.CompareTo("亽") < 0)
                {
                    return "Q";
                }
                if (testTxt.CompareTo("仨") < 0)
                {
                    return "R";
                }
                if (testTxt.CompareTo("他") < 0)
                {
                    return "S";
                }
                if (testTxt.CompareTo("哇") < 0)
                {
                    return "T";
                }
                if (testTxt.CompareTo("夕") < 0)
                {
                    return "W";
                }
                if (testTxt.CompareTo("丫") < 0)
                {
                    return "X";
                }
                if (testTxt.CompareTo("帀") < 0)
                {
                    return "Y";
                }
                if (testTxt.CompareTo("咗") < 0)
                {
                    return "Z";
                }
            }
            return testTxt;
        }

        #endregion

        /// <summary>
        /// 后端加密富文本
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("&;", "&amp;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("'", "&apos;");
                str = str.Replace("\"", "&quot;");
            }
            return str;
        }
        /// <summary>
        /// 前端解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("&amp;", "&;");
                str = str.Replace("&lt;", "<");
                str = str.Replace("&gt;", ">");
                str = str.Replace("&apos;", "'");
                str = str.Replace("&quot;", "\"");
            }
            return str;
        }
    }
}
