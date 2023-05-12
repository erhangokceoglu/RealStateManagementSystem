using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }

        bool IsActive { get; set; }
    }
}
