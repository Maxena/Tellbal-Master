using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Product
{
    public class Color : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
    }
}
