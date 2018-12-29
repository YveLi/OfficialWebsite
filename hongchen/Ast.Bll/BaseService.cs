using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ast.IDal;
using Ast.Models.ViewModels;
using SqlSugar;

namespace Ast.Bll
{
    public abstract class BaseService<T> where T : class, new()
    {
        public IDbSession DbSession = DalFactory.DbSessionFactory.GetCurrentDbSession();
        public IBaseDal<T> CurrentDal;//依赖抽象接口 

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseService()
        {
            //执行给当前的 CurrentDal 赋值
            //强迫子类给当前类的 CurrentDal 属性赋值
            SetCurrentDal();
        }

        public abstract void SetCurrentDal();

        public virtual bool IsAny(Expression<Func<T, bool>> Lambda)
        {
            return CurrentDal.IsAny(Lambda);
        }

        public virtual T GetById(string id)
        {
            return CurrentDal.GetById(id);
        }

        public virtual T GetSingle(Expression<Func<T, bool>> Lambda)
        {
            return CurrentDal.GetSingle(Lambda);
        }

        public virtual int Add(T entity)
        {
            return CurrentDal.Add(entity);
        }

        public virtual bool Update(T entity)
        {
            return CurrentDal.Update(entity);
        }

        public virtual bool Updates(IList<T> list)
        {
            return CurrentDal.Updates(list);
        }

        public virtual bool DeleteById(string id)
        {
            return CurrentDal.DeleteById(id);
        }

        public virtual bool DeleteByIds(params string[] ids)
        {
            return CurrentDal.DeleteByIds(ids);
        }
        public virtual bool DeleteByLambda(Expression<Func<T, bool>> Lambda)
        {
            return CurrentDal.DeleteByLambda(Lambda);
        }
        public virtual IList<T> GetList(dynamic[] ids)
        {
            return CurrentDal.GetList(ids);
        }
        public virtual IList<T> GetList(Expression<Func<T, bool>> Lambda)
        {
            return CurrentDal.GetList(Lambda);
        }
        public virtual IList<T> GetList(Expression<Func<T, bool>> Lambda, Expression<Func<T, object>> orderby, int type)
        {
            return CurrentDal.GetList(Lambda, orderby, type);
        }

        public virtual PageList<T> GetPageList(SqlSugar.PageModel page, Expression<Func<T, bool>> Lambda)
        {
            return CurrentDal.GetPageList(page, Lambda);
        }
        public virtual PageList<T> GetPageList(SqlSugar.PageModel page, Expression<Func<T, bool>> Lambda, Expression<Func<T, object>> orderby, int type)
        {
            return CurrentDal.GetPageList(page, Lambda, orderby, type);
        }
    }
}
