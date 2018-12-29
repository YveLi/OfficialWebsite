using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeHenUI.Social.Common.Infrastructure;
using KeHenUI.Social.Common.Models;

namespace KeHenUI.Social.Common.Services
{
    public class ActivityBannerListService : BaseService<ActivityBannerList, int>
    {
        public GetListsResponse<ActivityBannerList> GetBannerList(int type = 0)
        {
            var response = new GetListsResponse<ActivityBannerList>();
            var result = DbBase.Query<ActivityBannerList>().OrderByDescending(b => b.Sort).ToList();
            if (result != null && result.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "获取成功";
                response.Items = result;
                return response;
            }
            response.Message = "暂无数据";
            return response;
        }
    }
}
