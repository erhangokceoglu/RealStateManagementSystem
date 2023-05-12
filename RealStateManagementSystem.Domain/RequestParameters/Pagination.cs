using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.RequestParameters
{
    public record Pagination
    {
        public short Page { get; set; } = 0;

        public short Size { get; set; } = 5;
    }
}
