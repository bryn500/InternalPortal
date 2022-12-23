namespace Apim.Models
{
    public class UserIdResponse
    {
        public string? id { get; set; }
    }

    public class UserResponse
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? state { get; set; }
        public DateTime? registrationDate { get; set; }
    }

    public class GroupResponse
    {
        public string? displayName { get; set; }
        public string? description { get; set; }
        public string? type { get; set; }
        public bool builtIn { get; set; }
        public string? externalId { get; set; }
    }
}
