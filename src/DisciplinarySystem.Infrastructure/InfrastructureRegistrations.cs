using DisciplinarySystem.Application.Contracts.Interfaces;
using DisciplinarySystem.Infrastructure.Apis.Messengers;
using DisciplinarySystem.Infrastructure.Apis.Positions;
using DisciplinarySystem.Infrastructure.Apis.Users;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DisciplinarySystem.Infrastructure
{
    public static class InfrastructureRegistrations
    {
        public static IServiceCollection AddInfrastructureLayerDependencies ( this IServiceCollection services )
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IUserApi , UserApi>();
            services.AddScoped<ISmsService , SmsService>();
            services.AddScoped<IPositionAPI , PositionAPI>();
            return services;
        }
    }
}
