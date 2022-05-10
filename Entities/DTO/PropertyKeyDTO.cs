using Entities.Product.Dynamic;
using System;

namespace Entities.DTO
{
    public class PropertyKeyDTO
    {
        public Guid PropertyKeyId { get; set; }
        public string Name { get; set; }
        public KeyType KeyType { get; set; }
    }
}
