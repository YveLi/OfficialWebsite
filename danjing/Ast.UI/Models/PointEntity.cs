using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ast.UI.Models
{
    public class PointEntity
    {
        public int id { get; set; }
        public int pid { get; set; }
        public string name { get; set; }
        public bool isParent { get; set; }
        public bool spread { get; set; }
        public List<PointEntity> children { get; set; }

    }
    public class LoginUser
    {
        public string LoginName { get; set; }
        public string PassWord { get; set; }

    }
}