using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
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
using Scrambler.Api.AutoMapper;
using Scrambler.Quartz;
using Scrambler.Quartz.ConfigProviders;
using Scrambler.Quartz.Configuration;
using Scrambler.Quartz.Interfaces;
using Serilog;
using Serilog.Sinks.MSSqlServer;

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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton(CreateMapper());

            services.AddSingleton(x => CreateSchedulerConfig());

            services.AddSingleton(x => CreateLoggerConfig());

            services.AddSingleton<ISchedulingService, SchedulingService>();
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

            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
            app.UseCors();
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

        public IMapper CreateMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            return mapper;
        }

        public SchedulerConfiguration CreateSchedulerConfig()
        {
            var schedulerConfigPath = Configuration.GetValue<string>("SchedulerConfigPath");
            var schedulerConfig = FileSchedulerConfigurationProvider.GetSchedulerConfig(schedulerConfigPath);

            return schedulerConfig;
        }

        public LoggerConfiguration CreateLoggerConfig()
        {
            var loggerConfig = new LoggerConfiguration()
               .ReadFrom.Configuration(Configuration);

            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

            return loggerConfig;
        }
    }
}
