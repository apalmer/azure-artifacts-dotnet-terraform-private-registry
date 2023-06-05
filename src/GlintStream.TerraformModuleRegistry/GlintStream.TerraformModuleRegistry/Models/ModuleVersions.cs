namespace GlintStream.TerraformModuleRegistry.Models
{
    public class VersionInfo
    {
        [System.Text.Json.Serialization.JsonPropertyName("version")]
        public String? Version
        {
            get;set;
        }
    }

    public class VersionSet
    {
        public VersionSet()
        {
            Versions = new List<VersionInfo>();
        }

        [System.Text.Json.Serialization.JsonPropertyName("versions")]
        public List<VersionInfo> Versions { get; set; }
    }

    public class ModuleVersions
    {
        public ModuleVersions()
        {
            Modules = new  List<VersionSet>();
        }

        [System.Text.Json.Serialization.JsonPropertyName("modules")]
        public List<VersionSet> Modules { get; set; }
    }
}
