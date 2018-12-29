using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ast.Models.DBModels
{
    public class OnlineVisitorsResult
    {
        //public Users User { get; set; }

        public string LastActionIp { get; set; }

        public DateTime LastActionTime { get; set; }

        public string TypeName { get; set; }
    }
}