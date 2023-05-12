using Microsoft.AspNetCore.Identity;
using RealStateManagementSystem.Domain.Enums;
using RealStateManagementSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RealStateManagementSystem.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public byte[] PasswordSalt { get; set; } = null!;

        public byte[] PasswordHash { get; set; } = null!;

        public string Address { get; set; } = null!;

        //Navigation Property
        public int RoleId { get; set; }

        public Role Role { get; set; } = null!;

        public ICollection<Log>? Logs { get; set; } = new List<Log>();

        public ICollection<RealState> RealStates { get; set; } = new List<RealState>();
    }
}
