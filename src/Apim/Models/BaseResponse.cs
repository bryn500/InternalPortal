namespace Apim.Models
{
    public class ApimResponse<T>
    {
        public string? id { get; set; }
        //public string type { get; set; }
        public string? name { get; set; }
        public T? properties { get; set; }
    }
}
