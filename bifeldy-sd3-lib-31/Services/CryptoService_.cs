﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using bifeldy_sd3_lib_31.Models;

namespace bifeldy_sd3_lib_31.Services {

    public interface ICryptoService { }

    public sealed class CCryptoService : ICryptoService {

        private readonly Env _env;

        public CCryptoService(IOptions<Env> env) {
            _env = env.Value;
        }

        private string GenerateJwt(dynamic user, bool rememberMe = false) {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_env.JWT_SECRET);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = rememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }

}
