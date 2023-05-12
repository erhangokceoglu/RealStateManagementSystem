using RealStateManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.Dtos.RealState
{
    public class UpdateForRealStateDto
    {
        public int Id { get; set; }

        public string IslandNo { get; set; } = null!; // Ada No

        public string ParcelNo { get; set; } = null!; // Parsel No

        public int Qualification { get; set; }

        public string Address { get; set; } = null!;

        public int ProvinceId { get; set; }

        public int DistrictId { get; set; }

        public int NeighbourhoodId { get; set; }

        public DateTime? UpdateDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; }
    }
}
