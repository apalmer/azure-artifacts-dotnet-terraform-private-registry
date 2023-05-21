using GlintStream.TerraformModuleRegistry.Models;
using System.Net.Http.Headers;

namespace GlintStream.TerraformModuleRegistry.Processors
{
    internal class Processor
    {
        static HttpClient _client = new HttpClient();

        public Processor(HttpClient client)
        {
            _client = client;
        }

        internal async Task<ServiceDiscovery> GetServiceDiscovery()
        {
            var results = new ServiceDiscovery();

            return await Task.FromResult(results);

        }

        internal async Task<ModuleVersions> GetModuleVersions(string name_space, string name, string system)
        {
            var results = new ModuleVersions();

            try
            {
                var urlPath = "/_apis/packaging/Feeds?api-version=7.0";

                using (HttpResponseMessage response = await _client.GetAsync(urlPath))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(responseBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return await Task.FromResult(results);
        }

        internal async Task<DownloadLink> GetDownloadLink(string name_space, string name, string system, string version)
        {
            var results = new DownloadLink() { Link = $"/{name_space}/{name}/{system}/{version}/download-file" };

            try
            {
                var urlPath = "/_apis/packaging/Feeds/Terraform/packages?api-version=7.0";

                using (HttpResponseMessage response = await _client.GetAsync(urlPath))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(responseBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return await Task.FromResult(results);
        }

        internal async Task<DownloadFile> GetDownloadFile(string name_space, string name, string system, string version)
        {
            var results = new DownloadFile();

            try
            {
                var urlPath = "/_apis/packaging/Feeds/Terraform/packages?api-version=7.0";

                using (HttpResponseMessage response = await _client.GetAsync(urlPath))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(responseBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return await Task.FromResult(results);
        }
    }
}