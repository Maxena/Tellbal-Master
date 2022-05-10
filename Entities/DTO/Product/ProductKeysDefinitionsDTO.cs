using System.Collections.Generic;

namespace Entities.DTO
{
    public class ProductKeysDefinitionsDTO
    {
        public int CategoryId { get; set; }
        public List<PropertyKeyForCreateDTO> PropertyKeys { get; set; }
    }
}
