namespace Apim.Models
{
    public class ApisResponse
    {
        public List<Value> value { get; set; }
        public int count { get; set; }
        public object nextLink { get; set; }
    }

    public class Value
    {
        public string id { get; set; }
        public string name { get; set; }
        public string apiRevision { get; set; }
        public object description { get; set; }
        public bool subscriptionRequired { get; set; }
        public string serviceUrl { get; set; }
        public string path { get; set; }
        public List<string> protocols { get; set; }
        public AuthenticationSettings authenticationSettings { get; set; }
        public SubscriptionKeyParameterNames subscriptionKeyParameterNames { get; set; }
        public bool isCurrent { get; set; }
    }

    public class AuthenticationSettings
    {
        public object oAuth2 { get; set; }
        public object openid { get; set; }
    }

    public class SubscriptionKeyParameterNames
    {
        public string header { get; set; }
        public string query { get; set; }
    }
}
