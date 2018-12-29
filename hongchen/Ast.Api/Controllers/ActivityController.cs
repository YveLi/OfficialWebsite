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

namespace OTAMS.Api.Controllers
{
    public class ActivityController : ApiController
    {
        private readonly ApiRoot _api = new ApiRoot();
        private ILifeActivityListService activityservice = new LifeActivityListService();

        #region 获取活动列表

        [AcceptVerbs("GET", "POST")]
        public void GetActivityList()
        {
            try
            {
                var list = activityservice.GetList(o => true, "id");
                if (list != null)
                {
                    _api.Result = list;
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
}
