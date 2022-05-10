namespace Entities.DTO
{
    /// <summary>
    /// نتیجه پیامک رمز
    /// </summary>
    public class OtpResponseDTO
    {
        /// <summary>
        /// موفقیت آمیز
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// پیام
        /// </summary>
        public string Message { get; set; }
    }
}
