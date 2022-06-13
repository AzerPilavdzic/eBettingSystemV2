using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBettingSystemV2.APIVersionHelper
{
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var op = new OpenApiPaths();
            foreach (var (emptyKey, value) in swaggerDoc.Paths)
            {
                var completeKey = emptyKey.Replace("v{version}", swaggerDoc.Info.Version);
                op.Add(completeKey, value);
            }
            swaggerDoc.Paths = op;
        }
    }
}
