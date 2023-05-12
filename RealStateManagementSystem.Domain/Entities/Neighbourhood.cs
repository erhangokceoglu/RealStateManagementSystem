using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RealStateManagementSystem.Domain.Entities
{
    public class Neighbourhood : BaseEntity
    {
        public string Name { get; set; } = null!;

        //Navigation Property
        public int DistrictId { get; set; }

        public District District { get; set; } = null!;

        public ICollection<RealState> RealStates { get; set; } = new List<RealState>();
    }
}
