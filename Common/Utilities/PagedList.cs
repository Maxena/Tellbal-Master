using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Utilities
{
    /// <summary>
    /// لیست صفحه بندی شده
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T>
    {
        /// <summary>
        /// دیتا
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// صفجه جاری
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// تعداد صفحات
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// تعداد رکورد در هر صفحه
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// تعداد کل رکوردها
        /// </summary>
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(
            IQueryable<T> source,
           int pageNumber,
           int pageSize)
        {
            var count = await source
                .CountAsync();

            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>
            {
                Data = items,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                TotalCount = count
            };
        }
    }
}
