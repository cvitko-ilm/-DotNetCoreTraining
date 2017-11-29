using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Builder;

namespace WebApp2._0.Middleware
{
    public class RedirectUriHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectUriHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var currentUrl = $"{context.Request.Host}{context.Request.Path}";

            if (!string.IsNullOrWhiteSpace(currentUrl)) {
                context.Response.Headers.Add("X-Redirect",currentUrl);
            }

            return this._next(context);
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
