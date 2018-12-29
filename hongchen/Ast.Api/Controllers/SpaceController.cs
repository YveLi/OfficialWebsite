using OTAMS.Common;
using OTAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OTAMS.IBll;
using OTAMS.Bll;
using SqlSugar;
using Newtonsoft.Json;
using OTAMS.Api.Common;

namespace OTAMS.Api.Controllers
{
    public class SpaceController : ApiController
    {
        private readonly ApiRoot _api = new ApiRoot();
        private SqlSugarClient db = SqlSugarManage.GetInstance();
        private Ilife_shop_spaceService spaceservice = new life_shop_spaceService();
        private Ilife_marketinginfo_spaceService orderservice = new life_marketinginfo_spaceService();
        private Idt_usersService userservice = new dt_usersService();
        private Idt_user_fundService fundservice = new dt_user_fundService();
        private Idt_fund_recordService recordservice = new dt_fund_recordService();


        #region 获取空间列表

        [AcceptVerbs("GET", "POST")]
        public void GetSpaceList(string ddm)
        {
            try
            {
                var list = spaceservice.GetList(o => true, "space_id");
                IList<spaceinfo> spacelist = new List<spaceinfo>();
                foreach (var item in list)
                {
                    var spacemd = new spaceinfo();
                    spacemd.Id = item.id;
                    spacemd.spacename = item.space_name;
                    spacemd.spaceno = item.space_id;
                    spacemd.type = item.type;
                    spacemd.unit = item.unit;
                    spacemd.baseprice = item.base_money;
                    spacemd.unitprice = item.unit_money;
                    spacemd.capprice = item.cap_money;
                    spacemd.remark = item.remark;
                    spacemd.Imgurl = item.ImgUrl;
                    var orderlist = orderservice.GetList(o => o.ddm == ddm && o.space_id == item.space_id && o.order_state < 10, "space_id");
                    if (orderlist.Count > 0 && orderlist != null)
                    {
                        spacemd.spacestatus = 1;
                    }
                    else
                    {
                        spacemd.spacestatus = 0;
                    }
                    spacelist.Add(spacemd);
                }

                if (spacelist != null)
                {
                    _api.Result = spacelist;
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Message = "获取成功";
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "获取失败";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Fail;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        [AcceptVerbs("GET", "POST")]
        public void SpaceStartTiming(string vipid, string spaceno, string spacename, decimal oneprice, string unit, string ddm)
        {
            try
            {
                string orderno = vipid + "-S" + DateTime.Now.Ticks.ToString();
                var success = orderservice.Add(new life_marketinginfo_space()
                {
                    vip_id = vipid,
                    space_id = spaceno,
                    space_name = spacename,
                    begin_time = DateTime.Now,
                    unit = unit,
                    unit_money = oneprice,
                    nullah_num = orderno,
                    order_state = 0,
                    ddm = ddm,
                    end_time = DateTime.Now,
                    pay_time = DateTime.Now,
                });
                var list = orderservice.GetSingle(o => o.nullah_num == orderno && o.ddm == ddm);
                if (list != null)
                {
                    var spaceorder = new spaceorder();
                    spaceorder.id = list.id;
                    spaceorder.vipid = list.vip_id;
                    spaceorder.spaceno = list.space_id;
                    spaceorder.spacename = list.space_name;
                    spaceorder.orderno = list.nullah_num;
                    spaceorder.begintime = list.begin_time;
                    spaceorder.endtime = list.end_time;
                    spaceorder.duration = list.duration;
                    spaceorder.orderstate = list.order_state;
                    spaceorder.paytype = list.pay_way;
                    spaceorder.paytime = list.pay_time;
                    spaceorder.unit = list.unit;
                    spaceorder.payprice = list.pay_money;
                    spaceorder.unitprice = list.unit_money;
                    spaceorder.totalprice = list.total_money;
                    spaceorder.offprice = list.save_money;
                    spaceorder.ddm = list.ddm;
                    _api.Result = spaceorder;
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Message = "空间计时成功！";
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "空间计时失败！";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Fail;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        [AcceptVerbs("GET", "POST")]
        public void PaySpaceOrder(string orderno, int wid, int userid, string openid, string vipid, string ddm, int paytype, decimal payprice, decimal hours)
        {
            try
            {
                var code = "S" + ApiHelper.GetOrderNumber();
                var user = userservice.GetSingle(o => o.wid == wid && o.id == userid);
                if (paytype == 2)
                {
                    var fund = fundservice.GetSingle(o => o.wId == wid && o.userId == userid);
                    if (fund == null || fund.Id == 0)
                    {
                        _api.Tag = (int)ApiConfig.Status.Fail;
                        _api.Message = "获取用户钱包失败！";
                        WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
                    }
                    if (fund.useAmount >= payprice)
                    {
                        var fundRecord = new dt_fund_record();
                        fundRecord.wId = wid;
                        fundRecord.userId = userid;
                        fundRecord.userName = user.user_name;
                        fundRecord.changeAmount = payprice;
                        fundRecord.beforeAmount = fund.useAmount;
                        fundRecord.afterAmount = fund.useAmount - payprice;
                        fundRecord.type = ApiHelper.TypeRecord.Consume;
                        fundRecord.state = ApiHelper.StateRecharge.Committed;
                        fundRecord.addTime = DateTime.Now;
                        fundRecord.updateTime = DateTime.Now;
                        fundRecord.recordCode = code;
                        fundRecord.memo = "空间星币消费";
                        var success = recordservice.Add(fundRecord);
                        if (success)
                        {
                            var spacedt = db.Ado.UseStoredProcedure().GetDataTable("p_recharge", new { rCode = code, wId = wid, amount = payprice, state = 1 });
                            var spaceinfo = db.Ado.UseStoredProcedure().GetDataTable("p_life_PaySpace", new { nullah_num = orderno, pay_money = payprice, pay_way = paytype, vip_id = vipid, ddm = ddm, hours = hours });
                            var shoporder = orderservice.GetSingle(o => o.order_state == 0 && o.nullah_num == orderno);
                            if (shoporder != null)
                            {
                                shoporder.pay_time = DateTime.Now;
                                shoporder.order_state = 10;
                                shoporder.pay_way = 2;
                                var s = orderservice.Update(shoporder);
                                if (s)
                                {
                                    _api.Tag = (int)ApiConfig.Status.Success;
                                    _api.Message = "支付成功";
                                }
                                else
                                {
                                    _api.Tag = (int)ApiConfig.Status.Fail;
                                    _api.Message = "支付失败";
                                }
                            }
                        }
                    }
                    else
                    {
                        _api.Tag = (int)ApiConfig.Status.Fail;
                        _api.Message = "钱包星币不足，请前往充值！";
                    }
                }
                else
                {
                    var spaceinfo = db.Ado.UseStoredProcedure().GetDataTable("p_life_PaySpace", new { nullah_num = orderno, pay_money = payprice, pay_way = paytype, vip_id = vipid, ddm = ddm, hours = hours });
                    var paycode = "SP" + ApiHelper.GetOrderNumber();
                    WechatPayController wechatpay = new WechatPayController();
                    var result = wechatpay.WechatPay(wid, orderno, openid, paycode, payprice, "PaySpaceOrder");
                    if (result.Tag == 1)
                    {
                        _api.Result = result.Result;
                        _api.Tag = (int)ApiConfig.Status.Success;
                        _api.Message = "获取支付配置成功";
                    }
                    else
                    {
                        _api.Result = result.Result;
                        _api.Tag = (int)ApiConfig.Status.Fail;
                        _api.Message = "获取失败";
                    }
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        [AcceptVerbs("GET", "POST")]
        public void SubmitSpaceOrder(string orderno, int wid, int userid, string openid, string vipid, string ddm, int paytype, decimal payprice, decimal hours)
        {
            try
            {
                _api.Tag = (int)ApiConfig.Status.Success;
                _api.Message = "提交成功";
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }
        [AcceptVerbs("GET", "POST")]
        public void GetSpaceOrderList(string vipid, string ddm, int index, int size)
        {
            try
            {
                PageModel page = new PageModel()
                {
                    PageIndex = index,
                    PageSize = size
                };
                var orderlist = orderservice.GetPageList(page, o => o.vip_id == vipid && o.ddm == ddm);
                if (orderlist != null)
                {
                    IList<spaceorder> spaceorderlist = new List<spaceorder>();
                    foreach (var list in orderlist.datalist)
                    {
                        var spaceorder = new spaceorder();
                        spaceorder.id = list.id;
                        spaceorder.vipid = list.vip_id;
                        spaceorder.spaceno = list.space_id;
                        spaceorder.spacename = list.space_name;
                        spaceorder.orderno = list.nullah_num;
                        spaceorder.begintime = list.begin_time;
                        spaceorder.endtime = list.end_time;
                        spaceorder.duration = list.duration;
                        spaceorder.orderstate = list.order_state;
                        spaceorder.paytype = list.pay_way;
                        spaceorder.paytime = list.pay_time;
                        spaceorder.unit = list.unit;
                        spaceorder.payprice = list.pay_money;
                        spaceorder.unitprice = list.unit_money;
                        spaceorder.totalprice = list.total_money;
                        spaceorder.offprice = list.save_money;
                        spaceorder.ddm = list.ddm;
                        spaceorderlist.Add(spaceorder);
                    }
                    _api.Result = spaceorderlist;
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Message = "获取成功";
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "获取失败";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Fail;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        [AcceptVerbs("GET", "POST")]
        public void GetSpaceDetail(string spaceid)
        {
            try
            {
                var spaceinfo = spaceservice.GetSingle(o => o.space_id == spaceid);
                if (spaceinfo != null)
                {
                    var spacemd = new spaceinfo();
                    spacemd.Id = spaceinfo.id;
                    spacemd.spacename = spaceinfo.space_name;
                    spacemd.spaceno = spaceinfo.space_id;
                    spacemd.type = spaceinfo.type;
                    spacemd.unit = spaceinfo.unit;
                    spacemd.baseprice = spaceinfo.base_money;
                    spacemd.unitprice = spaceinfo.unit_money;
                    spacemd.capprice = spaceinfo.cap_money;
                    spacemd.remark = spaceinfo.remark;
                    spacemd.Imgurl = spaceinfo.ImgUrl;
                    _api.Result = spacemd;
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Message = "获取成功";
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "获取失败";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Fail;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }

        [AcceptVerbs("GET", "POST")]
        public void GetSpaceOrder(string orderno)
        {
            try
            {
                var list = orderservice.GetSingle(o => o.nullah_num == orderno);
                if (list != null)
                {
                    var spaceorder = new spaceorder();
                    spaceorder.id = list.id;
                    spaceorder.vipid = list.vip_id;
                    spaceorder.spaceno = list.space_id;
                    spaceorder.spacename = list.space_name;
                    spaceorder.orderno = list.nullah_num;
                    spaceorder.begintime = list.begin_time;
                    spaceorder.endtime = list.end_time;
                    spaceorder.duration = list.duration;
                    spaceorder.orderstate = list.order_state;
                    spaceorder.paytype = list.pay_way;
                    spaceorder.paytime = list.pay_time;
                    spaceorder.unit = list.unit;
                    spaceorder.payprice = list.pay_money;
                    spaceorder.unitprice = list.unit_money;
                    spaceorder.totalprice = list.total_money;
                    spaceorder.offprice = list.save_money;
                    spaceorder.ddm = list.ddm;
                    _api.Result = spaceorder;
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Message = "获取成功";
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "获取失败";
                }
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Fail;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }



        #endregion
    }

    public class spaceinfo
    {
        public int Id { get; set; }

        public string spacename { get; set; }

        public string spaceno { get; set; }

        public int type { get; set; }

        public string unit { get; set; }

        public decimal baseprice { get; set; }

        public decimal unitprice { get; set; }

        public decimal capprice { get; set; }

        public string remark { get; set; }

        public string Imgurl { get; set; }

        public int spacestatus { get; set; }
    }

    public partial class spaceorder
    {
        public int id { get; set; }
        public string vipid { get; set; }
        public string spacename { get; set; }
        public string spaceno { get; set; }
        public DateTime begintime { get; set; }
        public DateTime endtime { get; set; }
        public decimal duration { get; set; }
        public string orderno { get; set; }
        public int orderstate { get; set; }
        public string unit { get; set; }
        public int paytype { get; set; }
        public DateTime paytime { get; set; }
        public decimal payprice { get; set; }
        public decimal unitprice { get; set; }
        public decimal totalprice { get; set; }
        public decimal offprice { get; set; }
        public string ddm { get; set; }

    }
}
