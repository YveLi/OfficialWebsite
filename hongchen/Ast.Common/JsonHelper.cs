using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast.Common
{
    public class JsonHelper
    {
        /// <summary>
        /// 跨域请求前端请求格式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonpFormat(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
