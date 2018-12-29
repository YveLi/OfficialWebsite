using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using Ast.Models;
using System.Linq.Expressions;
using SyntacticSugar;

namespace Ast.Dal
{
    public class DbContextFactory
    {

        public SqlSugarClient DB;

        public DbContextFactory()
        {
            DB = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConfigSugar.GetConfigString("AstConn"),
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });
            //加入Aop
            //DB.Aop.OnLogExecuting = (sql, pars) =>
            //{
            //    Console.WriteLine(sql + "\r\n" + DB.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
            //    Console.WriteLine();
            //};
        }

        public SqlSugarClient GetCurrentDbContext()
        {
            return DB;
        }

    }
}
