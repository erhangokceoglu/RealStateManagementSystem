using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.Dtos.AppUser
{
    public class CreateForAppUserDto
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime? CreateDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public int RoleId { get; set; }
    }
}
