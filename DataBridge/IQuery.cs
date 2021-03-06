﻿using System.Collections.Generic;

namespace DataBridge
{
    public interface IQuery<TModel>
    {
        IQuery<TModel> Filter(StandardFilter filter);
        IQuery<TModel> Sort(params IndexSort[] indexSorts);
        IQuery<TModel> Page(int pageSize, int currentPage = 1);
        int Count();
        IList<TModel> ToList();
    }
}