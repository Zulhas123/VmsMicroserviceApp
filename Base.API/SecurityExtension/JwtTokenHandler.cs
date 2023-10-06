using Base.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

namespace Base.API.SecurityExtension
{
    public class JwtTokenHandler
    {

        public JwtTokenHandler()
        {

        }
        public bool AttachAccountToContext(string token, IConfiguration _configuration, HostString host, PathString path)
        {
            try
            {
                if (token.IsNullOrEmpty())
                {
                    return false;
                }
                var section = _configuration.GetSection("JWT");
                var secret = section.GetSection("Secret").Value;
                var validIssuer = section.GetSection("ValidIssuer").Value;
                var validAudience = section.GetSection("ValidAudience").Value;
                var serviceName = section.GetSection("serviceName").Value;
                //secret = login.Username + secret + login.Password;

                token = token.Substring(7, token.Length - 7);

                var tokenHandler = new JwtSecurityTokenHandler();
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                //var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = authSigningKey,
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var roleId = jwtToken.Claims.First(x => x.Type == "sub").Value;
                var userId = jwtToken.Claims.First(x => x.Type == "nameid").Value;

                UserData.RoleId = Convert.ToInt32(roleId);
                UserData.UserId = Convert.ToInt32(userId);

                return true;

               // return CheckPermission(UserData.RoleId, validIssuer, host, path);
            }
            catch
            {
                return false;
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }

        private bool CheckPermission(int roleId, string issuer, HostString host, PathString path)
        {
            var url = issuer + "/userservice/permission/GetSubMenuByRoleId?roleId=" + roleId;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", "U2VydmljZVRvU2VydmljZUFwaUF1dGhUb2tlbjppT2pNek1qRXdNemd5T0RVMUxDSnBjM01pT2lKb2RIUndPaTh2Ykc5");

            var result = client.GetAsync(url);
            var res = result.Result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<PermissionVm>>(res.Result);
            if (model.FirstOrDefault(c => c.Host == host.Value.ToString() && c.Path == path.ToString()) != null)
            {
                return true;
            }
            return false;

        }

        internal bool CheckBasicAuthentication(string token, IConfiguration _configuration)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }
                token = token.Substring(7, token.Length - 7);
                var encoding = Encoding.GetEncoding("iso-8859-1");
                var credentials = encoding.GetString(Convert.FromBase64String(token));
                int separator = credentials.IndexOf(':');
                string username = credentials.Substring(0, separator);
                string password = credentials.Substring(separator + 1);
                if(username== "ServiceToServiceApiAuthToken" && password=="iOjMzMjEwMzgyODU1LCJpc3MiOiJodHRwOi8vbG9")
                {
                    return true;
                }
                return false;

            }
            catch (Exception)
            {

                return false;
            }

        }

    }
}
