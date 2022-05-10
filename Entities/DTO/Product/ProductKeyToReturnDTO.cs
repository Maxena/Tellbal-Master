using Entities.Product.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Product
{
    public class ProductKeyToReturnDTO
    {
        public Guid PropertyKeyId { get; set; }
        public string Name { get; set; }
        public KeyType KeyType { get; set; }
        public ProductValueToReturnDTO ProductValue { get; set; }
    }
}
