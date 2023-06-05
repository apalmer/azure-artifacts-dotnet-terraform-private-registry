using GlintStream.TerraformModuleRegistry.Models;
using System.Net.Http.Headers;
using System.Reflection;

namespace GlintStream.TerraformModuleRegistry.Processors
{
    internal class AdoRestApiHelper
    {
        static HttpClient _client = new HttpClient();
        IConfiguration _configuration;

        public AdoRestApiHelper(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        internal async Task<ModuleVersions> GetModuleVersions(string name_space, string name, string system)
        {
            var results = new ModuleVersions();

            try
            {
                var urlPath = $"_apis/packaging/feeds/Terraform/Packages?api-version=7.0&packageNameQuery={name_space}-{name}-{system}";

                using (HttpResponseMessage response = await _client.GetAsync(urlPath))
                {
                    response.EnsureSuccessStatusCode();
                    var responseObject = await response.Content.ReadFromJsonAsync<AdoPackage>();
                    if (responseObject?.Value?.Count() > 0)
                    {
                        var responseValue = responseObject.Value.First();
                        var module = new VersionSet();
                        foreach (var adoVersion in responseValue.Versions)
                        {
                            module.Versions.Add(new VersionInfo() { Version = adoVersion.Version });
                        }

                        results.Modules.Add(module);
                    }
                    else
                    {
                        throw new ArgumentException("No Matching Package Found");
                    }
                }
            }
            catch
            {
                throw;
            }

            return results;
        }
    }
}