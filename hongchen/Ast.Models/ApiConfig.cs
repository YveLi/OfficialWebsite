using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast.Models
{
    public class ApiConfig
    {
        /// <summary>
        /// 状态 0-失败 1-成功
        /// </summary>
        public enum Status
        {
            Fail = -1,
            Success = 1,
            Wrong = -2,
        }

        /// <summary>
        /// 跨域请求异步参数
        /// </summary>
        public const string AjaxCallParams = "Ast";

    }
}
