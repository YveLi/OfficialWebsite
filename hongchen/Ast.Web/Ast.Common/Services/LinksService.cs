using KeHenUI.Social.Common.Infrastructure;
using KeHenUI.Social.Common.Models;
namespace KeHenUI.Social.Common.Services
{
    public class LinksService : BaseService<Links, int>
    {
        /// <summary>
        /// 读取友情链接列表
        /// </summary>
        /// <returns></returns>
        public GetListsResponse<Links> GetLinks()
        {
            var response = new GetListsResponse<Links>();
            var result = DbBase.Query<Links>().OrderByDescending(p=>p.Sort).ToList();
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
