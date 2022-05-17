using Microsoft.OpenApi.Models;

namespace CarConsole.EndpointDefinitions;

public class SwaggerEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarAPI"));
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference
          {
           Id = "Bearer",
           Type = ReferenceType.SecurityScheme
          }
        }, new List<string>()
      }

          });
        });
    }
}
