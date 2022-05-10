using System;

namespace Entities.DTO
{
    public class PropertyValueDTO
    {
        public Guid PropertyValueId { get; set; }
        public string Value { get; set; }
        public Guid PropertyKeyId { get; set; }

    }
}
