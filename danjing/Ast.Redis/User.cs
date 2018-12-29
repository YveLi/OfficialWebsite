using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast.Redis
{
    [Serializable]
    public class User
    {
        public int Id;
        public string UserName;
        public int Age;
    }
}
