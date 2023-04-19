﻿/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Middleware API Key
 *              :: Harap Didaftarkan Ke DI Container
 * 
 */

using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using bifeldy_sd3_lib_31.Models;
using bifeldy_sd3_lib_31.Services;
using bifeldy_sd3_lib_31.Utilities;

using Newtonsoft.Json;

namespace bifeldy_sd3_lib_31.Middlewares {

    public sealed class ApiKeyMiddleware {

        private readonly RequestDelegate _next;
        private readonly Env _env;
        private readonly ILogger _logger;
        private readonly IGlobalService _gs;
        private readonly IApiKeyService _aks;

        public ApiKeyMiddleware(RequestDelegate next, IOptions<Env> env, ILogger logger, IGlobalService gs, IApiKeyService aks) {
            _next = next;
            _env = env.Value;
            _logger = logger;
            _gs = gs;
            _aks = aks;
        }

        public async Task Invoke(HttpContext context) {
            ConnectionInfo connection = context.Connection;
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            string ipDomainHost = request.Host.Host;
            if (!_gs.allowedIpOrigin.Contains(ipDomainHost)) {
                _gs.allowedIpOrigin.Add(ipDomainHost);
            }
            string ipDomainProxy = request.Headers["X-Forwarded-Host"];
            if (!string.IsNullOrEmpty(ipDomainProxy) && !_gs.allowedIpOrigin.Contains(ipDomainProxy)) {
                _gs.allowedIpOrigin.Add(ipDomainProxy);
            }

            StreamReader reader = new StreamReader(request.Body);
            RequestJson reqBody = JsonConvert.DeserializeObject<RequestJson>(await reader.ReadToEndAsync());

            string apiKey = string.Empty;
            if (!string.IsNullOrEmpty(request.Cookies[_env.API_KEY_NAME])) {
                apiKey = request.Cookies[_env.API_KEY_NAME];
            }
            else if (!string.IsNullOrEmpty(request.Headers["x-api-key"])) {
                apiKey = request.Headers["x-api-key"];
            }
            else if (!string.IsNullOrEmpty(reqBody?.key)) {
                apiKey = reqBody.key;
            }
            else if (!string.IsNullOrEmpty(request.Query["key"])) {
                apiKey = request.Query["key"];
            }

            string ipOrigin = connection.RemoteIpAddress.ToString();
            if (!string.IsNullOrEmpty(request.Headers["origin"])) {
                ipOrigin = request.Headers["origin"];
            }
            else if (!string.IsNullOrEmpty(request.Headers["referer"])) {
                ipOrigin = request.Headers["referer"];
            }
            else if (!string.IsNullOrEmpty(request.Headers["cf-connecting-ip"])) {
                ipOrigin = request.Headers["cf-connecting-ip"];
            }
            else if (!string.IsNullOrEmpty(request.Headers["X-Forwarded-For"])) {
                ipOrigin = request.Headers["X-Forwarded-For"];
            }
            ipOrigin = _gs.CleanIpOrigin(ipOrigin);

            context.Items["api_key"] = apiKey;
            _logger.WriteInfo(GetType().Name, $"[API_KEY_IP_ORIGIN] 🌸 {apiKey} @ {ipOrigin}");

            if (!request.Path.Value.Contains("/api") || await _aks.CheckKeyOrigin(ipOrigin, apiKey)) {
                await _next(context);
            }
            else {
                response.Clear();
                response.StatusCode = 401;
                response.ContentType = "application/json";

                object resBody = new {
                    info = "🙄 401 - API Key :: Tidak Dapat Digunakan 😪",
                    result = new {
                        message = "💩 Api Key Salah / Tidak Terdaftar! 🤬",
                        api_key = apiKey,
                        ip_origin = ipOrigin
                    }
                };

                await response.WriteAsync(JsonConvert.SerializeObject(resBody));
            }
        }

    }

}
