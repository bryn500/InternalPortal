namespace InternalPortal.Web.Models.Apis
{
    public class Api
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Version { get; set; }
        public string Description { get; set; }

        public Api(string id, string name, string description, string? version = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Version = version;
        }
    }
}
