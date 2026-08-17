using Application.Interfaces;
using Application.Services;
using Domain.Genericos;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace CrudNotasFiscais.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection DependencInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped <INotaFiscalRepository, NotaFiscalRepository>();
            services.AddScoped <INotaFiscalService, NotaFiscalService>();
            services.AddScoped<NFeParser>();

            return services;
        }
    }
}
