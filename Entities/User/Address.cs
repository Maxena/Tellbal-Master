using Entities.Product.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.User
{
    public class Address : BaseEntity<Guid>
    {
        public string Label { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string DetailedAddress { get; set; }
        public string PostalCode { get; set; }
        public string ContactNumber { get; set; }
        public string ContactName { get; set; }
        public Identity.User User { get; set; }
        public Guid UserId { get; set; }
    }
    public class AddressConfiguartion : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {

        }
    }
}
