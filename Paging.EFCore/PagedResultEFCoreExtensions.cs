using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RandREng.Paging.EFCore
{
    public static class PagedResultEFCoreExtensions 
    {
        static public MapperConfiguration Config { set; get; }

        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            PagedResult<T> result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync()
            };

            double pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            int skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return result;
        }

        public static async Task<PagedResult<U>> GetPagedAsync<T, U>(this IQueryable<T> query, int page, int pageSize) where U : class
        {
            PagedResult<U> result = new PagedResult<U>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync()
            };

            double pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            int skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip)
                                        .Take(pageSize)
                                        .ProjectTo<U>(Config)
                                        .ToListAsync();
            return result;
        }
    }
}
