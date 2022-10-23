using DisciplinarySystem.Domain.Complaints.Interfaces;
using DisciplinarySystem.Domain.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Domain.Epistles.Interfaces;
using DisciplinarySystem.Persistence.Data;
using DisciplinarySystem.Persistence.Data.Initializer;
using DisciplinarySystem.Persistence.Data.Initializer.Interfaces;
using DisciplinarySystem.Persistence.Repositories;
using DisciplinarySystem.Persistence.Repositories.DisciplinaryCase;
using DisciplinarySystem.SharedKernel.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DisciplinarySystem.Persistence
{
    public static class PersistenceRegistrations
    {
        public static IServiceCollection AddPersistenceLayerDependencies (
            this IServiceCollection services , IConfiguration configuration , bool isDevelopment )
        {
            #region DataBase

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            #endregion


            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IRepository<>) , typeof(Repository<>));
            services.AddScoped<IComplaintRepository , ComplaintRepository>();
            services.AddScoped<IEpistleRepository , EpistleRepository>();
            services.AddScoped<ICaseReposiotry , CaseRepository>();
            services.AddScoped<IDbInitializer , DbInitializer>();
            return services;
        }
    }
}
