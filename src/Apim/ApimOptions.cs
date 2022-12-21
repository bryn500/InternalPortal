namespace Apim
{
    public class ApimOptions
    {
        public const string ConfigName = "ManagmentApi";
        public string ManagementApiUrl { get; set; }
        public string ManagementApiVersion { get; set; }
        public string ManagementApiId { get; set; }
        public string ManagementApiPrimaryKey { get; set; }
        public string BackendUrl { get; set; }
        public string ProxyHostnames { get; set; }
    }
}
