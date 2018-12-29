using Ast.Bll;
using Ast.IBll;
using Ast.Models;
using Ast.Web.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ast.Web.Controllers
{
    public class HomeController : BaseController
    {
        private IArticleListService articleListService = new ArticleListService();
        private IBannerListService bannerListService = new BannerListService();
        private IArticleTypeListService articleTypeListService = new ArticleTypeListService();
        private IArticleMarkListService articleMarkListService = new ArticleMarkListService();
        private IArticleCommentListService commentListService = new ArticleCommentListService();
        private IArticleChildCommentService childCommentService = new ArticleChildCommentService();
        private IMemberListService memberListService = new MemberListService();
        [AllowAnonymous]
        public ActionResult Index(string query = "", int page = 1, int limit = 16)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit
            };
            var articleList = articleListService.GetPageList(pagemodel, o => true);
            IList<ArticleModel> md = new List<ArticleModel>();
            foreach (ArticleList item in articleList.datalist)
            {
                var articlemd = new ArticleModel();
                articlemd.Articlemd = item;
                var typeList = articleTypeListService.GetSingle(o => o.Id == item.ArticleTypeId);
                articlemd.Typemd = typeList;
                var markList = articleMarkListService.GetSingle(o => o.Id == item.MarkId);
                articlemd.Markmd = markList;
                md.Add(articlemd);
            }
            ViewBag.articleModel = md;
            ViewBag.bannerList = bannerListService.GetList(o => true);
            ViewBag.typeList = articleTypeListService.GetList(o => true);
            return View();
        }

        public ActionResult Detail(int id=1)
        {
            var article = articleListService.GetById(id.ToString());
            article.ReadNum += 1;
            articleListService.Update(article);//更新阅读量+1
            ArticleModel articlemd = new ArticleModel();
            articlemd.Articlemd = article;
            var typeList = articleTypeListService.GetSingle(o => o.Id == article.ArticleTypeId);
            articlemd.Typemd = typeList;
            var markList = articleMarkListService.GetSingle(o => o.Id == article.MarkId);
            articlemd.Markmd = markList;

            var commentList = commentListService.GetList(o => o.ArticleId == id,o=>o.AddTime,0);
            IList<ArticleCommentModel> commentModel = new List<ArticleCommentModel>();
            foreach (var item in commentList)
            {
                var acm = new ArticleCommentModel();
                acm.Commentmd = item;
                var member = memberListService.GetById(item.UserId.ToString());
                acm.Membermd = member;
                var childList = childCommentService.GetList(o => o.CommentId == item.Id);
                acm.Childmd = childList;
                commentModel.Add(acm);
            }
            ViewBag.commentList = commentModel;
            ViewBag.article = articlemd;
            return View();        }
        //增加评论
        public JsonResult AddComment(ArticleCommentList comment, ArticleChildComment childComment, int PostId)
        {
            //区分两种情况，一个是评论回复（二级）；一个是评论
            if (childComment.CommentId > 0)
            {
                childComment.FromUserId = CurrentUserId;
                childComment.AddTime = DateTime.Now;
                childComment.FromUserName = CurrentName;
                childCommentService.Add(childComment);
            }
            else
            {
                comment.UserId = CurrentUserId;
                comment.AddTime = DateTime.Now;
                comment.ModifyTime = DateTime.Now;
                comment.ArticleId = PostId;
                commentListService.Add(comment);
                var Post = articleListService.GetById(PostId.ToString());
                Post.ModifyTime = DateTime.Now;
                Post.CommentNum += 1;
                articleListService.Update(Post);
            }
            return Json(new { success = true, msg = "评论成功", data = PostId }, JsonRequestBehavior.AllowGet);
        }
    }
}