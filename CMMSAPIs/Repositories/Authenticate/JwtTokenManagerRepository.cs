using CMMSAPIs.Helper;
using CMMSAPIs.Models.Authentication;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Authenticate
{
    public class JwtTokenManagerRepository : GenericRepository
    {
        private readonly IConfiguration _configuration;
        public JwtTokenManagerRepository(MYSQLDBHelper sqlDBHelper, IConfiguration configuration) : base(sqlDBHelper)
        {
            _configuration = configuration;
        }

        internal async Task<UserToken> Authenticate(CMUserCrentials userCrentials)
        {
            string myQuery = "SELECT loginId as login_id FROM Users WHERE loginId = '" + userCrentials.user_name + "' AND password = '" + userCrentials.password + "'";
            List<CMUserLogin> _List = await Context.GetData<CMUserLogin>(myQuery).ConfigureAwait(false);
            if (_List.Count == 0)
            {
                return null;
            }
            var user_id  = _List[0].login_id;
            var key      = _configuration.GetValue<string>("JwtConfig:Key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, user_id)
                    }),
                Expires = DateTime.UtcNow.AddMinutes(CMMS.TOKEN_EXPIRATION_TIME),
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserToken user_token = new UserToken(tokenHandler.WriteToken(token));
            return user_token;
        }
    }
}
