using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast.Models
{
    public class ApiRoot
    {
        /// <summary>
        /// 状态编 0-失败 -1成功
        /// </summary>
        public int Tag { get; set; }
        /// <summary>
        /// 返回消息备注
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回结果 用于存放序列化的实体
        /// </summary>
        public object Result { get; set; }

        public ApiRoot()
        {
            Tag = 0;
            Message = "失败！";
        }

    }
    public class ApiRoot<T> : ApiRoot
    {

    }
}
