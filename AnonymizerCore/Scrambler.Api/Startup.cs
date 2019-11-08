﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Scrambler.Quartz;
using Scrambler.Quartz.ConfigProviders;
using Scrambler.Quartz.Interfaces;
using Serilog;

namespace Scrambler.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var schedulerConfigPath = Configuration.GetValue<string>("SchedulerConfigPath");
            var schedulerConfig = FileSchedulerConfigurationProvider.GetSchedulerConfig(schedulerConfigPath);
            
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("serilog.config.json")
                .Build();
                
            var loggerConfig = new LoggerConfiguration()
                  .ReadFrom.Configuration(Configuration);

            services.AddSingleton(x => schedulerConfig);
            services.AddSingleton(x => loggerConfig);

            services.AddSingleton<ISchedulingService,SchedulingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();

            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.ContractResolver = new DefaultContractResolver();
                return settings;
            };
        }
    }
}
