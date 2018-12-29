using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast.Models.DBModels
{
    public class LoginHistory
    {

        public int Uid { get; set; }

        public string UniqueKey { get; set; }

        public DateTime CreateDate { get; set; }

        public Boolean IsDeleted { get; set; }

    }
}
