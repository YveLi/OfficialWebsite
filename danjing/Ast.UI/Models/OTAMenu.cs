using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ast.UI.Models
{
    public class OTAMenu
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string MenuName { get; set; }
        public string Icon { get; set; }
        public string MenuUrl { get; set; }
        public bool Checked { get; set; }
        public string MenuFunction { get; set; }
        public List<OTAMenu> Children { get; set; }
    }
}