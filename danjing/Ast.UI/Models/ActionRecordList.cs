using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ast.UI.Models
{
    public partial class ActionRecordList
    {

        /// <summary>
        /// 主键ID
        /// </summary>		
        public int Id { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>		
        public int CustomerId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>		
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>		
        public string Memo { get; set; }

        /// <summary>
        /// 沟通日期
        /// </summary>		
        public DateTime ActionTime { get; set; }

        /// <summary>
        /// 下次沟通日期
        /// </summary>		
        public DateTime NextTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>		
        public int CreateUser { get; set; }

        /// <summary>
        /// OTAMSGUID
        /// </summary>		
        public string OTAMSGUID { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>		
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime ModifyTime { get; set; }

        public string CreateUserName { get; set; }
    }

    public class actionList
    {
        public ActionRecordList action { get; set; }
    }
}