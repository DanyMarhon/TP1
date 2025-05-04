using EFIntro.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TP1Datos;
using TP1Datos.Interfaces;
using TP1Servicios.Interfaces;

namespace Ioc
{
    public static class DI
    {
        public static IServiceProvider ConfigureDI()
        {
            var services = new ServiceCollection();
            var connectionString = @"Data Source=.; Initial Catalog=LibraryDb; Trusted_Connection=true; TrustServerCertificate=true;";

            services.AddDbContext<MarketContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            services.AddScoped<IClienteServicio, IClienteServicio>();

            return services.BuildServiceProvider();


        }
    }
}
