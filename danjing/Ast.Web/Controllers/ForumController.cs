using Ast.Bll;
using Ast.IBll;
using Ast.Models;
using System.Web.Mvc;
using System;
using Ast.Web.Models;
using System.Collections;
using System.Collections.Generic;
using Ast.Common;
using SqlSugar;

namespace Ast.Web.Controllers
{
    public class ForumController : BaseController
    {
        private IClubPostCommentListService PostCommentListService = new ClubPostCommentListService();
        private IClubPostTypeService PostTypeService = new ClubPostTypeService();
        private IClubPostListService PostListService = new ClubPostListService();
        private IMemberListService memberListSevice = new MemberListService();
        private IClubChildCommentService childCommentService = new ClubChildCommentService();
        // GET: Forum
        [AllowAnonymous]
        #region 论坛首页
        public ActionResult Index(int typeId = 0, int page = 1, int limit = 15)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit
            };
            ViewBag.PostType = PostTypeService.GetList(o => true);
            if (typeId == 0)
            {
                var PostList = PostListService.GetPageList(pagemodel, o => true);
                IList<PostModel> md = new List<PostModel>();
                foreach (ClubPostList item in PostList.datalist)
                {
                    var artmd = new PostModel();
                    artmd.postmd = item;
                    var memList = memberListSevice.GetSingle(o => o.Id == item.MemberId);
                    artmd.membermd = memList;
                    md.Add(artmd);
                }
                ViewBag.Postmodel = md;
            }
            else
            {
                var PostList = PostListService.GetPageList(pagemodel, o => o.PostTypeId == typeId);
                IList<PostModel> md = new List<PostModel>();
                foreach (ClubPostList item in PostList.datalist)
                {
                    var artmd = new PostModel();
                    artmd.postmd = item;
                    var memList = memberListSevice.GetSingle(o => o.Id == item.MemberId);
                    artmd.membermd = memList;
                    md.Add(artmd);
                }
                ViewBag.Postmodel = md;
            }
            return View();
        }

        [ValidateInput(false)]//用来允许用户发帖时上传标签
        //发帖
        public JsonResult Add(ClubPostList Post)
        {
            var list = PostListService.GetList(o => o.Title.Contains(Post.Title));
            if (list.Count > 0)
            {
                return Json(new { success = false, msg = "这个帖子已经发表过了" }, JsonRequestBehavior.AllowGet);
            }
            MemberList user = Session["Users"] as MemberList;

            ClubPostList md = new ClubPostList();
            md.Title = Post.Title;
            md.PostContent = Post.PostContent;
            md.PostTypeId = Post.PostTypeId;
            md.AddTime = DateTime.Now;
            md.ModifyTime = DateTime.Now;
            md.LastReplyTime = DateTime.Now;
            md.MemberId = user.Id;
            PostListService.Add(md);
            return Json(new
            {
                success = true,
                msg = "发表成功！",
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 帖子详情
        //帖子主页
        [AllowAnonymous]
        public ActionResult PostDetail(int id)
        {
            //参数的id是帖子对应的Id 
            IList<CommentModel> md = new List<CommentModel>();//整串对象
            var Post = PostListService.GetById(id.ToString());
            //每点击进来一次帖子，点击量就加一
            Post.LookNum += 1;
            PostListService.Update(Post);
            var member = memberListSevice.GetById(Post.MemberId.ToString());
            string PostTypeName = PostTypeService.GetSingle(o => o.Id == Post.PostTypeId).TypeName;

            var comment = PostCommentListService.GetList(o => o.PostId == id);
            for (var n = comment.Count - 1; n >= 0; n--)
            {
                //倒序
                CommentModel commentModel = new CommentModel();//包含用户信息的评论实体对象
                var user = memberListSevice.GetById(comment[n].MemberId.ToString());
                var childComments = childCommentService.GetList(o => o.PostCommentId == comment[n].Id);
                commentModel.Commentmd = comment[n];
                commentModel.Membermd = user;
                commentModel.childComments = childComments;
                md.Add(commentModel);
            }
            PostModel am = new PostModel();
            am.membermd = member;
            am.postmd = Post;
            am.posttypename = PostTypeName;
            ViewBag.Post = am;
            ViewBag.comment = comment;
            ViewBag.commentModel = md;
            ViewBag.PostType = PostTypeService.GetList(o => true);
            return View();
        }
        //增加评论
        public JsonResult AddComment(ClubPostCommentList comment, ClubChildComment childComment, int PostId)
        {
           
            //区分两种情况，一个是评论回复（二级）；一个是评论
            if (childComment.PostCommentId != 0)
            {
                childComment.FromUserId = CurrentUserId;
                childComment.AddTime = DateTime.Now;
                childComment.FromUserName = CurrentName;
                childCommentService.Add(childComment);
            }
            else
            {
                comment.MemberId = CurrentUserId;
                comment.AddTime = DateTime.Now;
                comment.PostId = PostId;
                PostCommentListService.Add(comment);
                var Post = PostListService.GetById(PostId.ToString());
                Post.LastReplyTime = DateTime.Now;
                Post.ReplyCount += 1;
                PostListService.Update(Post);
            }
            return Json(new { success = true, msg = "评论成功", data = PostId }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}