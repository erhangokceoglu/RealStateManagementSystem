using RealStateManagementSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.Entities
{
    public class Token : IEntity
    {
        public int Id { get; set; }
        public string? AccesToken { get; set; }
        public bool IsActive { get; set; }
    }
}
