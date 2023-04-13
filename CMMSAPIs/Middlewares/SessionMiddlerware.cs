using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;


namespace CMMSAPIs.Middlewares
{
    public class SessionMiddlerware
    {
        private readonly RequestDelegate _next;
        
        public SessionMiddlerware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var headers = httpContext.Request.Headers;           
            foreach (var hList in headers)
            {
                if (hList.Key == "Authorization") {
            
                    String authorizeStr = hList.Value;
                    char[] spearator = { ' ' };
                    Int32 count = 2;
                    String[] strlist = authorizeStr.Split(spearator, count, StringSplitOptions.None);
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(strlist[1]);
                    var tokenS = jsonToken as JwtSecurityToken;
                    var userId = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
                    
                    httpContext.Session.SetString("_User_Id", userId);
                }                
            }
            await _next(httpContext); // calling next middleware

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionMiddlerware>();
        }
    }
}
