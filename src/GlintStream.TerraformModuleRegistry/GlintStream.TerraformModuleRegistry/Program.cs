
using GlintStream.TerraformModuleRegistry.Models;
using GlintStream.TerraformModuleRegistry.Processors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Xml.Linq;

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

            var feedBaseUrl = builder.Configuration["AzureDevOps:FeedsBaseUrl"];
            var organization = builder.Configuration["AzureDevOps:Organization"];
            var project = builder.Configuration["AzureDevOps:Project"];
            var personalAccessToken = builder.Configuration["AzureDevOps:PersonalAccessToken"];

            var baseAddress = $"{feedBaseUrl}/{organization}/{project}/";
            if (baseAddress != null)
            {
                _client.BaseAddress = new Uri(baseAddress);
            }

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var authPayload = $":{personalAccessToken}";
            var authBytes = System.Text.Encoding.Unicode.GetBytes(authPayload);
            var authBase64 = Convert.ToBase64String(authBytes);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authBase64);

            var adoRestApiHelper = new AdoRestApiHelper(_client, builder.Configuration);

            var azCliHelper = new AzCliHelper(builder.Configuration);

            app.MapGet("/.well-known/terraform.json", (HttpContext httpContext) =>
            {
                return new ServiceDiscovery();
            })
            .WithName("GetServiceDiscovery")
            .WithOpenApi();

            app.MapGet("/{name_space}/{name}/{system}/versions", async (string name_space, string name, string system, HttpContext httpContext) =>
            {
                try
                {
                    var results = await adoRestApiHelper.GetModuleVersions(name_space, name, system);
                    return Results.Ok(results);
                }
                catch (ArgumentException ae)
                {
                    return Results.NotFound();
                }
            })
            .WithName("GetModuleVersions")
            .WithOpenApi();

            app.MapGet("/{name_space}/{name}/{system}/{version}/download", async (string name_space, string name, string system, string version, HttpContext httpContext, LinkGenerator linker) =>
            {
                var downloadFileUrl = linker.GetPathByName("GetDownloadArchive", values: new { name_space = name_space, name = name, system = system, version = version });
                httpContext.Response.Headers.Add("X-Terraform-Get", downloadFileUrl);
                return Results.Ok();

            })
            .WithName("GetDownloadLink")
            .WithOpenApi();

            //The route pattern ending in the extension .zip is important, otherwise terraform wont treat the downloaded file as a zip archive.
            app.MapGet("/{name_space}/{name}/{system}/{version}/download.zip", async (string name_space, string name, string system, string version, HttpContext httpContext) =>
            {
                var downloadFile = await azCliHelper.DownloadPackage(name_space, name, system, version);
                var mimeType = "application/zip";
                return Results.File(downloadFile.FilePath, mimeType, downloadFile.FileName);
            })
            .WithName("GetDownloadArchive")
            .WithOpenApi();

            app.Run();
        }
    }
}