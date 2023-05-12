using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.Entities
{
    public class Log : BaseEntity
    {
        public string? State { get; set; }

        public string? ProcessType { get; set; }

        public string? Description { get; set; }

        public string? UserIp { get; set; }

        //Navigation Property
        public int? AppUserId { get; set; }

        public AppUser? AppUser { get; set; }
    }
}
