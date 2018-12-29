using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ast.Models.ViewModels;
using SqlSugar;

namespace Ast.Dal
{
    public class BaseDal<T> where T : class, new()
    {

        private readonly SqlSugarClient Context = new DbContextFactory().GetCurrentDbContext();

        public virtual bool IsAny(Expression<Func<T, bool>> Lambda)
        {
            return Context.GetSimpleClient().IsAny(Lambda);
        }
        public virtual T GetById(string id)
        {
            return Context.GetSimpleClient().GetById<T>(id);
        }

        public virtual T GetSingle(Expression<Func<T, bool>> Lambda)
        {
            return Context.GetSimpleClient().GetSingle(Lambda);
        }
        public virtual int Add(T entity)
        {
            return Context.GetSimpleClient().InsertReturnIdentity<T>(entity);
        }

        public virtual bool Update(T entity)
        {
            return Context.GetSimpleClient().Update(entity);
        }
        public virtual bool Updates(IList<T> list)
        {
            return Context.GetSimpleClient().UpdateRange((list.ToArray()));
        }

        public virtual bool Deletes(T entity)
        {
            return Context.GetSimpleClient().Delete(entity);
        }

        public virtual bool DeleteById(string id)
        {
            return Context.GetSimpleClient().DeleteById<T>(id);
        }

        public virtual bool DeleteByIds(params string[] ids)
        {
            return Context.Deleteable<T>().In(ids).ExecuteCommand() > 0;
        }

        public virtual bool DeleteByLambda(Expression<Func<T, bool>> Lambda)
        {
            return Context.Deleteable<T>().Where(Lambda).ExecuteCommand() > 0;
        }

        public virtual IList<T> GetList(dynamic[] ids)
        {
            return Context.Queryable<T>().In(ids).ToList();
        }
        public virtual IList<T> GetList(Expression<Func<T, bool>> Lambda)
        {
            return Context.Queryable<T>().Where(Lambda).OrderBy("Id desc").ToList();
        }
        public virtual IList<T> GetList(Expression<Func<T, bool>> Lambda, Expression<Func<T, object>> orderby, int type = 0)
        {
            if (type == 0)
            {
                return Context.Queryable<T>().Where(Lambda).OrderBy(orderby, OrderByType.Asc).ToList();
            }
            else
            {
                return Context.Queryable<T>().Where(Lambda).OrderBy(orderby, OrderByType.Desc).ToList();
            }
        }

        public virtual PageList<T> GetPageList(SqlSugar.PageModel page, Expression<Func<T, bool>> Lambda)
        {
            int total = 0;
            var list = Context.Queryable<T>().Where(Lambda).OrderBy("Id desc").ToPageList(page.PageIndex, page.PageSize, ref total);
            return new PageList<T>(list, total);
        }
        public virtual PageList<T> GetPageList(SqlSugar.PageModel page, Expression<Func<T, bool>> Lambda, Expression<Func<T, object>> orderby, int type)
        {
            int total = 0;
            if (type == 0)
            {
                var list = Context.Queryable<T>().Where(Lambda).OrderBy(orderby, OrderByType.Asc).ToPageList(page.PageIndex, page.PageSize, ref total);
                return new PageList<T>(list, total);
            }
            else
            {
                var list = Context.Queryable<T>().Where(Lambda).OrderBy(orderby, OrderByType.Desc).ToPageList(page.PageIndex, page.PageSize, ref total);
                return new PageList<T>(list, total);
            }
        }
    }
}
