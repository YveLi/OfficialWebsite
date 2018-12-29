using KeHenUI.Social.Common.Models;
using KeHenUI.Social.Common.Infrastructure;

namespace KeHenUI.Social.Common.Services
{
    public class PostCategoryService : BaseService<PostCategory, int>
    {
        /// <summary>
        /// 读取帖子分类列表
        /// </summary>
        /// <returns></returns>
        public GetListsResponse<PostCategory> GetPostCategories()
        {
            var response = new GetListsResponse<PostCategory>();
            var result = DbBase.Query<PostCategory>().ToList();
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
        public PostCategory GetPostType(int CategoriesId)
        {
            return DbBase.SingleOrDefaultById<PostCategory>(CategoriesId);
        }
    }
}
