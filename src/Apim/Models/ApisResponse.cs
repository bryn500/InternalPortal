namespace Apim.Models
{
    public class ApisResponse
    {
        public List<ApisValue>? value { get; set; }
        public int count { get; set; }
        public string? nextLink { get; set; }
        public string? nextName { get; set; }
    }

    public class ApisValue
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public ApiProperties properties { get; set; }
    }
}
