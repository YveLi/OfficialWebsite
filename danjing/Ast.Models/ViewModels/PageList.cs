using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast.Models.ViewModels
{
    public class PageList<TObject>
    {
        /// <summary>
        /// layui表格默认Json格式参数code，默认值为0
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// layui表格默认Json格式参数msg，默认值为""
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// layui表格默认Json格式参数count，记录总数
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<TObject> datalist { get; set; }
        /// <summary>
        /// 构建实例
        /// </summary>
        public PageList()
        {

        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PageList(IEnumerable<TObject> data, int? count)
        {
            this.code = 0;
            this.count = count ?? 0;
            this.msg = "";
            this.datalist = data.ToList();
        }
    }

    public class DataList<SelectData>
    {
        public DataList()
        {

        }
        public DataList(IEnumerable<SelectData> data)
        {
            this.code = 0;
            this.msg = "success";
            this.data = data.ToList();
        }
        public int code { get; set; }
        public string msg { get; set; }
        public List<SelectData> data { get; set; }
    }

    public class SelectData
    {
        public string name { get; set; }
        public int value { get; set; }
        public string selected { get; set; }
        public string disabled { get; set; }

        public IList<SelectData> children { get; set; }
    }
}
