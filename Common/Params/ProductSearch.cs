namespace Common.Params
{
    public class ProductSearch
    {
        /// <summary>
        /// جستجو در داخل یک دسته بندی
        /// </summary>
        public int? CategoryId { get; set; } = null;

        /// <summary>
        /// متن جستجو
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// ترتیب مرتب سازی
        /// </summary>
        public SearchOrder? SearchOrder { get; set; }

        /// <summary>
        /// جست و جو بر اساس نوع محصول
        /// </summary>
        public ProductType? ProductType { get; set; }

        /// <summary>
        /// جست و جو در داخل یک برند
        /// </summary>
        public int? BrandId { get; set; } = null;

        /// <summary>
        /// جست و جو در داخل یک مدل
        /// </summary>
        public int? ModelId { get; set; } = null;
    }
}

namespace Common
{
    /// <summary>
    /// مرتب سازی
    /// </summary>
    public enum SearchOrder
    {
        /// <summary>
        /// جدید ترین
        /// </summary>
        New,
        /// <summary>
        /// گران ترین
        /// </summary>
        Expensive,
        /// <summary>
        /// ارزان ترین
        /// </summary>
        Cheap,
    }
    public enum ProductType
    {
        /// <summary>
        /// محصول نو
        /// </summary>
        New,

        /// <summary>
        /// محصول کارکرده
        /// </summary>
        Used
    }
}