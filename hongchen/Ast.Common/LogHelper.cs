using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ast.Common
{
    /// <summary>
    ///  日志类 By KeHen 2018.3.31
    /// </summary>
    public static class LogHelper
    {
        public static void WriteLog()
        {
            //开启线程池 用独立的线程专门处理这错误 
            ThreadPool.QueueUserWorkItem(s =>
            {
                while (true)
                {
                    if (ErrorQueue.Count <= 0) continue;

                    string msg = ErrorQueue.Dequeue(); //从队列中取出消息

                    ILog log = LogManager.GetLogger("SysAppender");
                    log.Debug(msg);

                    Thread.Sleep(1000);
                }
            });
        }

        public static void WriteLog(string msg)
        {
            // log4net 内部已处理并发的场景
            ILog log = LogManager.GetLogger("SysAppender");
            log.Debug(msg);
        }

        public static void Info(string msg)
        {
            // log4net 内部已处理并发的场景
            ILog log = LogManager.GetLogger("SysAppender");
            log.Debug(msg);
        }

        private static readonly Queue<string> ErrorQueue = new Queue<string>();

        public static void AddError(string msg)
        {
            ErrorQueue.Enqueue(msg);
        }
    }
}
