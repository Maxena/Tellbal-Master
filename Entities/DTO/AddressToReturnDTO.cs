using System;

namespace Entities.DTO
{
    public class AddressToReturnDTO
    {
        public Guid AddressId { get; set; }
        public string Label { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string DetailedAddress { get; set; }
        public string PostalCode { get; set; }
        public string ContactNumber { get; set; }
        public string ContactName { get; set; }
        public Guid UserId { get; set; }
    }
}
