using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class PropertyValueForCreateDTO
    {
        public Guid? Id { get; set; }
        public string Value { get; set; }
        public Guid PropertyKeyId { get; set; }
    }
}
