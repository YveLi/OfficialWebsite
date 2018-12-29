﻿using KeHenUI.Social.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeHenUI.Social.Common.Services
{
    public class LuckyDrawListService : BaseService<LuckyDrawList, int>
    {
        public IList<LuckyDrawList> GetLuckyDrawList()
        {
            return DbBase.Query<LuckyDrawList>().ToList();
        }

    }
}
