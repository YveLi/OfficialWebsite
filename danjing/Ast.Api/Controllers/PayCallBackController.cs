using OTAMS.Bll;
using OTAMS.Common;
using OTAMS.IBll;
using OTAMS.Models;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace OTAMS.Api.Controllers
{
    public class PayCallBackController : Controller
    {
        private readonly ApiRoot _api = new ApiRoot();
        private SqlSugarClient db = SqlSugarManage.GetInstance();
        private Iwx_payment_wxpayService payservice = new wx_payment_wxpayService();
        private Ilife_marketinginfo_shopsummaryService shopservice = new life_marketinginfo_shopsummaryService();
        private Ilife_marketinginfo_spaceService spaceservice = new life_marketinginfo_spaceService();
        private Ilife_marketinginfo_summaryService orderservice = new life_marketinginfo_summaryService();
        public ActionResult CallBack()
        {
            return View();
        }

        public string CallBackUrl()
        {
            string result = "";
            int wId = 0;
            string appId = "";
            string mchId = "";
            string sign = "";
            decimal amount = 0;
            string key = "";
            var ordertype = "";
            var orderno = "";
            OrderParam orderParam = new OrderParam();
            try
            {
                byte[] byts = new byte[Request.InputStream.Length];
                Request.InputStream.Read(byts, 0, byts.Length);
                string req = System.Text.Encoding.Default.GetString(byts);
                req = Server.UrlDecode(req);
                result = req;
                string returnCode = XmlHelper.ReadXmlValue(req, "xml/return_code");
                appId = XmlHelper.ReadXmlValue(req, "xml/appid");
                string attach = XmlHelper.ReadXmlValue(req, "xml/attach");
                string[] attachArr = attach.Split('|');
                wId = attachArr[0].Aint();
                orderno = attachArr[1].ObjToString();
                ordertype = attachArr[2].ObjToString();
                TxtLog.CreateErrorLogTxt("RechargeCallBack", "RechargeCallBack", "RechargeCallBack", "RechargeCallBack：" + result);
                var payinfo = payservice.GetSingle(o => o.wid == wId);
                if (payinfo != null)
                {
                    key = payinfo.paykey;
                    mchId = payinfo.mch_id;

                    string rCode = XmlHelper.ReadXmlValue(req, "xml/out_trade_no");
                    amount = XmlHelper.ReadXmlValue(req, "xml/total_fee").Adecimal();
                    sign = XmlHelper.ReadXmlValue(req, "xml/sign");
                    string bankType = XmlHelper.ReadXmlValue(req, "xml/bank_type");
                    string cashFee = XmlHelper.ReadXmlValue(req, "xml/cash_fee");

                    string feeType = XmlHelper.ReadXmlValue(req, "xml/fee_type");
                    string isSubscribe = XmlHelper.ReadXmlValue(req, "xml/is_subscribe");
                    string nonceStr = XmlHelper.ReadXmlValue(req, "xml/nonce_str");
                    string openid = XmlHelper.ReadXmlValue(req, "xml/openid");
                    string tradeType = XmlHelper.ReadXmlValue(req, "xml/trade_type");

                    string transactionId = XmlHelper.ReadXmlValue(req, "xml/transaction_id");
                    string timeEnd = XmlHelper.ReadXmlValue(req, "xml/time_end");

                    string resultCode = XmlHelper.ReadXmlValue(req, "xml/result_code");
                    #region 签名验证

                    //创建支付应答对象
                    Senparc.Weixin.MP.TenPayLibV3.RequestHandler packageReqHandler =
                        new Senparc.Weixin.MP.TenPayLibV3.RequestHandler(null);
                    packageReqHandler.SetParameter("appid", appId);
                    packageReqHandler.SetParameter("mch_id", mchId);
                    packageReqHandler.SetParameter("attach", attach);
                    packageReqHandler.SetParameter("bank_type", bankType);
                    packageReqHandler.SetParameter("cash_fee", cashFee);

                    packageReqHandler.SetParameter("fee_type", feeType);
                    packageReqHandler.SetParameter("is_subscribe", isSubscribe);
                    packageReqHandler.SetParameter("nonce_str", nonceStr);
                    packageReqHandler.SetParameter("openid", openid);
                    packageReqHandler.SetParameter("out_trade_no", rCode);

                    packageReqHandler.SetParameter("total_fee", amount.Astring());
                    packageReqHandler.SetParameter("trade_type", tradeType);
                    packageReqHandler.SetParameter("transaction_id", transactionId);
                    packageReqHandler.SetParameter("time_end", timeEnd);
                    packageReqHandler.SetParameter("result_code", resultCode);

                    packageReqHandler.SetParameter("return_code", returnCode);
                    string signStr = packageReqHandler.CreateMd5Sign("key", key);
                    //验证签名
                    if (signStr == sign)
                    {
                        orderParam.WId = wId;
                        orderParam.RCode = rCode;
                        orderParam.Amount = (amount / 100).Adecimal();
                        //成功
                        if (returnCode.Trim().ToUpper() == "SUCCESS")
                        {
                            orderParam.State = 1;
                            switch (ordertype)
                            {
                                case "WalletRecharge":
                                    UpdateOrderInfo(orderParam);
                                    break;
                                case "PaySpaceOrder":
                                    var space = spaceservice.GetSingle(o => o.nullah_num == orderno);
                                    if (space != null)
                                    {
                                        space.pay_time = DateTime.Now;
                                        space.order_state = 10;
                                        space.pay_way = 1;
                                        space.SpaceType = 1;
                                        spaceservice.Update(space);
                                        var success = orderservice.Add(new life_marketinginfo_summary()
                                        {
                                            vip_id = space.vip_id,
                                            program = 1,
                                            nullah_num = orderno,
                                            ddm = space.ddm,
                                            total_money = space.total_money,
                                            save_money = space.save_money,
                                            pay_money = space.pay_money,
                                            pay_time = space.pay_time,
                                            pay_way = space.pay_way,
                                            share_state = 0,
                                            detail = space.space_name + "消费成功"
                                        });
                                        if (success)
                                        {
                                            _api.Tag = (int)ApiConfig.Status.Success;
                                            _api.Message = "支付成功";
                                        }
                                    }
                                    break;
                                case "PayShopOrder":
                                    var shop = shopservice.GetSingle(o => o.nullah_num == orderno && o.order_state == 9);
                                    if (shop != null)
                                    {
                                        shop.pay_time = DateTime.Now;
                                        shop.order_state = 10;
                                        shop.pay_way = 1;
                                        shopservice.Update(shop);
                                        var shopsuccess = orderservice.Add(new life_marketinginfo_summary()
                                        {
                                            vip_id = shop.vip_id,
                                            program = 2,
                                            nullah_num = orderno,
                                            ddm = shop.ddm,
                                            total_money = shop.total_money,
                                            save_money = shop.save_money,
                                            pay_money = shop.pay_money,
                                            pay_time = shop.pay_time,
                                            pay_way = shop.pay_way,
                                            share_state = 0,
                                            detail = "商店消费成功"
                                        });
                                        if (shopsuccess)
                                        {
                                            _api.Tag = (int)ApiConfig.Status.Success;
                                            _api.Message = "支付成功";
                                        }
                                    }
                                    else
                                    {
                                        _api.Tag = (int)ApiConfig.Status.Fail;
                                        _api.Message = "支付失败";
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            orderParam.State = 2;
                            switch (ordertype)
                            {
                                case "WalletRecharge":
                                    UpdateOrderInfo(orderParam);
                                    break;
                                case "PaySpaceOrder":
                                    _api.Tag = (int)ApiConfig.Status.Fail;
                                    _api.Message = "支付失败";
                                    break;
                                case "PayShopOrder":
                                    _api.Tag = (int)ApiConfig.Status.Fail;
                                    _api.Message = "支付失败";
                                    break;
                            }
                        }
                        WriteContext("success");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                result = "";
                //记录日志
                TxtLog.CreateErrorLogTxt("RechargeCallBack", "RechargeCallBack", "RechargeCallBack", "RechargeCallBack失败：" + ex.Message);
            }
            return HttpUtility.HtmlEncode(result); //result;         
        }


        private void WriteContext(string returnCode, string returnMsg = "")
        {
            Response.Write(returnCode);
            Response.End();
        }

        public int UpdateOrderInfo(OrderParam param)
        {
            int result = 0;
            try
            {
                var spacedt = db.Ado.UseStoredProcedure().GetDataTable("p_recharge", new { rCode = param.RCode, wId = param.WId, amount = param.Amount, state = param.State });
                if (spacedt != null)
                {
                    _api.Result = spacedt;
                    _api.Tag = (int)ApiConfig.Status.Success;
                    _api.Message = "充值成功";
                }
                else
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "充值失败";
                }
            }
            catch (Exception ex)
            {
                //记录日志
                TxtLog.CreateErrorLogTxt("UpdateOrderInfo", "UpdateOrderInfo", "UpdateOrderInfo失败：", ex.Message);
            }
            return result;
        }
    }
    [Serializable]
    public partial class OrderParam
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string RCode { get; set; }
        /// <summary>
        /// 平台ID
        /// </summary>
        public int WId { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
    }
}
