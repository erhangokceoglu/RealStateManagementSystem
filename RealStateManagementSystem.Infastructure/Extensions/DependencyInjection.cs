using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealStateManagementSystem.Domain.Entities;
using RealStateManagementSystem.Domain.IServices;
using RealStateManagementSystem.Infastructure.Data;
using RealStateManagementSystem.Infastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Infastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection InfDependencyResolver(this IServiceCollection services, IConfiguration configuration)
        {
            var myConnectionString = configuration.GetConnectionString(ApplicationDbContext._connectionString);

            services.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(myConnectionString));

            services.AddScoped<IAppUserService, AppUserService>()
                    .AddScoped<ILogService, LogService>()
                    .AddScoped<IRealStateService, RealStateService>();

            services.AddScoped<AppUser>()
                    .AddScoped<Log>()
                    .AddScoped<RealState>()
                    .AddScoped<Token>();

            return services;
        }
    }
}
