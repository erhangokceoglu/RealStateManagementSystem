using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.Dtos.AppUser
{
    public class PasswordChangeForAppUserDto
    {
        public int Id { get; set; }
        public string NewPassword { get; set; } = null!;
        public string RepeatNewPassword { get; set; } = null!;
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}
