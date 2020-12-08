using Application.Common.Interfaces;
using Domain.Entities;
using Infractructure.BulkData;
using Infractructure.Dapper;
using Infractructure.Dapper.Repositories;
using Infractructure.Persistence;
using Infractructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infractructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient(typeof(IBulkImport<Agency>), typeof(AgencyBulkImport));
            services.AddTransient(typeof(ConnectionService));

            services.AddTransient<IAgentRepository, AgentRepository>();
            services.AddTransient<IAgencyRepository, AgencyRepository>();

            return services;
        }
    }
}