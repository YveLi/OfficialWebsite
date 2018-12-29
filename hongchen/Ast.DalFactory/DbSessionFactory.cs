using Ast.IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Ast.DalFactory
{
    public class DbSessionFactory
    {
        public static IDbSession GetCurrentDbSession()
        {
            IDbSession dbSession = (IDbSession)CallContext.GetData("DbSession");
            if (dbSession != null) return dbSession;
            dbSession = new DbSession();
            CallContext.SetData("DbSession", dbSession);
            return dbSession;
        }
    }
}
