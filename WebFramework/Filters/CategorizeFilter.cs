using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace WebFramework.Filters
{
    public class CategorizeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string path = context.ApiDescription.RelativePath;

            var segment = path.Split('/')[3] + path.Split('/')[2];

            if (segment != context.ApiDescription.GroupName)
            {
                operation.Tags = new List<OpenApiTag> { new OpenApiTag { Name = segment } };
            }
        }
    }
}
