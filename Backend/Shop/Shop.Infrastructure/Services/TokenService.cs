﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shop.Domain.Domain;
using Shop.Infrastructure.ConfigurationModels;
using Shop.Infrastructure.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Shop.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<JWTConfigurationModel> _tokenConfiguration;
        private readonly UserManager<User> _userManager;

        public TokenService(IOptions<JWTConfigurationModel> tokenConfiguration, UserManager<User> userManager)
        {
            _tokenConfiguration = tokenConfiguration;
            _userManager = userManager;
        }

        public JwtSecurityToken GenerateTokenSettings(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenSettings = new JwtSecurityToken(
                issuer: _tokenConfiguration.Value.ValidIssuer,
                audience: _tokenConfiguration.Value.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1).AddMinutes(Convert.ToDouble(_tokenConfiguration.Value.ExpiryInMinutes)),
                signingCredentials: signingCredentials
                );
            return tokenSettings;
        }

        public async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_tokenConfiguration.Value.SecurityKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var r = RandomNumberGenerator.Create())
            {
                r.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_tokenConfiguration.Value.SecurityKey)),
                ValidateLifetime = false,
                ValidIssuer = _tokenConfiguration.Value.ValidIssuer,
                ValidAudience = _tokenConfiguration.Value.ValidAudience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = null;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Error - Invalid Token");
            }
            return principal;
        }
    }
}