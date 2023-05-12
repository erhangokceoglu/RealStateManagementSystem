using RealStateManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.Dtos.RealState
{
    public class CreateForRealStateDto
    {
        public string IslandNo { get; set; } = null!; // Ada No

        public string ParcelNo { get; set; } = null!; // Parsel No

        public Qualification Qualification { get; set; } // Nitelik

        public string? Address { get; set; }

        public DateTime? CreateDate { get; set; } = DateTime.Now;

        public string? Latitude { get; set; }

        public string? Longitude { get; set; }

        //public int? AppUserId { get; set; }

        public int ProvinceId { get; set; }

        public int DistrictId { get; set; }

        public int NeighbourhoodId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
