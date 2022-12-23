namespace Apim.Models
{
    public class ApisResponse<T> : CollectionResponse<T>
    {
        public string? nextName { get; set; }
    }

    public class ApiResponse
    {
        public string? displayName { get; set; }
        //public string apiRevision { get; set; }
        public string? description { get; set; }
        //public bool subscriptionRequired { get; set; }
        //public string serviceUrl { get; set; }
        //public string path { get; set; }
        //public List<string> protocols { get; set; }
        //public bool isCurrent { get; set; }
        public string? apiVersion { get; set; }
        //public string apiVersionSetId { get; set; }
        //public ApiVersionSet apiVersionSet { get; set; }
        //public SubscriptionKeyParameterNames subscriptionKeyParameterNames { get; set; }
        //public AuthenticationSettings authenticationSettings { get; set; }
    }

    //public class AuthenticationSettings
    //{
    //    public object oAuth2 { get; set; }
    //    public object openid { get; set; }
    //}

    //public class SubscriptionKeyParameterNames
    //{
    //    public string header { get; set; }
    //    public string query { get; set; }
    //}

    //public class ApiVersionSet
    //{
    //    public string id { get; set; }
    //    public string name { get; set; }
    //    public object description { get; set; }
    //    public string versioningScheme { get; set; }
    //    public object versionQueryName { get; set; }
    //    public object versionHeaderName { get; set; }
    //}
}
