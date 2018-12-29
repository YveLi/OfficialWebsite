using OTAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OTAMS.Common;
using OTAMS.IBll;
using OTAMS.Bll;
using SqlSugar;
using Newtonsoft.Json;
using OTAMS.Api.Common;
using System.Configuration;
using System.Text;
using System.IO;

namespace OTAMS.Api.Controllers
{
    public class ShopController : ApiController
    {
        private readonly ApiRoot _api = new ApiRoot();
        private SqlSugarClient db = SqlSugarManage.GetInstance();
        private Ilife_shop_typeService typeservice = new life_shop_typeService();
        private Ilife_shopinfoService proservice = new life_shopinfoService();
        private Ilife_shop_sizeService sizeservice = new life_shop_sizeService();
        private Ilife_marketinginfo_shopsummaryService orderservice = new life_marketinginfo_shopsummaryService();
        private Ilife_marketinginfo_shopService orderproservice = new life_marketinginfo_shopService();
        private ILife_OrderTick_ListService ordertickservice = new Life_OrderTick_ListService();
        private Idt_usersService userservice = new dt_usersService();
        private Idt_user_fundService fundservice = new dt_user_fundService();
        private Idt_fund_recordService recordservice = new dt_fund_recordService();

        #region 获取所有商品
        /// <summary>
        /// 获取所有商品
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]

        public void GetProductList()
        {
            try
            {
                var list = typeservice.GetList(o => true, "id");
                IList<prodata> typelist = new List<prodata>();
                foreach (var item in list)
                {
                    var prolist = proservice.GetList(o => o.type_id == item.id, "type_id");
                    IList<prolist> prodata = new List<prolist>();
                    foreach (var pro in prolist)
                    {
                        var sizelist = sizeservice.GetList(o => o.ProId == pro.id, "type_id");
                        IList<prosizelist> prosizelist = new List<prosizelist>();
                        foreach (var size in sizelist)
                        {
                            var sizes = new prosizelist()
                            {
                                id = size.id,
                                saleprice = size.price_direct,
                                baseprice = size.price_join,
                                sizename = size.size,
                                costprice = size.price_cost_direct,
                            };
                            prosizelist.Add(sizes);
                        }
                        var proinfo = new prolist()
                        {
                            id = pro.id,
                            imgurl = pro.imgurl,
                            memo = pro.introduction,
                            proid = pro.product_id,
                            proname = pro.product_name,
                            protemper = ApiHelper.gettemper(pro.bz1),
                            prosweet = ApiHelper.getsize(pro.bz2),
                            prosizelist = prosizelist,
                        };
                        prodata.Add(proinfo);
                    }
                    var type = new prodata()
                    {
                        id = item.id,
                        typename = item.type_name,
                        state = item.state,
                        prolist = prodata,
                    };
                    typelist.Add(type);
                }
                if (typelist != null)
                {
                    _api.Result = typelist;
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

        #region 提交商店订单

        [AcceptVerbs("GET", "POST")]
        public void SubmitShopOrder(string vipid, string ddm, int spaceid, string spaceno, string spacename, decimal totalprice, decimal payprice, decimal offerprice, int offertype, string memo, int paytype, string productlist, string couponlist)
        {
            try
            {
                var orderno = vipid + "-M" + DateTime.Now.Ticks.ToString();
                //预支付订单
                orderservice.Add(new life_marketinginfo_shopsummary()
                {
                    nullah_num = orderno,
                    vip_id = vipid,
                    ddm = ddm,
                    spaceid = spaceid,
                    space_id = spaceno,
                    total_money = totalprice,
                    pay_money = payprice,
                    save_money = offerprice,
                    remark = memo,
                    space_name = spacename,
                    pay_way = paytype,
                    order_state = 9,
                    discount_type = offertype,
                    distribution_state = 0,
                    create_time = DateTime.Now,
                    pay_time = DateTime.Now,
                    distribution_time = DateTime.Now
                });
                //订单商品
                IList<orderproinfo> orderprolist = JsonConvert.DeserializeObject<IList<orderproinfo>>(productlist);
                foreach (var item in orderprolist)
                {
                    orderproservice.Add(new life_marketinginfo_shop()
                    {
                        spaceid = spaceid,
                        space_id = spaceno,
                        space_name = spacename,
                        ddm = ddm,
                        product_id = item.prono,
                        proid = item.proid,
                        product_name = item.proname,
                        unit_money = item.oneprice,
                        num = item.count,
                        pay_money = item.payprice,
                        save_money = item.offerprice,
                        nullah_num = orderno,
                        size = item.prosize,
                        temperature = item.protemper,
                        sugar = item.prosweet,
                        create_time = DateTime.Now,
                        pay_time = DateTime.Now,
                    });
                }
                //优惠卷
                //ordercouponinfo ordercouponlist = JsonConvert.DeserializeObject<ordercouponinfo>(couponlist);
                //if (ordercouponlist != null)
                //{
                //    ordertickservice.Add(new Life_OrderTick_List()
                //    {
                //        TickId = ordercouponlist.tickid,
                //        TickName = ordercouponlist.tickname,
                //        OrderNo = orderno,
                //        Price = ordercouponlist.faceprice
                //    });
                //}
                _api.Result = orderno;
                _api.Tag = (int)ApiConfig.Status.Success;
                _api.Message = "下单成功！";
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Fail;
                _api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
        }
        #endregion

        #region  获取订单列表
        [AcceptVerbs("GET", "POST")]
        public void GetShopOrderList(string vipid, string ddm, int index, int size)
        {
            try
            {
                PageModel page = new PageModel()
                {
                    PageIndex = index,
                    PageSize = size
                };
                var orderlist = orderservice.GetPageList(page, o => o.vip_id == vipid && o.ddm == ddm);
                IList<orderlist> orderlistinfo = new List<orderlist>();
                foreach (var item in orderlist.datalist)
                {
                    var prolist = orderproservice.GetList(o => o.nullah_num == item.nullah_num, "id");
                    IList<orderproinfo> orderproinfo = new List<orderproinfo>();
                    foreach (var pro in prolist)
                    {
                        orderproinfo.Add(new orderproinfo()
                        {
                            proid = pro.id,
                            prono = pro.product_id,
                            proname = pro.product_name,
                            oneprice = pro.unit_money,
                            count = pro.num,
                            totalprice = pro.total_money,
                            payprice = pro.pay_money,
                            offerprice = pro.save_money,
                            prosize = pro.size,
                            protemper = pro.temperature,
                            prosweet = pro.sugar
                        });
                    }
                    //var ticklist = ordertickservice.GetSingle(o => o.OrderNo == item.nullah_num);
                    //var tickinfo = new ordercouponinfo();
                    //if (ticklist != null)
                    //{
                    //    tickinfo.tickid = ticklist.Id;
                    //    tickinfo.tickname = ticklist.TickName;
                    //    tickinfo.faceprice = ticklist.Price;
                    //}
                    var order = new orderlist()
                    {
                        orderid = item.id,
                        orderno = item.nullah_num,
                        spacename = item.space_name,
                        spaceno = item.space_id,
                        paytime = item.pay_time,
                        memo = item.remark,
                        paytype = item.pay_way,
                        prolist = orderproinfo,
                        couponinfo = new ordercouponinfo(),
                        spaceid = item.spaceid,
                        totalprice = item.total_money,
                        payprice = item.pay_money,
                        offerprice = item.save_money,
                        orderstate = item.order_state,
                        addtime = item.create_time,
                    };
                    orderlistinfo.Add(order);
                }
                if (orderlistinfo != null)
                {
                    _api.Result = orderlistinfo;
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

        #region 商店付款
        [AcceptVerbs("GET", "POST")]
        public void PayShopOrder(string orderno, int wid, int userid, string openid, string vipid, string ddm, int paytype, decimal payprice)
        {
            ApiRoot api = new ApiRoot();
            try
            {
                var user = userservice.GetSingle(o => o.wid == wid && o.id == userid);
                if (paytype == 2)
                {
                    var code = "C" + ApiHelper.GetOrderNumber();
                    var fund = fundservice.GetSingle(o => o.wId == wid && o.userId == userid);
                    if (fund == null || fund.Id == 0)
                    {
                        api.Tag = (int)ApiConfig.Status.Fail;
                        api.Message = "获取用户钱包失败！";
                        WebUtils.ResponseHTML(JsonHelper.JsonpFormat(api));
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
                        fundRecord.type = 2;
                        fundRecord.state = 1;
                        fundRecord.addTime = DateTime.Now;
                        fundRecord.updateTime = DateTime.Now;
                        fundRecord.recordCode = code;
                        fundRecord.memo = "商城星币消费";
                        var success = recordservice.Add(fundRecord);
                        if (success)
                        {
                            var spacedt = db.Ado.UseStoredProcedure().GetDataTable("p_recharge", new { rCode = code, wId = wid, amount = payprice, state = 1 });
                            var shoporder = orderservice.GetSingle(o => o.order_state == 9 && o.nullah_num == orderno);
                            if (shoporder != null)
                            {
                                shoporder.pay_time = DateTime.Now;
                                shoporder.order_state = 10;
                                shoporder.pay_way = 2;
                                var s = orderservice.Update(shoporder);
                                if (s)
                                {
                                    api.Tag = (int)ApiConfig.Status.Success;
                                    api.Message = "支付成功";
                                }
                                else
                                {
                                    api.Tag = (int)ApiConfig.Status.Fail;
                                    api.Message = "支付失败";
                                }
                            }
                            else
                            {
                                api.Tag = (int)ApiConfig.Status.Fail;
                                api.Message = "支付失败";
                            }
                        }
                        else
                        {
                            api.Tag = (int)ApiConfig.Status.Fail;
                            api.Message = "支付失败";
                        }
                    }
                    else
                    {
                        api.Tag = (int)ApiConfig.Status.Fail;
                        api.Message = "钱包星币不足，请前往充值！";
                    }
                }
                else
                {
                    var paycode = "Sh" + ApiHelper.GetOrderNumber();
                    WechatPayController wechatpay = new WechatPayController();
                    var result = wechatpay.WechatPay(wid, orderno, openid, paycode, payprice, "PayShopOrder");
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
            catch (Exception ex)
            {
                api.Tag = (int)ApiConfig.Status.Wrong;
                api.Message = ex.Message;
            }
            WebUtils.ResponseHTML(JsonHelper.JsonpFormat(api));
        }
        #endregion
    }

    #region model
    public class JsonResult<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
    public class RechargeRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public int WId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// opendId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderCode { get; set; }
    }
    public class WxPayResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        /// 支付签名时间戳
        /// </summary>
        public string timeStamp { get; set; }

        /// <summary>
        /// 支付签名随机串，不长于 32 位
        /// </summary>
        public string nonceStr { get; set; }

        /// <summary>
        /// 统一支付接口返回的prepay_id参数值
        /// </summary>
        public string package { get; set; }
        /// <summary>
        /// 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
        /// </summary>
        public string signType { get; set; }
        /// <summary>
        /// 支付签名
        /// </summary>
        public string paySign { get; set; }
    }
    public class orderlist
    {
        public int orderid { get; set; }
        public string orderno { get; set; }
        public string spacename { get; set; }
        public string spaceno { get; set; }
        public DateTime paytime { get; set; }
        public string memo { get; set; }
        public int paytype { get; set; }
        public int spaceid { get; set; }
        public decimal totalprice { get; set; }
        public decimal payprice { get; set; }
        public decimal offerprice { get; set; }
        public int orderstate { get; set; }
        public DateTime addtime { get; set; }
        public IList<orderproinfo> prolist { get; set; }
        public ordercouponinfo couponinfo { get; set; }

    }
    public class ordercouponinfo
    {
        public int tickid { get; set; }
        public decimal faceprice { get; set; }
        public string tickname { get; set; }
    }

    public class orderproinfo
    {
        public int proid { get; set; }
        public string prono { get; set; }
        public string proname { get; set; }
        public decimal oneprice { get; set; }
        public int count { get; set; }
        public decimal totalprice { get; set; }
        public decimal payprice { get; set; }
        public decimal offerprice { get; set; }
        public string prosize { get; set; }
        public string protemper { get; set; }
        public string prosweet { get; set; }

    }


    public class prodata
    {
        public int id { get; set; }
        public string typename { get; set; }
        public int state { get; set; }
        public IList<prolist> prolist { get; set; }
    }
    public class prolist
    {
        public int id { get; set; }
        public int state { get; set; }
        public int sort { get; set; }
        public string proid { get; set; }
        public string proname { get; set; }
        public string imgurl { get; set; }
        public string memo { get; set; }
        public string protemper { get; set; }
        public string prosweet { get; set; }
        public IList<prosizelist> prosizelist { get; set; }
    }
    public class prosizelist
    {
        public int id { get; set; }
        public string sizename { get; set; }
        public decimal saleprice { get; set; }
        public decimal baseprice { get; set; }
        public decimal costprice { get; set; }

    }

    #endregion

}
