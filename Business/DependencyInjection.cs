using AutoMapper;
using Business.Interfaces;
using Business.Services;
using Data.Data;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IStatisticService, StatisticService>();

            return services;
        }
    }
}
