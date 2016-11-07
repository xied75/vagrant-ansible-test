using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace aspnetcoreapp
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(await GetEarthquake());
            });
        }

        private async Task<string> GetEarthquake()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Hello World from ASP.NET Core!");
            sb.AppendFormat("(With love from {0})\n\n", System.Environment.MachineName);

            sb.AppendLine("Here are the earth quakes above M 4 in the last day:\n");

            var root = await UsgsService.MakeRequestAsync();
            foreach (var e in root.features)
                sb.AppendFormat("{0} {1}\n", e.properties.title, e.properties.url);

            return sb.ToString();
        }
    }
}
