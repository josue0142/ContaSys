using System.Configuration;

namespace ContaSys.Auth
{
    /// <summary>
    /// Middleware para autenticación mediante API Key.
    /// </summary>
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration) {

            _next = next;
            _configuration = configuration;


        }

        public async Task InvokeAsync(HttpContext context) {

            if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey)) {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key no encontrada.");
                return;
            }

            var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key Invalida.");
                return;
            }

            await _next(context);
        
        }


    }
}
