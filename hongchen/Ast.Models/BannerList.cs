//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2018-09-10 13:24:14 by KeHen
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
namespace Ast.Models
{	
	public partial class BannerList
    {
		
		/// <summary>
		/// 主键ID
		/// </summary>		
		public int Id { get; set; }
		
		/// <summary>
		/// 标题
		/// </summary>		
		public string Title { get; set; }
		
		/// <summary>
		/// 链接
		/// </summary>		
		public string Url { get; set; }
		
		/// <summary>
		/// 排序
		/// </summary>		
		public int Sort { get; set; }
		
		/// <summary>
		/// 备注
		/// </summary>		
		public string memo { get; set; }
		
		/// <summary>
		/// 添加时间
		/// </summary>		
		public DateTime AddTime { get; set; }
		
		/// <summary>
		/// 修改时间
		/// </summary>		
		public DateTime ModifyTime { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
		public string ImgUrl { get; set; }
		   
    }
}

