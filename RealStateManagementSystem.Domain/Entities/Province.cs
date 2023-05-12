using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RealStateManagementSystem.Domain.Entities
{
    public class Province : BaseEntity
    {
        public string Name { get; set; } = null!;

        public ICollection<District> Districts { get; set; } = new List<District>();

        public ICollection<RealState> RealStates { get; set; } = new List<RealState>();
    }
}
