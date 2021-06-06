using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PlutoNetCoreTemplate.Job.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Application;
    using Domain;
    using HostedService;
    using Infrastructure;
    using Infrastructure.Listenings;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.Extensions.Configuration;
    using Models;
    using PlutoNetCoreTemplate.Infrastructure;
    using PlutoNetCoreTemplate.Infrastructure.ConnectionString;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Simpl;
    using Quartz.Spi;

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddHttpContextAccessor()
                .AddApplicationLayer()
                .AddDomainLayer()
                .AddInfrastructureLayer(Configuration);

            services.Configure<TenantStoreOptions>(Configuration);

            services.AddSingleton<IJobListener, CustomJobListener>();
            services.AddSingleton<ITriggerListener, CustomTriggerListener>();
            services.AddTransient<IJobInfoStore,InMemoryJobStore>();
            services.AddTransient<IJobLogStore,InMemoryJobLog>();
            services.AddTransient<QuartzJobRunner>();
            AddJobs(services);
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddTransient<IJobStore,RAMJobStore>();
            services.AddQuartzServer(options =>
            {
                options.WaitForJobsToComplete = true;
            });
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddHostedService<QuartzHostedService>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var store = app.ApplicationServices.GetService<IJobInfoStore>();
            var jobs = Configuration.GetSection("JobSettings").Get<List<JobSetting>>();
            if (jobs!=null)
            {
                foreach (var job in jobs)
                {
                    store?.AddAsync(new JobInfoModel
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        TaskType = EnumTaskType.StaticExecute,
                        TaskName = job.Name,
                        DisplayName = job.DisplayName,
                        GroupName = job.GroupName,
                        Interval = job.Cron,
                        Describe = job.Description,
                        Status = EnumJobStates.Normal
                    });
                }
            }
           
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }


        private static void AddJobs(IServiceCollection services)
        {
            var jobDefined = new Dictionary<string,Type>();
            var assembly = Assembly.GetExecutingAssembly();
            var baceType = typeof(IBackgroundJob);
            var implTypes = assembly?.GetTypes().Where(c => c!=baceType&&baceType.IsAssignableFrom(c)).ToList();
            if (implTypes == null)
            {
                return;
            }
            foreach (var impltype in implTypes)
            {
                jobDefined.Add(impltype.Name,impltype);
                services.AddTransient(impltype);
            }
            services.AddSingleton<JobDefined>(s=>new JobDefined
            {
                JobDictionary = jobDefined
            });
        }
    }
}
