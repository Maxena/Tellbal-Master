using System;

namespace Entities.DTO
{
    public class ProfileToSetDTO
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    }
}
