namespace Apim.Models
{
    public class CollectionResponse<T>
    {
        public List<T>? value { get; set; }
        public int count { get; set; }
        public string? nextLink { get; set; }
    }
}
