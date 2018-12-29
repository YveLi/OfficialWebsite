using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast.Models
{
    public partial class dt_users
    {

        /// <summary>
        /// 自增ID
        /// </summary>		
        public int id { get; set; }

        /// <summary>
        /// 
        /// </summary>		
        public int group_id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>		
        public string user_name { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>		
        public string password { get; set; }

        /// <summary>
        /// 6位随机字符串,加密用到
        /// </summary>		
        public string salt { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>		
        public string email { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>		
        public string nick_name { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>		
        public string avatar { get; set; }

        /// <summary>
        /// 用户性别
        /// </summary>		
        public string sex { get; set; }

        /// <summary>
        /// 生日
        /// </summary>		
        public DateTime birthday { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>		
        public string telphone { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>		
        public string mobile { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>		
        public string qq { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>		
        public string address { get; set; }

        /// <summary>
        /// 安全问题
        /// </summary>		
        public string safe_question { get; set; }

        /// <summary>
        /// 问题答案
        /// </summary>		
        public string safe_answer { get; set; }

        /// <summary>
        /// 预存款
        /// </summary>		
        public decimal amount { get; set; }

        /// <summary>
        /// 用户积分
        /// </summary>		
        public int point { get; set; }

        /// <summary>
        /// 经验值
        /// </summary>		
        public int exp { get; set; }

        /// <summary>
        /// 用户状态,0正常,1待验证,2待审核,3锁定
        /// </summary>		
        public byte status { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>		
        public DateTime reg_time { get; set; }

        /// <summary>
        /// 注册IP
        /// </summary>		
        public string reg_ip { get; set; }

        /// <summary>
        /// 是否是微信号，1、是 0、不是
        /// </summary>		
        public byte isweixin { get; set; }

        /// <summary>
        /// 微信用户表主键Id
        /// </summary>		
        public int wid { get; set; }

        /// <summary>
        /// 
        /// </summary>		
        public string wxOpenId { get; set; }

        /// <summary>
        /// 
        /// </summary>		
        public string wxName { get; set; }

        /// <summary>
        /// 推荐者Id
        /// </summary>		
        public int recommendId { get; set; }

    }
}
