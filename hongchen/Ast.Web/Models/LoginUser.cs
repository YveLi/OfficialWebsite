using Ast.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ast.Web.Models
{
    public class LoginUser
    {
        public string LoginName { get; set; }
        public string PassWord { get; set; }

    }
    public class PostModel
    {
        public ClubPostList postmd { get; set; }
        public MemberList membermd { get; set; }
        public string posttypename { get; set; }
    }
    public class CommentModel//这个是POST的评论，下面那个是Article的评论
    {
        public ClubPostCommentList Commentmd { get; set; }
        public MemberList Membermd { get; set; }
        public IList<ClubChildComment> childComments { get; set; }
    }
    public class ArticleModel
    {
        public ArticleList Articlemd { get; set; }
        public ArticleTypeList Typemd { get; set; }
        public ArticleMarkList Markmd { get; set; }
    }
    public class ArticleCommentModel
    {
        public ArticleCommentList Commentmd { get; set; }
        public MemberList Membermd { get; set; }
        public IList<ArticleChildComment> Childmd { get; set; }
    }
}