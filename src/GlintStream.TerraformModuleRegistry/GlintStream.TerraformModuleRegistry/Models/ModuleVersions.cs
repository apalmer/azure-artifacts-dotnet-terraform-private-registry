namespace GlintStream.TerraformModuleRegistry.Models
{
    public class VersionInfo
    {
        [System.Text.Json.Serialization.JsonPropertyName("version")]
        public String Version
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
            var module = new VersionSet();
            module.Versions.Add(new VersionInfo() { Version = "1.0.0" });
            module.Versions.Add(new VersionInfo() { Version = "1.1.0" });
            module.Versions.Add(new VersionInfo() { Version = "2.0.0" });

            Modules.Add(module);
        }

        [System.Text.Json.Serialization.JsonPropertyName("modules")]
        public List<VersionSet> Modules { get; set; }
    }
}
