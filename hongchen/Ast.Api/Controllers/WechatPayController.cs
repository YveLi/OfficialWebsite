using OTAMS.Bll;
using OTAMS.Common;
using OTAMS.IBll;
using OTAMS.Models;
using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;

namespace OTAMS.Api.Controllers
{
    public class WechatPayController : ApiController
    {
        private readonly ApiRoot _api = new ApiRoot();
        private SqlSugarClient db = SqlSugarManage.GetInstance();
        private Iwx_userweixinService wxservice = new wx_userweixinService();
        private Iwx_payment_wxpayService payservice = new wx_payment_wxpayService();
        /// <summary>
        /// 支付回调地址URl
        /// </summary>
        private string _callBackUrl = ConfigurationManager.AppSettings["PayCallBackUrl"];
        public String packageValue = "";


        /// <summary>
        /// 微信支付
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        public ApiRoot WechatPay(int wid, string orderno, string openid, string paycode, decimal payprice, string ordetype)
        {
            string key = "";
            string mchId = "";
            string timeStamp = "";
            string nonceStr = "";
            string paySign = "";
            string prepayId;
            string ipAddress = "127.0.0.1";
            try
            {
                var wxinfo = wxservice.GetSingle(o => o.id == wid);
                var appid = wxinfo.AppId;
                var wxpayconfig = payservice.GetSingle(o => o.wid == wid);
                if (wxpayconfig != null)
                {
                    key = wxpayconfig.paykey;
                    mchId = wxpayconfig.mch_id;
                }
                if (key.AIsNullOrEmptyString())
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "支付失败，秘钥为空";
                    WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
                }
                if (mchId.AIsNullOrEmptyString())
                {
                    _api.Tag = (int)ApiConfig.Status.Fail;
                    _api.Message = "支付失败，商户号为空";
                    WebUtils.ResponseHTML(JsonHelper.JsonpFormat(_api));
                }

                ipAddress = GetIP();
                timeStamp = TenPayV3Util.GetTimestamp();
                nonceStr = TenPayV3Util.GetNoncestr();

                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                //设置package订单参数
                packageReqHandler.SetParameter("appid", appid);       //公众账号ID
                packageReqHandler.SetParameter("mch_id", mchId);          //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
                packageReqHandler.SetParameter("body", "recharge");  //商品描述
                packageReqHandler.SetParameter("attach", wid + "|" + orderno + "|" + ordetype);//自定义信息
                packageReqHandler.SetParameter("out_trade_no", paycode);      //商家订单号
                packageReqHandler.SetParameter("total_fee", (ApiHelper.Aint(payprice * 100)).ToString());                    //商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.SetParameter("spbill_create_ip", ipAddress);   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", _callBackUrl);         //接收财付通通知的URL
                packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());//交易类型
                packageReqHandler.SetParameter("openid", openid);                       //用户的openId
                string sign = packageReqHandler.CreateMd5Sign("key", key);
                packageReqHandler.SetParameter("sign", sign);
                string data = packageReqHandler.ParseXML();
                var resultStr = TenPayV3.Unifiedorder(data);
                var res = XDocument.Parse(resultStr);
                prepayId = res.Element("xml").Element("prepay_id").Value;
                //设置支付参数
                Senparc.Weixin.MP.TenPayLibV3.RequestHandler paySignReqHandler = new Senparc.Weixin.MP.TenPayLibV3.RequestHandler(null);
                paySignReqHandler.SetParameter("appId", appid);
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                paySign = paySignReqHandler.CreateMd5Sign("key", key);
                var paydata = new
                {
                    appId = appid,
                    timeStamp = timeStamp,
                    nonceStr = nonceStr,
                    package = "prepay_id=" + prepayId,
                    signType = "MD5",
                    paySign = paySign
                };
                TxtLog.CreateErrorLogTxt("ss", "sds", "sds", JsonConvert.SerializeObject(paydata));
                _api.Result = paydata;
                _api.Tag = (int)ApiConfig.Status.Success;
                _api.Message = "获取支付配置成功！";
            }
            catch (Exception ex)
            {
                _api.Tag = (int)ApiConfig.Status.Wrong;
                _api.Message = ex.Message;
            }
            return _api;
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                return "0.0.0.0";
            }
            return result;
        }
    }
}
