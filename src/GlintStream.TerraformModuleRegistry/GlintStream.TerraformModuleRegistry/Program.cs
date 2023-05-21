
using GlintStream.TerraformModuleRegistry.Processors;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace GlintStream.TerraformModuleRegistry
{
    public class Program
    {
        static HttpClient _client = new HttpClient();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var baseUrl = builder.Configuration["AzureDevOps:BaseUrl"];
            var personalAccessToken = builder.Configuration["AzureDevOps:PersonalAccessToken"];

            if (baseUrl != null)
            {
                _client.BaseAddress = new Uri(baseUrl);
            }

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", personalAccessToken))));

            var proc = new Processor(_client);

            app.MapGet("/.well-known/terraform.json", async (HttpContext httpContext) =>
            {
                return await proc.GetServiceDiscovery();
            })
            .WithName("GetServiceDiscovery")
            .WithOpenApi();

            app.MapGet("/{name_space}/{name}/{system}/versions", async (string name_space, string name, string system, HttpContext httpContext) =>
            {
                return await proc.GetModuleVersions(name_space, name, system);
            })
            .WithName("GetModuleVersions")
            .WithOpenApi();

            app.MapGet("/{name_space}/{name}/{system}/{version}/download", async (string name_space, string name, string system, string version, HttpContext httpContext) =>
            {
                var downloadLink = await proc.GetDownloadLink(name_space, name, system, version);
                httpContext.Response.Headers.Add("X-Terraform-Get", downloadLink.Link);
            })
            .WithName("GetDownloadLink")
            .WithOpenApi();


            app.MapGet("/{name_space}/{name}/{system}/{version}/download-file", async (string name_space, string name, string system, string version, HttpContext httpContext) =>
            {
                
                return await proc.GetDownloadFile(name_space, name, system, version);
            })
            .WithName("GetDownloadFile")
            .WithOpenApi();


            app.Run();
        }
    }
}