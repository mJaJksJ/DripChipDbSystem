using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Startup.SwaggerSettings
{
    public static class SwaggerSettings
    {
        public static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Paths.AppName,
                    Version = "v1"
                });

                c.IncludeXmlComments(Paths.DocsFile);
                c.SchemaFilter<EnumTypesSchemaFilter>(Paths.DocsFile);
                c.DocumentFilter<EnumTypesDocumentFilter>();
                c.CustomOperationIds(e => (e.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
            });
        }

        public static void UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", Paths.AppName); });
        }
    }
}
