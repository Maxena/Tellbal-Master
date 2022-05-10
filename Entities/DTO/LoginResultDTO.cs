using System;
using System.Collections.Generic;

namespace Entities.DTO
{
    public class LoginResultDTO
    {
        public bool IsAuthenticated { get; set; } = false;
        public List<string> Roles { get; set; } = new List<string>();
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public string SnapShot { get; set; } = string.Empty;
    }
}
