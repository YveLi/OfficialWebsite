using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast.Common
{
    public class ConfigHelp
    {

        /// <summary>
        /// 获取配置文件里的节点值
        /// </summary>
        /// <returns></returns>
        public static string GetAppSettingValue(string valueName)
        {
            string ret = "";
            if (ConfigurationManager.AppSettings[valueName] != null)
            {
                ret = ConfigurationManager.AppSettings[valueName].ToString();
            }

            return ret;
        }


        /// <summary>
        /// 读取AppSettings值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettings(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
        }

        /// <summary>
        /// 读取WebApiUrl 地址
        /// </summary>
        /// <returns></returns>
        public static string GetWebApiUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["WebApiUrl"].ToString();
        }
    }
    public class TxtLog
    {
        /// <summary>
        /// 创建错误日志
        /// </summary>
        /// <param name="path">日志地址 在当前工作目录+ErrorLog下</param>
        /// <param name="strFunctionName">问题出现的模块</param>
        /// <param name="strErrorMethod">问题出现的方法</param>
        /// <param name="strErrorDescription">错误信息</param>
        public static void CreateErrorLogTxt(string path, string strFunctionName, string strErrorMethod,
            string strErrorDescription)
        {
            //判断是否开启错误日志记录
            if (ConfigHelp.GetAppSettingValue("IsErrorLog").ToString() != "T")
            {
                return;
            }
            string strPath; //错误文件的路径
            DateTime dt = DateTime.Now;
            try
            {

                // 创建日志文件夹 
                strPath = System.Threading.Thread.GetDomain().BaseDirectory + "\\ErrorLog\\" + path;

                if (Directory.Exists(strPath) == false) //工程目录下 Log目录 '目录是否存在,为true则没有此目录
                {
                    Directory.CreateDirectory(strPath); //建立目录　Directory为目录对象
                }
                strPath = strPath + "\\" + dt.ToString("yyyyMM");

                if (Directory.Exists(strPath) == false) //目录是否存在  '工程目录下 Log\月 目录   yyyymm
                {
                    Directory.CreateDirectory(strPath); //建立目录//日志文件，以 日 命名 
                }
                strPath = strPath + "\\" + dt.ToString("dd") + ".log";
                StreamWriter fileWriter = new StreamWriter(strPath, true); //创建日志文件
                fileWriter.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>START");
                fileWriter.WriteLine("Time: " + dt.ToString("HH:mm:ss"));
                fileWriter.WriteLine("问题出现的模块: " + strFunctionName);
                fileWriter.WriteLine("问题出现的方法: " + strErrorMethod);
                fileWriter.WriteLine("错误信息: " + strErrorDescription);
                fileWriter.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<END");
                fileWriter.Close(); //关闭StreamWriter对象
            }
            catch (Exception ex)
            {
                string str = ex.Message.ToString();
            }
        }

        /// <summary>
        /// 创建操作日志
        /// </summary>
        /// <param name="path"></param>
        /// <param name="strFunctionName"></param>
        /// <param name="strErrorMethod"></param>
        /// <param name="strErrorDescription"></param>
        public static void CreateOperateLogTxt(string path, string strFunctionName, string strErrorMethod,
            string strErrorDescription)
        {
            //判断是否开启操作日志记录
            if (ConfigHelp.GetAppSettingValue("IsOperateLog").ToString() != "T")
            {
                return;
            }
            string strPath; //错误文件的路径
            DateTime dt = DateTime.Now;
            try
            {
                // 创建日志文件夹 
                strPath = System.Threading.Thread.GetDomain().BaseDirectory + "OperateLog\\" + path;

                if (Directory.Exists(strPath) == false) //工程目录下 Log目录 '目录是否存在,为true则没有此目录
                {
                    Directory.CreateDirectory(strPath); //建立目录　Directory为目录对象
                }
                strPath = strPath + "\\" + dt.ToString("yyyyMM");

                if (Directory.Exists(strPath) == false) //目录是否存在  '工程目录下 Log\月 目录   yyyymm
                {
                    Directory.CreateDirectory(strPath); //建立目录//日志文件，以 日 命名 
                }
                strPath = strPath + "\\" + dt.ToString("dd") + ".log";
                StreamWriter fileWriter = new StreamWriter(strPath, true); //创建日志文件
                fileWriter.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>START");
                fileWriter.WriteLine("Time: " + dt.ToString("HH:mm:ss"));
                fileWriter.WriteLine("查看的模块: " + strFunctionName);
                fileWriter.WriteLine("查看的方法: " + strErrorMethod);
                fileWriter.WriteLine("日志内容: " + strErrorDescription);
                fileWriter.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<END");
                fileWriter.Close(); //关闭StreamWriter对象
            }
            catch (Exception ex)
            {
                string str = ex.Message.ToString();
            }
        }
    }
}
