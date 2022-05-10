using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Product
{
    public class ProductValueToReturnDTO
    {
        public Guid PropertyValueId { get; set; }
        public string Value { get; set; }
        public Guid PropertyKeyId { get; set; }
    }
}
