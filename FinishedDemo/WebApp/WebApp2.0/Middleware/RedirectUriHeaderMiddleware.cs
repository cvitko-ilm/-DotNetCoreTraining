using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics;

namespace WebApp2_0.Middleware
{
    public class RedirectUriHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectUriHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Debug.WriteLine("Start custom middleware");
            var currentUrl = $"{context.Request.Host}{context.Request.Path}";

            if (!string.IsNullOrWhiteSpace(currentUrl)) {
                context.Response.Headers.Add("X-Redirect", currentUrl);
            }

            await this._next(context);

            Debug.WriteLine("Back from end of middleware");
        }
    }

    public static class RedirectUriHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseEncodeUri(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RedirectUriHeaderMiddleware>();
        }
    }
}
