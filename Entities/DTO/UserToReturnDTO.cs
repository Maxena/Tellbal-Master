using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class UserToReturnDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public string SnapShot { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    }
}
