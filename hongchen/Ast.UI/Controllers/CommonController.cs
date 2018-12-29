using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ast.Common;
using Ast.IBll;
using SqlSugar;
using Ast.Models;
using Ast.Models.ViewModels;
using Ast.UI.Models;
using Newtonsoft.Json;
using Ast.Bll;

namespace Ast.UI.Controllers
{
    public class CommonController : UserBaseController
    {
        private IOTAMsgListService msgservice = new OTAMsgListService();
        private IOTAMailListService mailservice = new OTAMailListService();

        #region 发送内部消息
        public bool SendMsg(string acceptusers, string title, string content, int type, int flowid, string msg)
        {
            bool success = true;
            try
            {
                foreach (string str in acceptusers.Split(','))
                {
                    int id = DataConversion.StrToInt(str);
                    var msgmd = new OTAMsgList
                    {
                        SendUserName = CurrentUserName,
                        SendUser = CurrentUserId,
                        AcceptUser = id,
                        SendStatus = 1,
                        IsRead = 0,
                        AddTime = DateTime.Now,
                        ModifyTime = DateTime.Now,
                        Status = 0,
                        Type = type,
                        Title = title,
                        MsgContent = content,
                        FlowMsg = msg,
                        FlowId = flowid
                    };
                    msgservice.Add(msgmd);
                }
            }
            catch (Exception ex)
            {
                success = false;
                LogHelper.WriteLog(ex.ToString());
            }
            return success;
        }
        public ActionResult GetUnReadMsgList()
        {
            var list = msgservice.GetList(o => o.AcceptUser.Equals(CurrentUserId) && o.IsRead == 0).OrderByDescending(o => o.AddTime);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MsgIndex()
        {
            return View();
        }
        public ActionResult MsgDetail()
        {
            return View();
        }
        public ActionResult MsgInfo(int Id = 0)
        {
            var model = msgservice.GetSingle(o => o.Id == Id) ?? new OTAMsgList();
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult MsgUpdate(OTAMsgList model)
        {
            var md = msgservice.GetById(model.Id.ToString());
            md.IsRead = 1;
            md.ModifyTime = DateTime.Now;
            msgservice.Update(md);
            return Content("return sth");
        }
        #endregion

        #region 发送内部邮件
        public bool SendMail(string acceptusers, string title, string content, int type, int isout)
        {
            bool success = true;
            try
            {
                foreach (string str in acceptusers.Split(','))
                {
                    int id = DataConversion.StrToInt(str);
                    var mail = new OTAMailList
                    {
                        SendUser = CurrentUserId,
                        AcceptUser = id,
                        SendStatus = 1,
                        IsRead = 0,
                        AddTime = DateTime.Now,
                        ModifyTime = DateTime.Now,
                        Status = 0,
                        IsOut = isout,
                        OuterName = str,
                        Type = type,
                        Title = title,
                        MsgContent = content,
                    };
                    mailservice.Add(mail);
                }
            }
            catch (Exception ex)
            {
                success = false;
                LogHelper.WriteLog(ex.ToString());
            }
            return success;
        }
        #endregion

        #region 获取自定义编码
        public JsonResult GetCustomNo()
        {
            string firstname = Request["name"] ?? "";
            string table = Request["table"] ?? "";

            var returl = CustomeTable(firstname, table);
            return Json(returl, JsonRequestBehavior.AllowGet);
        }

        public string CustomeTable(string firstname, string name)
        {
            int n = 1;
            string msg = string.Empty;
            string json = string.Empty;
            string fieldName = string.Empty;
            var date = DateTime.Now.ToString("yyyyMMdd") + "-";
            try
            {
                switch (name)
                {
                    case "Group":
                        fieldName = "GroupNo";
                        //json = JsonConvert.SerializeObject(groupservice.GetList(o => o.GroupNo.Contains(date)).OrderByDescending(o => o.Id).Select(o => new { CustomeNo = o.GroupNo }).First());
                        break;
                }
            }
            catch
            {
            }
            if (json != "")
            {
                Custome list = JsonConvert.DeserializeObject<Custome>(json);
                msg = list.CustomeNo;
                if (msg != "")
                {
                    int length = (firstname + date).Length;
                    if (msg.Length > length)
                    {
                        string strNumber = msg.Substring(length);
                        try
                        {
                            n = int.Parse(strNumber) + 1;
                        }
                        catch { }
                    }
                }
                msg = firstname + date + n.ToString().PadLeft(2, '0');
            }
            else
            {
                msg = firstname + date + n.ToString().PadLeft(2, '0');
            }
            return msg;
        }
        public class Custome
        {
            public string CustomeNo { get; set; }
        }
        #endregion

    }
}