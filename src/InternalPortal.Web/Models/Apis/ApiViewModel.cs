namespace InternalPortal.Web.Models.Apis
{
    public class ApiViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Version { get; set; }
        public string Description { get; set; }

        public ApiViewModel(string id, string name, string description, string? version = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Version = version;
        }
    }
}
