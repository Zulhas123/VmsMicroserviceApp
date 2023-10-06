using Base.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Base.API.Controllers
{
    public class AuthenticateController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public AuthenticateController(IConfiguration configuration)
        {

            _configuration = configuration;
        }
        [HttpPost]
        public string CreateToken(LoginModel login)
        {
            //var token = BasicToken();
            //return token;
            var token = GetToken(login);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        private JwtSecurityToken GetToken(LoginModel login)
        {
            var section = _configuration.GetSection("JWT");
            var secret = section.GetSection("Secret").Value;
            var validIssuer = section.GetSection("ValidIssuer").Value;
            var validAudience = section.GetSection("ValidAudience").Value;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, login.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, login.RoleId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var token = new JwtSecurityToken(
                issuer: validIssuer,
                audience: validAudience,
                expires: DateTime.Now.AddHours(8),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        //private string  BasicToken()
        //{
        //    string userName = "ServiceToServiceApiAuthToken";
        //    string password = "iOjMzMjEwMzgyODU1LCJpc3MiOiJodHRwOi8vbG9";
        //    var credentials = userName + ":" + password;
        //    var encoding = Encoding.GetEncoding("iso-8859-1");
        //    var token= Convert.ToBase64String(encoding.GetBytes(credentials));

        //    return token;
            

        //}
    }
}
