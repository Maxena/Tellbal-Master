using System;

namespace Entities.DTO
{
    public class ProfileToReturnDTO
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Image_S { get; set; }
        public string Image_M { get; set; }
        public string Image_L { get; set; }
    }
}
