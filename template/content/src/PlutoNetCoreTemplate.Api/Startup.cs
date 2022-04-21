namespace PlutoNetCoreTemplate.Api
{
    using PlutoNetCoreTemplate.Api.SeedData;

#if (Grpc)
    using Application.Grpc;
    using PlutoNetCoreTemplate.Application.Grpc.Services;
#endif


    public class Startup
    {
        private const string DefaultCorsName = "default";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region 基础组件服务
            services.AddControllers(options =>
                {
                    //options.Filters.Add<UnitOfWorkFilter>();
                    options.ModelBinderProviders.Insert(0, new SortingBinderProvider());
                })
                .AddCustomJsonSerializer()
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters()
                .ConfigCustomApiBehaviorOptions()
                .AddDataAnnotationsLocalization();

            // 本地化
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // 防火墙
            services.AddFirewall();

            services.AddCustomHealthCheck(Configuration)
                .AddCustomSwagger()
                .AddCustomCors(DefaultCorsName, Configuration)
                .AddHttpContextAccessor();
            #endregion

            #region 领域组件

            services.AddApplicationLayer()
                .AddDomainLayer()
                .AddTenantComponent(Configuration)
                .AddInfrastructureLayer(Configuration);

            #endregion

            #region GRPC

#if (Grpc)
            services.AddGrpc();
            services.AddSingleton<GrpcCallerService>();
#endif


            #endregion

            #region 认证授权
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "pluto",
                        ValidAudience = "12312",
                        ClockSkew = TimeSpan.FromMinutes(10),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("715B59F3CDB1CF8BC3E7C8F13794CEA9")),
                    };
                });
            services.AddCustomAuthorization();
            #endregion

            #region 缓存
            services.AddMemoryCache(options =>
            {
                options.SizeLimit = 10240;
            });
            #endregion

            #region http请求相关配置

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardLimit = null;// 限制所处理的标头中的条目数
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; // X-Forwarded-For：保存代理链中关于发起请求的客户端和后续代理的信息。X-Forwarded-Proto：原方案的值 (HTTP/HTTPS)
                options.KnownNetworks.Clear(); // 从中接受转接头的已知网络的地址范围。 使用无类别域际路由选择 (CIDR) 表示法提供 IP 范围。使用CDN时应清空
                options.KnownProxies.Clear(); // 从中接受转接头的已知代理的地址。 使用 KnownProxies 指定精确的 IP 地址匹配。使用CDN时应清空
            });
            // 路由小写
            services.AddRouting(options => options.LowercaseUrls = true);
            #endregion

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true;


            app.UseForwardedHeaders()
                .UseCertificateForwarding();
            app.UseCustomLocalization();
            app.UseHttpRequestLogging();


            if (env.IsDevelopment())
            {
                app.DataSeederAsync().Wait();
                app.UseDeveloperExceptionPage();
                app.UseCustomSwagger();
            }
            if (env.IsProduction())
            {
                app.UseExceptionProcess();
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseCors(DefaultCorsName);
            app.UseUnitOfWork();
            app.UseRouting();
            app.UseAuthentication();
            app.UseTenant();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
#if (Grpc)
                endpoints.MapGrpcService<OrderGrpc>();
#endif
                endpoints.MapCustomHealthChecks();
                endpoints.MapControllers();
            });
        }
    }

}