namespace Common.Utilities
{
    public class PaginationParams<T>
    {
        /// <summary>
        /// پارامتر جستجو
        /// </summary>
        public T Query { get; set; }

        /// <summary>
        /// صفحه جاری
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// تعداد آیتم در هر صفحه
        /// </summary>
        public int PageSize { get; set; } = 5;
    }
}
