using KeHenUI.Social.Common.Models;
using System;
using KeHenUI.Social.Common.Extension;
using KeHenUI.Social.Common.Infrastructure;
using KeHenUI.Social.Common.Services.Messaging;

namespace KeHenUI.Social.Common.Services
{
    public class ArticleCategoryService : BaseService<ArticleCategory, int>
    {
        public GetListsResponse<ArticleCategory> GetArticleCategories(int CategoriesId)
        {
            var response = new GetListsResponse<ArticleCategory>();
            var result = DbBase.Query<ArticleCategory>().Where(a => a.ParentId == CategoriesId).ToList();
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
        public ArticleCategory GetPostType(int CategoriesId)
        {
            return DbBase.SingleOrDefaultById<ArticleCategory>(CategoriesId);
        }

    }
}
