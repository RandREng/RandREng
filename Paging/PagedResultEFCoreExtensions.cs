using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Linq;

namespace RandREng.Paging
{
    public static partial class PagedResultExtensions
    {
        static public MapperConfiguration Config {set;get;}

        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            PagedResult<T> result = new()
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            double pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            int skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public static PagedResult<U> GetPaged<T, U>(this IQueryable<T> query, int page, int pageSize) where U : class
        {
            PagedResult<U> result = new()
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            double pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            int skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip)
                                  .Take(pageSize)
                                  .ProjectTo<U>(Config)
                                  .ToList();
            return result;
        }

    }
}
