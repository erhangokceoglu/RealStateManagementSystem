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
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.Property(x => x.State).HasMaxLength(80);
            builder.Property(x => x.ProcessType).HasMaxLength(50);
            builder.Property(x => x.UserIp).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(180);
            builder.Property(x => x.CreateDate).IsRequired(false).HasColumnType("date");
            builder.Property(x => x.UpdateDate).IsRequired(false).HasColumnType("date");
        }
    }
}
