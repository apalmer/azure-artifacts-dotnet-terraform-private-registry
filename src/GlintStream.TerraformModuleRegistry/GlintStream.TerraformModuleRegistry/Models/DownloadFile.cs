namespace GlintStream.TerraformModuleRegistry.Models
{
    public class DownloadFile
    {
        public string FilePath { get; set; }
        public string FileName { get { return Path.GetFileName(FilePath); } }


    }
}
