using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.Entities
{
    public class District : BaseEntity
    {
        public string Name { get; set; } = null!;

        //Navigation Property
        public int ProvinceId { get; set; }

        public Province Province { get; set; } = null!;

        public ICollection<Neighbourhood> Neighborhoods { get; set; } = new List<Neighbourhood>();
    }
}
