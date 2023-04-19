/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Mail         :: bias@indomaret.co.id
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Bifeldy's Initial Main Application
 * 
 */

using System;
using System.Collections.Generic;

using Helmet.Net.DontSniffMimetype;
using Helmet.Net.FrameGuard;
using Helmet.Net.HidePoweredByHeader;
using Helmet.Net.IeNoOpen;
using Helmet.Net.NoCache;
using Helmet.Net.XssFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using bifeldy_sd3_lib_31.Databases;
using bifeldy_sd3_lib_31.Utilities;
using bifeldy_sd3_lib_31.Middlewares;
using bifeldy_sd3_lib_31.Services;

namespace bifeldy_sd3_lib_31 {

    public static class Bifeldy {

        public static IDictionary<string, dynamic> AppConfig = null;

        public static IServiceCollection Services = null;

        public static IApplicationBuilder App = null;

        /* ** */

        public static void InitServices(IServiceCollection services) {
            Services = services;
        }

        public static void InitApp(IApplicationBuilder app) {
            App = app;
        }

        /* ** */

        public static void AddSwagger(string apiUrlPrefix, string docsTitle = null, string docsDescription = null) {
            Services.AddSwaggerGen(c => {
                c.SwaggerDoc(apiUrlPrefix ?? "api", new OpenApiInfo {
                    Title = docsTitle ?? "API Documentation",
                    Description = docsDescription ?? "// No Description"
                });
                OpenApiSecurityScheme apiKey = new OpenApiSecurityScheme {
                    Description = @"API-Key Origin. Example: 'http://.../...?key=000...'",
                    Name = "key",
                    In = ParameterLocation.Query,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKey",
                    Reference = new OpenApiReference {
                        Id = "api_key",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(apiKey.Reference.Id, apiKey);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { apiKey, Array.Empty<string>() }
                });
                OpenApiSecurityScheme jwt = new OpenApiSecurityScheme {
                    Description = @"JWT Information. Example: 'Bearer eyj...'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference {
                        Id = "jwt",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(jwt.Reference.Id, jwt);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { jwt, Array.Empty<string>() }
                });
            });
        }

        public static void AddDependencyInjection() {
            Services.AddSingleton<IStream, CStream>();
            Services.AddSingleton<IApplication, CApplication>();
            Services.AddSingleton<ILogger, CLogger>();
            Services.AddSingleton<IConverter, CConverter>();
            Services.AddSingleton<IApi, CApi>();
            Services.AddSingleton<IBerkas, CBerkas>();
            Services.AddSingleton<IFtp, CFtp>();
            Services.AddSingleton<ISftp, CSftp>();
            Services.AddSingleton<IOracle, COracle>();
            Services.AddSingleton<IMsSQL, CMsSQL>();
            Services.AddSingleton<IPostgres, CPostgres>();
            // --
            Services.AddSingleton<IApiKeyService, CApiKeyService>();
            Services.AddSingleton<IUserService, CUserService>();
            Services.AddSingleton<ICryptoService, CCryptoService>();
        }

        /* ** */

        public static void UseSwagger(string apiUrlPrefix) {
            App.UseSwagger(c => {
                c.RouteTemplate = "{documentName}/swagger.json"
                c.PreSerializeFilters.Add((swaggerDoc, request) => {
                    List<OpenApiServer> openApiServers = new List<OpenApiServer>() {
                        new OpenApiServer() {
                            Description = "Direct IP Server",
                            Url = "/"
                        }
                    };
                    string proxyPath = request.Headers["X-Forwarded-Prefix"];
                    if (!string.IsNullOrEmpty(proxyPath)) {
                        openApiServers.Add(new OpenApiServer() {
                            Description = "Reverse Proxy Path",
                            Url = proxyPath.StartsWith("/") || proxyPath.StartsWith("http") ? proxyPath : $"/{proxyPath}"
                        });
                    }
                    swaggerDoc.Servers = openApiServers;
                });
            });
            App.UseSwaggerUI(c => {
                c.RoutePrefix = apiUrlPrefix ?? "api";
                c.SwaggerEndpoint("swagger.json", apiUrlPrefix ?? "api");
            });
        }

        /* ** */

        public static void UseHelmetMiddleware() {
            // App.UseMiddleware<DontSniffMimetypeMiddleware>();
            App.UseMiddleware<FrameGuardMiddleware>("SAMEORIGIN");
            App.UseMiddleware<HidePoweredByHeaderMiddleware>();
            App.UseMiddleware<IeNoOpenMiddleware>();
            App.UseMiddleware<NoCacheMiddleware>();
            App.UseMiddleware<XssFilterMiddleware>();
        }

        public static void UseApiKeyMiddleware() {
            App.UseMiddleware<ApiKeyMiddleware>();
        }

        public static void UseJwtMiddleware() {
            App.UseMiddleware<JwtMiddleware>();
        }

        public static void UseCacheMiddleware() {
            // App.UseMiddleware<CacheMiddleware>();
        }

        public static void UseRequestRateLimiterMiddleware(int maxReqPerMin = 15) {
            // App.UseMiddleware<RateLimiterMiddleware>();
        }

    }

}
