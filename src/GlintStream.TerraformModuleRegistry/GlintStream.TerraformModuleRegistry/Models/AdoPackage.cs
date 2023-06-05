namespace GlintStream.TerraformModuleRegistry.Models
{
    public class AdoPackageVersion
    {
        [System.Text.Json.Serialization.JsonPropertyName("version")]
        public string Version { get; set; }
    }

    public class AdoPackageValue
    {
        [System.Text.Json.Serialization.JsonPropertyName("versions")]
        public IEnumerable<AdoPackageVersion>  Versions { get; set; }
    }

    public class AdoPackage
    {
        [System.Text.Json.Serialization.JsonPropertyName("value")]
        public IEnumerable<AdoPackageValue> Value { get; set; }
    }
}
