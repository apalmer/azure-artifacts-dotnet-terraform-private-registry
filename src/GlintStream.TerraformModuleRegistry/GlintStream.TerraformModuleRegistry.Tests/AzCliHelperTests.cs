using System.Diagnostics;
using GlintStream.TerraformModuleRegistry.Processors;
using Microsoft.Extensions.Configuration;

namespace GlintStream.TerraformModuleRegistry.Tests
{
    public class AzCliHelperTests
    {
        IConfiguration _configuration;

        public AzCliHelperTests(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Fact]
        public void DownloadPackageTest()
        {
            var helper = new AzCliHelper(_configuration);
            var fileName = helper.DownloadPackage("wright","orville", "flight", "0.0.2");
        }
    }
}