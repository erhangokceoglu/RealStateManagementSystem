using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.Dtos.AppUser
{
    public class UpdateForAppUserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime? UpdateDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; }

        public int RoleId { get; set; } 
    }
}
