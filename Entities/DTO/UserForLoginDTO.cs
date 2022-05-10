using System.ComponentModel.DataAnnotations;

namespace Entities.DTO
{
    public class UserForLoginDTO
    {
        /// <summary>
        /// نام کاربری
        /// </summary>
        [Required(ErrorMessage = "وارد کردن نام کاربری الزامی است")]
        public string UserName { get; set; }

        /// <summary>
        /// رمز عبور
        /// </summary>
        [Required(ErrorMessage = "وارد کردن رمز عبور الزامی است")]
        public string PassWord { get; set; }
    }
}
