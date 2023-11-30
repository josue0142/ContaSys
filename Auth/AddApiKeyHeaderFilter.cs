using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace ContaSys.Auth
{
    
    public class AddApiKeyHeaderFilter : IOperationFilter
    {
        private readonly IConfiguration _configuration;

        public AddApiKeyHeaderFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Obtener el valor de la clave de API desde la configuración dentro del filtro
            var apiKeyFromConfig = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = AuthConstants.ApiKeyHeaderName,
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                }
            });
        }
    }

}
