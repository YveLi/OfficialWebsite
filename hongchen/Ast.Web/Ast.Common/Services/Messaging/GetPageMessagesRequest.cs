using KeHenUI.Social.Common.Infrastructure;
namespace KeHenUI.Social.Common.Services.Messaging
{
    public class GetPageMessagesRequest : RequestBaseOfPaging
    {
        public GetPageMessagesRequest(int pageIndex, int pageSize, int userId) : base(pageIndex, pageSize)
        {
            UserId = userId;
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; private set; }
    }
}
