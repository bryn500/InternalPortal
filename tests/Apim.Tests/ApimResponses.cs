namespace Apim.Tests
{
    public static class ApimResponses
    {
        public const string AuthResponse = "{\"id\": \"user id\"}";

        public const string ApisResponse = @"{
            ""value"": [
                {
                    ""id"": ""api-id"",
                    ""type"": ""Microsoft.ApiManagement/service/apis"",
                    ""name"": ""api-name"",
                    ""properties"": {
                        ""displayName"": ""api display name"",
                        ""apiRevision"": ""1"",
                        ""description"": ""api description"",
                        ""subscriptionRequired"": true,
                        ""serviceUrl"": ""api url"",
                        ""path"": ""api"",
                        ""protocols"": [
                            ""https""
                        ],
                        ""authenticationSettings"": {
                            ""oAuth2"": null,
                            ""openid"": null
                        },
                        ""subscriptionKeyParameterNames"": {
                            ""header"": ""Ocp-Apim-Subscription-Key"",
                            ""query"": ""subscription_key""
                        },
                        ""isCurrent"": true,
                        ""apiVersion"": ""v1"",
                        ""apiVersionSetId"": ""version-set-id"",
                        ""apiVersionSet"": {
                            ""id"": ""version-set-id"",
                            ""name"": ""version set name"",
                            ""description"": ""version set description"",
                            ""versioningScheme"": ""Segment"",
                            ""versionQueryName"": null,
                            ""versionHeaderName"": null
                        }
                    }
                },
                {
                    ""id"": ""api-id"",
                    ""type"": ""Microsoft.ApiManagement/service/apis"",
                    ""name"": ""api-name"",
                    ""properties"": {
                        ""displayName"": ""api display name"",
                        ""apiRevision"": ""1"",
                        ""description"": ""api description"",
                        ""subscriptionRequired"": true,
                        ""serviceUrl"": ""api url"",
                        ""path"": ""api"",
                        ""protocols"": [
                            ""https""
                        ],
                        ""authenticationSettings"": {
                            ""oAuth2"": null,
                            ""openid"": null
                        },
                        ""subscriptionKeyParameterNames"": {
                            ""header"": ""Ocp-Apim-Subscription-Key"",
                            ""query"": ""subscription_key""
                        },
                        ""isCurrent"": true,
                        ""apiVersion"": ""v1""
                    }
                }
            ],
            ""count"": 100,
            ""nextLink"": ""next link""            
        }";

        public const string ApiResponse = @"{
            ""id"": ""api-id"",
            ""type"": ""Microsoft.ApiManagement/service/apis"",
            ""name"": ""api-name"",
            ""properties"": {
                ""displayName"": ""api display name"",
                ""apiRevision"": ""1"",
                ""apiVersion"": ""v1"",
                ""description"": ""api description"",
                ""subscriptionRequired"": true,
                ""serviceUrl"": ""api-url"",
                ""path"": ""api"",
                ""protocols"": [
                    ""https""
                ],
                ""authenticationSettings"": {
                    ""oAuth2"": null,
                    ""openid"": null
                },
                ""subscriptionKeyParameterNames"": {
                    ""header"": ""Ocp-Apim-Subscription-Key"",
                    ""query"": ""subscription-key""
                },
                ""isCurrent"": true
            }
        }";

        public const string OperationsResponse = @"{
            ""value"": [
                {
                    ""id"": ""op-id"",
                    ""type"": ""Microsoft.ApiManagement/service/apis/operations"",
                    ""name"": ""op-name"",
                    ""properties"": {
                        ""displayName"": ""op display name"",
                        ""method"": ""GET"",
                        ""urlTemplate"": ""/op"",
                        ""templateParameters"": [],
                        ""description"": ""op description"",
                        ""request"": {
                            ""queryParameters"": [
                                {
                                    ""name"": ""test"",
                                    ""description"": ""The number"",
                                    ""type"": ""integer"",
                                    ""values"": []
                                },
                                {
                                    ""name"": ""test-string"",
                                    ""description"": ""The string"",
                                    ""type"": ""string"",
                                    ""values"": []
                                }
                            ],
                            ""headers"": [],
                            ""representations"": []
                        },
                        ""responses"": [
                            {
                                ""statusCode"": 200,
                                ""description"": ""op response description"",
                                ""representations"": [
                                    {
                                        ""contentType"": ""application/json"",
                                        ""examples"": {
                                            ""default"": {
                                                ""value"": [
                                                    {
                                                        ""id"": 123,
                                                        ""nested"": {
                                                            ""Code"": 1
                                                        }
                                                    }
                                                ]
                                            }
                                        },
                                        ""schemaId"": ""1234"",
                                        ""typeName"": ""op-response"",
                                        ""generatedSample"": ""[\r\n  { \""id\"": 123,\r\n \""nested\"": {\r\n \""Code\"": 1 \r\n} }\r\n]"",
                                        ""sample"": ""[\r\n  { \""id\"": 123,\r\n \""nested\"": {\r\n \""Code\"": 1 \r\n} }\r\n]""
                                    }
                                ],
                                ""headers"": []
                            }
                        ],
                        ""policies"": null
                    }
                },
                {

                }
            ],
            ""count"": 15,
            ""nextLink"": ""next-link""
        }";

        public const string UserResponse = @"{
                ""id"": ""###"",
                ""type"": ""Microsoft.ApiManagement/service/users"",
                ""name"": ""users id"",
                ""properties"": {
                    ""firstName"": ""users first name"",
                    ""lastName"": ""users last name"",
                    ""email"": ""users email"",
                    ""state"": ""active"",
                    ""registrationDate"": ""2020-06-12T10:02:56.327Z"",
                    ""note"": null,
                    ""identities"": [
                        {
                        ""provider"": ""Basic"",
                            ""id"": ""users email""
                        }
                    ]
                }
            }";

        public const string GroupsResponse = @"{
            ""value"": [
                {
                    ""id"": ""###/groups/groupname"",
                    ""type"": ""Microsoft.ApiManagement/service/users/groups"",
                    ""name"": ""groupname"",
                    ""properties"": {
                        ""displayName"": ""group display name"",
                        ""description"": ""group description"",
                        ""builtIn"": true,
                        ""type"": ""group type"",
                        ""externalId"": null
                    }
                }
            ],
            ""count"": 1,
            ""nextLink"": ""next page url""
        }";
    }
}
