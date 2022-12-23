namespace Apim.Models
{
    public class OperationResponse
    {
        public string? displayName { get; set; }
        public string? method { get; set; }
        public string? urlTemplate { get; set; }
        public string? description { get; set; }
        public List<TemplateParameters>? templateParameters { get; set; }
        public Request? request { get; set; }
        public List<Response>? responses { get; set; }
        //public List<object>? policies { get; set; }
    }

    public class TemplateParameters
    { 
        public string? name { get; set;}
        public string? description { get; set;}
        public string? type { get; set;}
        public bool required { get; set;}
    }

    public class Request
    {
        public List<Header>? headers { get; set; }
        public List<object>? queryParameters { get; set; }        
        public List<Representations>? representations { get; set; }
    }

    public class Response
    {
        public int statusCode { get; set; }
        public string? description { get; set; }
        public List<Header>? headers { get; set; }
        public List<Representations>? representations { get; set; }
    }

    public class Header
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public string? type { get; set; }
        public bool required { get; set; }
        //public List<object>? values { get; set; }
    }

    public class Representations
    {
        public string? contentType { get; set;}
        public string? generatedSample { get; set;}
    }
}
