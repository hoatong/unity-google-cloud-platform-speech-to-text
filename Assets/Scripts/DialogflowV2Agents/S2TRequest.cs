using Newtonsoft.Json;


[JsonObject]
public class S2TRequest
{
    [JsonProperty] public S2TConfig Config { get; set; }

    [JsonProperty] public S2TAudio Audio { get; set; }

    public S2TRequest()
    {
       
    }
    public S2TRequest(S2TConfig config, S2TAudio audio)
    {
        Config = config;
        Audio = audio;
    }
}

[JsonObject]
public class S2TAudio
{
    [JsonProperty] public string Content { get; set; }
}

public class S2TConfig
{
    [JsonProperty] public string LanguageCode { get; set; }
}