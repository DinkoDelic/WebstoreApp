using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entitites.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        // Used to decrypt and encrypt our tokens 
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }

        public string CreateToken(AppUser user)
        {
            // claims are a list of details about the user
            var claims = new List<Claim>
            {
                // Claims are public so a user can see their claims
                // user email claim
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                // user name claim
                new Claim(JwtRegisteredClaimNames.GivenName, user.DisplayName)
            };  

            //parameters: our secure key and hashing algorithm, key gets hashed
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // token options
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // pass in our claims
                Subject = new ClaimsIdentity(claims),
                // expires after 7 days
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["Token:Issuer"]
            };

            // used to create and return our token
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}