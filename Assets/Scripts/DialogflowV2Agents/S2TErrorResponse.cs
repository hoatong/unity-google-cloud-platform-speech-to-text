using Newtonsoft.Json;


[JsonObject]
public class S2TErrorResponse
{
    [JsonProperty] public S2TError Error { get; set; }
}

[JsonObject]
public class S2TError
{
    [JsonProperty] public string Code { get; set; }
    [JsonProperty] public string Message { get; set; }
}