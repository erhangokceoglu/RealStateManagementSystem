using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Infastructure.Configurations
{
    public class RealStateConfiguration : IEntityTypeConfiguration<RealState>
    {
        public void Configure(EntityTypeBuilder<RealState> builder)
        {
            builder.Property(x => x.IslandNo).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ParcelNo).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Address).HasMaxLength(180);
            builder.Property(x => x.Latitude).HasMaxLength(100);
            builder.Property(x => x.Longitude).HasMaxLength(100);
            builder.Property(x => x.CreateDate).IsRequired(false).HasColumnType("date");
            builder.Property(x => x.UpdateDate).IsRequired(false).HasColumnType("date");
        }
    }
}
