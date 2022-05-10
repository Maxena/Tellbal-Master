using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.Product
{
    public class ProductKeysValuesToReturnDTO
    {
        public List<PropertyKeyDTO> PropertyKeys { get; set; }
        public List<PropertyValueDTO> PropertyValues { get; set; }
    }
}
