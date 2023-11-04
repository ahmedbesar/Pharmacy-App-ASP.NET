using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Pharmacy.Domian.Entities.OrderAggregate;
using Pharmacy.Domian.Interfaces;
using Pharmacy.Infrastructure.Data;
using Pharmacy.Services;

namespace Pharmacy.API.Extentions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();

           services.AddScoped<IBasketRepository, BasketRepository>();
            

            return services;
        }
    }
}

