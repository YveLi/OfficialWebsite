//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2018-12-20 17:51:18 by KeHen
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
namespace Ast.Models
{	
	public partial class ClubChildComment
    {
		
		/// <summary>
		/// 
		/// </summary>		
		public int Id { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
		public int PostCommentId { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
		public string CommentContent { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
		public int FromUserId { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
		public DateTime AddTime { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
		public string ToUserName { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
		public string FromUserName { get; set; }
		   
    }
}

