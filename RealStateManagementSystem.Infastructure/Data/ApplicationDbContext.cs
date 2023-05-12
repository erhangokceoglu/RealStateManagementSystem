using Microsoft.EntityFrameworkCore;
using RealStateManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RealStateManagementSystem.Infastructure.Extensions;

namespace RealStateManagementSystem.Infastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public const string _connectionString = "AplicationDbConnection";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers => Set<AppUser>();

        public DbSet<RealState> RealStates => Set<RealState>();

        public DbSet<Province> Provinces => Set<Province>();

        public DbSet<Neighbourhood> Neighbourhoods => Set<Neighbourhood>();

        public DbSet<District> Districts => Set<District>();

        public DbSet<Log> Logs => Set<Log>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<Domain.Entities.Token> Tokens => Set<Domain.Entities.Token>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelBuilderExtensions.SeedData(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
