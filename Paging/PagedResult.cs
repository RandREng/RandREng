﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandREng.Paging
{
    public class PagedResult<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}
