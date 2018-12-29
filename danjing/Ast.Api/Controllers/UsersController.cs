using OTAMS.Common;
using OTAMS.IBll;
using OTAMS.Bll;
using OTAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OTAMS.Api.Common;
using SqlSugar;
using Newtonsoft.Json;

namespace OTAMS.Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly ApiRoot _api = new ApiRoot();
        private SqlSugarClient db = SqlSugarManage.GetInstance();
        private Idt_usersService userservice = new dt_usersService();
        private Idt_user_fundService fundservice = new dt_user_fundService();
        private Idt_fund_recordService recordservice = new dt_fund_recordService();
        private Ilife_vipinfoService vipservice = new life_vipinfoService();
        private ILifeRechargeCardListService cardservice = new LifeRechargeCardListService();
        private Ilife_marketinginfo_summaryService moneyservice = new life_marketinginfo_summaryService();
        /// <summary>
        /// 领取VIP卡
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="openid"></param>
        /// <param name="userid"></param>
        [AcceptVerbs("GET", "POST")]
        public void GetVipData(int wid, string openid, string headimgurl)
        {
            try
            {
                var vip = db.Ado.UseStoredProcedure().GetDataTable("p_life_VipInfo", new { openid = openid, wid = wid, headimgurl = headimgurl });
                IList<vipinfo> users = ApiHelper.ModelConvertHelper<vipinfo>.ConvertToModel(vip);
                if (users != null)
                {
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Result = users.SingleOrDefault();
                    _api.Message = "领取VIP成功！";
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "领取VIP失败！";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        /// <summary>
        /// 注册会员
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="userid"></param>
        /// <param name="nickname"></param>
        /// <param name="sex"></param>
        /// <param name="phone"></param>
        /// <param name="birthday"></param>
        /// <param name="constellation"></param>
        /// <param name="ddm"></param>
        [AcceptVerbs("GET", "POST")]
        public void RegisterVipObject(int wid, int userid, string vipid, string vipname, int sex, string phone, string birthday, int constellation, string ddm)
        {
            try
            {
                if (!string.IsNullOrEmpty(phone))
                {
                    var vipinfo = vipservice.GetSingle(o => o.phone == phone);
                    if (vipinfo != null)
                    {
                        _api.Tag = (int)ApiConfig.Status.Fail;
                        _api.Message = "手机已被注册！";
                        WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
                    }
                }
                var vip = db.Ado.UseStoredProcedure().GetDataTable("p_life_RegisterVip", new { vip_id = vipid, vip_name = vipname, sex = sex, phone = phone, birthday = birthday, constellation = constellation, ddm = ddm, wid = wid, userid = userid });

                if (vip != null)
                {
                    var vipinfo = vipservice.GetSingle(o => o.vip_id == vipid);
                    if (vipinfo != null)
                    {
                        _api.Tag = (int)ApiConfig.Status.Success;
                        _api.Result = vipinfo;
                        _api.Message = "注册成功！";
                    }
                    else
                    {
                        _api.Tag = (int)ApiConfig.Status.Fail;
                        _api.Result = vipinfo;
                        _api.Message = "注册失败！";
                    }
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "注册失败！";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        /// <summary>
        /// 会员更新
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="userid"></param>
        /// <param name="vipid"></param>
        /// <param name="vipname"></param>
        /// <param name="sex"></param>
        /// <param name="phone"></param>
        /// <param name="birthday"></param>
        /// <param name="constellation"></param>
        /// <param name="ddm"></param>
        [AcceptVerbs("GET", "POST")]
        public void UpdateVipData(int wid, int userid, string vipid, string vipname, int sex, string phone, string birthday, int constellation, string ddm)
        {
            try
            {
                if (!string.IsNullOrEmpty(phone))
                {
                    var vipinfo = vipservice.GetSingle(o => o.phone == phone && o.vip_id != vipid);
                    if (vipinfo != null)
                    {
                        _api.Tag = (int)ApiConfig.Status.Fail;
                        _api.Message = "手机已被注册！";
                        WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
                    }
                }
                var vip = vipservice.GetSingle(o => o.vip_id == vipid);
                if (vip != null)
                {
                    var vipinfo = vipservice.GetSingle(o => o.vip_id == vipid);
                    vipinfo.vip_name = vipname;
                    vipinfo.sex = sex;
                    vipinfo.phone = phone;
                    vipinfo.birthday = birthday;
                    vipinfo.constellation = constellation;
                    vipinfo.ddm = ddm;
                    var success = vipservice.Update(vipinfo);
                    if (success)
                    {
                        _api.Tag = (int)ApiConfig.Status.Success;
                        _api.Result = vipinfo;
                        _api.Message = "修改成功！";
                    }
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "修改失败！";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }
        /// <summary>
        /// 钱包充值
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="userid"></param>
        /// <param name="openid"></param>
        /// <param name="payprice"></param>
        [AcceptVerbs("GET", "POST")]
        public void WalletRecharge(int wid, int userid, string openid, decimal payprice)
        {
            ApiRoot api = new ApiRoot();
            try
            {
                var user = userservice.GetSingle(o => o.wid == wid && o.id == userid);
                var fund = fundservice.GetSingle(o => o.wId == wid && o.userId == userid);
                if (fund == null || fund.Id == 0)
                {
                    api.Tag = (int)ApiConfig.Status.Fail;
                    api.Message = "获取用户钱包失败！";
                }
                else
                {
                    var code = "Re" + ApiHelper.GetOrderNumber();
                    var fundRecord = new dt_fund_record();
                    fundRecord.wId = wid;
                    fundRecord.userId = userid;
                    fundRecord.userName = user.user_name;
                    fundRecord.changeAmount = payprice;
                    fundRecord.beforeAmount = fund.useAmount;
                    fundRecord.afterAmount = fund.useAmount + payprice;
                    fundRecord.type = ApiHelper.TypeRecord.Recharge;
                    fundRecord.state = ApiHelper.StateRecharge.Committed;
                    fundRecord.addTime = DateTime.Now;
                    fundRecord.updateTime = DateTime.Now;
                    fundRecord.recordCode = code;
                    fundRecord.memo = "钱包充值";
                    var orderid = db.Insertable(fundRecord).ExecuteReturnBigIdentity();
                    api.Message = JsonConvert.SerializeObject(orderid);
                    if (orderid <= 0)
                    {
                        api.Tag = (int)ApiConfig.Status.Fail;
                        api.Message = "充值失败";
                    }
                    else
                    {
                        WechatPayController wechatpay = new WechatPayController();
                        var result = wechatpay.WechatPay(wid, code, openid, code, payprice, "WalletRecharge");
                        if (result.Tag == 1)
                        {
                            api.Result = result.Result;
                            api.Tag = (int)ApiConfig.Status.Success;
                            api.Message = "获取支付配置成功";
                        }
                        else
                        {
                            api.Tag = (int)ApiConfig.Status.Fail;
                            api.Message = "获取失败";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                api.Tag = (int)ApiConfig.Status.Wrong;
                api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(api));
        }

        /// <summary>
        /// 获取钱包详情
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="userid"></param>
        [AcceptVerbs("GET", "POST")]
        public void GetWalletData(int wid, int userid)
        {
            try
            {
                var user = userservice.GetSingle(o => o.wid == wid && o.id == userid);
                if (user == null)
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "不存在此用户！";
                }
                else
                {
                    var fund = fundservice.GetSingle(o => o.wId == wid && o.userId == userid);
                    if (fund == null || fund.Id == 0)
                    {
                        _api.Tag = (int)ApiConfig.Status.Fail;
                        _api.Message = "获取用户钱包失败！";
                    }
                    else
                    {
                        _api.Tag = (int)ApiConfig.Status.Success;
                        _api.Result = fund;
                    }
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
                TxtLog.CreateErrorLogTxt("Users", "UserManage", "GetWalletData", "获取钱包信息失败：" + ex.Message);
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        /// <summary>
        /// 获取钱包星币消费记录
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="userid"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        [AcceptVerbs("GET", "POST")]
        public void GetStarWalletRecord(int wid, int userid, DateTime starttime, DateTime endtime, int index, int size)
        {
            try
            {
                PageModel page = new PageModel()
                {
                    PageIndex = index,
                    PageSize = size
                };
                var recordlist = recordservice.GetPageList(page, o => o.wId == wid && o.userId == userid && o.state == 1 && o.addTime >= starttime && o.addTime <= endtime);
                if (recordlist != null)
                {
                    _api.Result = recordlist;
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Message = "获取成功";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        /// <summary>
        /// 获取现金消费记录
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="userid"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        [AcceptVerbs("GET", "POST")]
        public void GetMoneyWalletRecord(string vipid, DateTime starttime, DateTime endtime, int index, int size)
        {
            try
            {
                PageModel page = new PageModel()
                {
                    PageIndex = index,
                    PageSize = size
                };
                var monrylist = moneyservice.GetPageList(page, o => o.vip_id == vipid && o.pay_time >= starttime && o.pay_time <= endtime);
                if (monrylist != null)
                {
                    _api.Result = monrylist;
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Message = "获取成功";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }
        /// <summary>
        /// 充值卡充值
        /// </summary>
        /// <param name="cardno"></param>
        /// <param name="wid"></param>
        /// <param name="userid"></param>

        [AcceptVerbs("GET", "POST")]
        public void CardRecharge(string cardno, int wid, int userid, string username)
        {
            try
            {
                var card = cardservice.GetSingle(o => o.CardNo == cardno && o.Status == 0);
                if (card != null)
                {
                    card.UserId = userid;
                    card.Wid = wid;
                    card.Status = 1;
                    card.UseTime = DateTime.Now;
                    card.UserName = username;
                    cardservice.Update(card);
                    var fund = fundservice.GetSingle(o => o.userId == userid && o.wId == wid);
                    fund.frozenAmount += card.Amount;
                    fund.amount += card.Amount;
                    fundservice.Update(fund);
                    var fundrecord = new dt_fund_record();
                    fundrecord.state = 1;
                    fundrecord.type = 1;
                    fundrecord.memo = "充值卡充值";
                    fundrecord.recordCode = "Card" + ApiHelper.GetOrderNumber();
                    fundrecord.updateTime = DateTime.Now;
                    fundrecord.userId = userid;
                    fundrecord.wId = wid;
                    fundrecord.userName = username;
                    fundrecord.changeAmount = card.Amount;
                    fundrecord.afterAmount = fund.useAmount;
                    fundrecord.beforeAmount = fund.useAmount;
                    fundrecord.addTime = DateTime.Now;
                    var success = recordservice.Add(fundrecord);
                    if (success)
                    {
                        _api.Tag = (int)ApiConfig.Status.Success;
                        _api.Message = "充值成功！";
                    }
                    else
                    {
                        _api.Tag = (int)ApiConfig.Status.Fail;
                        _api.Message = "充值失败！";
                    }
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "不存在此卡号！";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        /// <summary>
        /// 会员签到详情
        /// </summary>
        [AcceptVerbs("GET", "POST")]
        public void GetSignDetail(string vipid)
        {
            try
            {
                var singdata = db.Ado.UseStoredProcedure().GetDataTable("p_life_SignSummary", new { vip_id = vipid });
                IList<singinfo> sing = ApiHelper.ModelConvertHelper<singinfo>.ConvertToModel(singdata);
                if (sing != null)
                {
                    _api.Result = sing.SingleOrDefault();
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Message = "获取成功！";
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "获取失败！";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        /// <summary>
        /// 会员签到列表
        /// </summary>
        [AcceptVerbs("GET", "POST")]
        public void GetSignList(string vipid, DateTime begintime, DateTime endtime)
        {
            try
            {
                var singdata = db.Ado.UseStoredProcedure().GetDataTable("p_life_SignList", new { vip_id = vipid, begin_time = begintime, end_time = endtime });
                IList<singlist> sing = ApiHelper.ModelConvertHelper<singlist>.ConvertToModel(singdata);
                if (sing != null)
                {
                    _api.Result = sing;
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Message = "获取成功！";
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "获取失败！";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        /// <summary>
        /// 会员签到
        /// </summary>
        [AcceptVerbs("GET", "POST")]
        public void SetSignData(string vipid)
        {
            try
            {
                var singdata = db.Ado.UseStoredProcedure().GetDataTable("p_life_SignIn", new { vip_id = vipid });
                if (singdata != null)
                {
                    _api.Result = singdata;
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Message = "签到成功！";
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "签到失败！";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }
    }

    public class singinfo
    {
        public int days_sum { get; set; }
        public int days_continue { get; set; }
        public int state { get; set; }
    }
    public class singlist
    {
        public int id { get; set; }
        public string sign_time { get; set; }
        public int state { get; set; }
    }

    public partial class vipinfo
    {

        /// <summary>
        /// 
        /// </summary>		
        public int id { get; set; }

        /// <summary>
        /// 会员卡号
        /// </summary>		
        public string vip_id { get; set; }

        /// <summary>
        /// 微信openid
        /// </summary>		
        public string openid { get; set; }

        /// <summary>
        /// 绑定时间
        /// </summary>		
        public DateTime create_time { get; set; }

        /// <summary>
        /// 微信unionid
        /// </summary>		
        public string unionid { get; set; }

        /// <summary>
        /// 
        /// </summary>		
        public string remark { get; set; }

        /// <summary>
        /// 
        /// </summary>		
        public int wid { get; set; }

        /// <summary>
        /// 
        /// </summary>		
        public int userid { get; set; }

        /// <summary>
        /// 
        /// </summary>		
        public string headimgurl { get; set; }

        public int sex { get; set; }

        public string phone { get; set; }
        public string vip_name { get; set; }
        public string birthday { get; set; }
        public int constellation { get; set; }
        public DateTime addtime { get; set; }
        public string ddm { get; set; }
        public int account_state { get; set; }
        public int ticketcount { get; set; }
        public decimal useAmount { get; set; }
    }
}
