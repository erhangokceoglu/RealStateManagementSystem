using RealStateManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.Entities
{
    public class RealState : BaseEntity
    {
        public string IslandNo { get; set; } = null!; // Ada No

        public string ParcelNo { get; set; } = null!; // Parsel No

        public Qualification Qualification { get; set; } // Nitelik

        public string? Address { get; set; } 

        public string? Latitude { get; set; }

        public string? Longitude { get; set; }

        //Navigation Property
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; } = null!;

        //Navigation Property
        public int ProvinceId { get; set; }

        public Province Province { get; set; } = null!;

        //Navigation Property
        public int DistrictId { get; set; }

        public District District { get; set; } = null!;

        //Navigation Property
        public int NeighbourhoodId { get; set; }

        public Neighbourhood Neighbourhood { get; set; } = null!;
    }
}
