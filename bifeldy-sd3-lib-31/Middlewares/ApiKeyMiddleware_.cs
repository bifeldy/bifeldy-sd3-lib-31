﻿using System.IO;
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
        private readonly IApiKeyService _aks;

        public ApiKeyMiddleware(RequestDelegate next, IOptions<Env> env, ILogger logger, IApiKeyService aks) {
            _next = next;
            _env = env.Value;
            _logger = logger;
            _aks = aks;
        }

        public async Task Invoke(HttpContext context) {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

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

            string ipOrigin = request.Host.Host;
            if (!string.IsNullOrEmpty(request.Headers["origin"])) {
                ipOrigin = request.Headers["origin"];
            }
            else if (!string.IsNullOrEmpty(request.Headers["cf-connecting-ip"])) {
                ipOrigin = request.Headers["cf-connecting-ip"];
            }
            else if (!string.IsNullOrEmpty(request.Headers["referer"])) {
                ipOrigin = request.Headers["referer"];
            }

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