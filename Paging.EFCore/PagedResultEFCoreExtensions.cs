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
        static private MapperConfiguration config;

        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }

        public static async Task<PagedResult<U>> GetPagedAsync<T, U>(this IQueryable<T> query, int page, int pageSize) where U : class
        {
            var result = new PagedResult<U>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = await query.CountAsync();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip)
                                        .Take(pageSize)
                                        .ProjectTo<U>(config)
                                        .ToListAsync();
            return result;
        }
    }
}
