﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using aspcore_async_deploy_smart_contract.AppService;
using aspcore_async_deploy_smart_contract.Dal;
using Microsoft.EntityFrameworkCore;

namespace aspcore_async_deploy_smart_contract.Startup
{
    public class TestConnectionString
    {
        public string ConnectionString { get; }
        public TestConnectionString(string connstr)
        {
            ConnectionString = connstr;
        }
    }

    public class StartupDevelopmentTest
    {
        public StartupDevelopmentTest(IConfiguration configuration, TestConnectionString testConnectionString)
        {
            Configuration = configuration;
            TestConnectionString = testConnectionString;
        }

        private readonly TestConnectionString TestConnectionString;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // For development
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddDbContext<BECDbContext>(
                options => options.UseSqlServer
                        (
                            TestConnectionString.ConnectionString
                        )
                );

            services.AddService();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddApplicationPart(Assembly.Load("aspcore_async_deploy_smart_contract"));

            //foreach (var service in services)
            //{
            //    Console.WriteLine("Loaded {0} with {1}", service.ServiceType.FullName, service.ImplementationType?.FullName);
            //}
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseCors("CorsPolicy");

            //startupQueueUnfinishedReceiptPollingTask.StartAsync(new System.Threading.CancellationToken()).GetAwaiter().GetResult();
        }
    }
}
