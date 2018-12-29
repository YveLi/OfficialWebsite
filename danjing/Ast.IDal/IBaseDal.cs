using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ast.Models.ViewModels;
using SqlSugar;

namespace Ast.IDal
{
    public interface IBaseDal<T> where T : class, new()
    {
        bool IsAny(Expression<Func<T, bool>> Lambda);
        T GetById(string id);
        T GetSingle(Expression<Func<T, bool>> Lambda);
        int Add(T entity);
        bool Update(T entity);
        bool Updates(IList<T> list);
        bool Deletes(T entity);
        bool DeleteById(string id);
        bool DeleteByIds(params string[] ids);
        bool DeleteByLambda(Expression<Func<T, bool>> Lambda);
        IList<T> GetList(dynamic[] ids);
        IList<T> GetList(Expression<Func<T, bool>> Lambda);
        IList<T> GetList(Expression<Func<T, bool>> Lambda, Expression<Func<T, object>> orderby, int type);
        PageList<T> GetPageList(SqlSugar.PageModel page, Expression<Func<T, bool>> Lambda);
        PageList<T> GetPageList(SqlSugar.PageModel page, Expression<Func<T, bool>> Lambda, Expression<Func<T, object>> orderby, int type);
    }
}
