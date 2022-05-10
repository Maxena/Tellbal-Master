using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.System
{
    public class FAQ : BaseEntity<Guid>
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Arrange { get; set; }
    }
}
