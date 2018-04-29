using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Data;
using Server.Services;
using NSwag.AspNetCore;
using System.Reflection;
using NJsonSchema;
using NSwag.SwaggerGeneration.Processors.Security;
using NSwag;

namespace Server
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
            services
                .AddSwagger()
                .AddJwtAuthenticationOptions(Configuration["Jwt:Issuer"], Configuration["Jwt:Key"])
                .AddUserDbContext(Configuration.GetConnectionString("UserDatabase"))
                .AddRepositories()
                .AddServerServices()
                .AddAutoMapperProfiles()
                .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
                settings.GeneratorSettings.DocumentProcessors.Add(new SecurityDefinitionAppender("Authorization", new SwaggerSecurityScheme
                {
                    Type = SwaggerSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = SwaggerSecurityApiKeyLocation.Header
                }));
            });
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
