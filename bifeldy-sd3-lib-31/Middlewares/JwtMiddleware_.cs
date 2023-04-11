using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using bifeldy_sd3_lib_31.Models;
using bifeldy_sd3_lib_31.Services;

using Newtonsoft.Json;

namespace bifeldy_sd3_lib_31.Middlewares {

    public sealed class JwtMiddleware {

        private readonly RequestDelegate _next;
        private readonly Env _env;
        private readonly IUserService _us;

        public JwtMiddleware(RequestDelegate next, IOptions<Env> env, IUserService us) {
            _next = next;
            _env = env.Value;
            _us = us;
        }

        public async Task Invoke(HttpContext context) {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            StreamReader reader = new StreamReader(request.Body);
            RequestJson reqBody = JsonConvert.DeserializeObject<RequestJson>(await reader.ReadToEndAsync());

            string jwt = string.Empty;
            if (!string.IsNullOrEmpty(request.Cookies[_env.JWT_NAME])) {
                jwt = request.Cookies[_env.JWT_NAME];
            }
            else if (!string.IsNullOrEmpty(request.Headers["authorization"])) {
                jwt = request.Headers["authorization"].FirstOrDefault()?.Split(" ").Last();
            }
            else if (!string.IsNullOrEmpty(request.Headers["x-access-token"])) {
                jwt = request.Headers["x-access-token"];
            }
            else if (!string.IsNullOrEmpty(reqBody?.token)) {
                jwt = reqBody.token;
            }
            else if (!string.IsNullOrEmpty(request.Query["token"])) {
                jwt = request.Query["token"];
            }

            context.Items["user"] = null;

            if (string.IsNullOrEmpty(jwt)) {
                await _next(context);
            }
            else {
                try {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    byte[] key = Encoding.ASCII.GetBytes(_env.KUNCI);

                    tokenHandler.ValidateToken(jwt, new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    JwtSecurityToken jwtToken = (JwtSecurityToken) validatedToken;
                    var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                    context.Items["user"] = await _us.GetById(userId);
                    await _next(context);
                }
                catch {
                    response.Clear();
                    response.StatusCode = 401;
                    response.ContentType = "application/json";

                    object resBody = new {
                        info = "🙄 401 - JWT :: Tidak Dapat Digunakan 😪",
                        result = new {
                            message = "💩 Token Expired / Format Salah! 🤬",
                            token = jwt
                        }
                    };

                    await response.WriteAsync(JsonConvert.SerializeObject(resBody));
                }
            }
        }

    }

}
