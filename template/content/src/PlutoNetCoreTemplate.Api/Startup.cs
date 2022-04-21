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
            #region �����������
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

            // ���ػ�
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // ����ǽ
            services.AddFirewall();

            services.AddCustomHealthCheck(Configuration)
                .AddCustomSwagger()
                .AddCustomCors(DefaultCorsName, Configuration)
                .AddHttpContextAccessor();
            #endregion

            #region �������

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

            #region ��֤��Ȩ
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

            #region ����
            services.AddMemoryCache(options =>
            {
                options.SizeLimit = 10240;
            });
            #endregion

            #region http�����������

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardLimit = null;// ����������ı�ͷ�е���Ŀ��
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; // X-Forwarded-For������������й��ڷ�������Ŀͻ��˺ͺ����������Ϣ��X-Forwarded-Proto��ԭ������ֵ (HTTP/HTTPS)
                options.KnownNetworks.Clear(); // ���н���ת��ͷ����֪����ĵ�ַ��Χ�� ʹ����������·��ѡ�� (CIDR) ��ʾ���ṩ IP ��Χ��ʹ��CDNʱӦ���
                options.KnownProxies.Clear(); // ���н���ת��ͷ����֪����ĵ�ַ�� ʹ�� KnownProxies ָ����ȷ�� IP ��ַƥ�䡣ʹ��CDNʱӦ���
            });
            // ·��Сд
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