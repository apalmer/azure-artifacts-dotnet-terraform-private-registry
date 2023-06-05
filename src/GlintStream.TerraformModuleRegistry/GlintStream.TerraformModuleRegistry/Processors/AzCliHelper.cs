using GlintStream.TerraformModuleRegistry.Models;
using System.Diagnostics;
using System.IO.Compression;

namespace GlintStream.TerraformModuleRegistry.Processors
{
    public class AzCliHelper
    {
        IConfiguration _configuration;

        public AzCliHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DownloadFile> DownloadPackage(string name_space, string name, string system, string version)
        {
            var results = new DownloadFile();

            string personalAccessToken = _configuration["AzureDevOps:PersonalAccessToken"];
            string baseUrl = _configuration["AzureDevOps:DefaultBaseUrl"]; 
            string organization = _configuration["AzureDevOps:Organization"];
            string project = _configuration["AzureDevOps:Project"];
            string feedId = _configuration["AzureDevOps:ArtifactFeed"];
            string shellProgram = _configuration["Shell:Program"];
            string shellCommandParameter = _configuration["Shell:CommandParameter"];

            string packageName = $"{name_space}-{name}-{system}";
            string packageVersion = version;
            string packageArchiveName = $"{packageName}.zip";

            string tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            string downloadPath = Path.Combine(tempPath, "download");
            string packageArchive = Path.Combine(tempPath, packageArchiveName);
            
            Directory.CreateDirectory(tempPath);
            Directory.CreateDirectory(downloadPath);

            string authenticateCommand = $"$env:AZURE_DEVOPS_EXT_PAT = '{personalAccessToken}';";
            //string authenticateCommand = $"$env:AZURE_DEVOPS_EXT_PAT='x';Get-ChildItem;";
            string downloadCommand = $"az artifacts universal download --organization \"{baseUrl}/{organization}/\" --project=\"{project}\" --scope project --feed \"{feedId}\" --name \"{packageName}\" --version \"{packageVersion}\" --path \"{downloadPath}\";";
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = shellProgram;
            startInfo.Arguments = $"{shellCommandParameter} {authenticateCommand} {downloadCommand}";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;

            var proc = Process.Start(startInfo);
            //string line = String.Empty;
            //while (!proc.StandardOutput.EndOfStream)
            //{
            //    line = proc.StandardOutput.ReadToEnd();
            //}
            await proc.WaitForExitAsync();

            ZipFile.CreateFromDirectory(downloadPath, packageArchive);

            results.FilePath = packageArchive;

            return results;
        }
    }
}
