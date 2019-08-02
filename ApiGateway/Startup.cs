using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Startup
    {

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(cors =>
            {
                cors.AddPolicy("CorsPolicy", opts =>
                {
                    opts.AllowAnyHeader();
                    opts.AllowAnyMethod();
                    opts.AllowAnyOrigin();
                    opts.SetIsOriginAllowed(origin => true);
                });
            });

            var identityUrl = _configuration.GetValue<string>("IdentityUrl");
            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer("IdentityApiKey", x =>
                {
                    x.Authority = identityUrl;
                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidAudiences = new[] {
                            "emailing",
                            "medias",
                            "surveys",
                            "users"
                        }
                    };
                });

            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseHttpMethodOverride();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseWebSockets();

            // k8s ingress use this endpoint to determine whether service is ready or not
            app.Map("/health", b =>
            {
                b.Run(async x =>
                {
                    x.Response.StatusCode = StatusCodes.Status200OK;
                    await x.Response.WriteAsync("{}");
                });
            });

            app.UseOcelot().Wait();
        }
    }
}
