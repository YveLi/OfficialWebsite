using KeHenUI.Social.Common.Infrastructure;
using KeHenUI.Social.Common.Models;

namespace KeHenUI.Social.Common.Services
{
    /// <summary>
    /// zan
    /// </summary>
    public class AsterLikesService : BaseService<AsterLikes, int>
    {
        /// <summary>
        /// 是否关注
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsExistsGuanZhu(int likeuserid, int userid)
        {
            return DbBase.Query<AsterLikes>().Any(p => p.LikeUserId == likeuserid && p.UserId == userid);
        }

        /// <summary>
        /// 关注或取消
        /// </summary>
        /// <param name="likeuserid"></param>
        /// <param name="ok"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Response AddOrRemoveLike(int likeuserid, bool ok, int userId)
        {
            var response = new Response();
            var W_Users = DbBase.SingleOrDefaultById<W_Users>(userId);
            if (W_Users == null)
            {
                response.Message = "非法操作";
                return response;
            }
            using (var tran = DbBase.GetTransaction())
            {
                if (!ok)
                {
                    W_Users.LikeNum--;
                    DbBase.DeleteMany<AsterLikes>().Where(p => p.LikeUserId == likeuserid && p.UserId == userId).Execute();
                }
                else
                {
                    W_Users.LikeNum++;
                    DbBase.Insert(new AsterLikes
                    {
                        LikeUserId = likeuserid,
                        UserId = userId
                    });
                }
                var result = DbBase.UpdateMany<W_Users>().OnlyFields(p => p.LikeNum).Where(p => p.Id == userId).Execute(W_Users);
                tran.Complete();
                if (IsUpdateSuccess(result))
                {
                    response.IsSuccess = true;
                    response.Message = "操作成功";
                    return response;
                }
            }
            response.Message = "操作失败";

            return response;
        }

        public int GetFansNum(int userid)
        {
            return DbBase.Query<AsterLikes>().Include(u => u.LikeUser).Where(u => u.UserId == userid).Count();
        }
    }
}
