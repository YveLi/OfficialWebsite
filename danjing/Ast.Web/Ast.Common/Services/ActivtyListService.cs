using KeHenUI.Social.Common.Models;
using System;
using KeHenUI.Social.Common.Extension;
using KeHenUI.Social.Common.Infrastructure;
using KeHenUI.Social.Common.Services.Messaging;
using System.Collections.Generic;

namespace KeHenUI.Social.Common.Services
{
    public class ActivtyListService : BaseService<ActivtyList, int>
    {
        public ActivtyList GetActivtyInfo(int id)
        {
            return DbBase.SingleOrDefaultById<ActivtyList>(id);
        }

        /// <summary>
        /// 访问次数+10的随机数
        /// </summary>
        /// <param name="entity"></param>
        public void VisitPlusOne(ActivtyList entity)
        {
            if (entity != null)
            {
                Random r = new Random();
                entity.visit += r.Next(1, 11);
                DbBase.UpdateMany<ActivtyList>().OnlyFields(p => p.visit).Where(p => p.Id == entity.Id).Execute(entity);
            }
        }
    }
}
