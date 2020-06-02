
using Newtonsoft.Json;

[JsonObject]
public class S2TResponse
{

    [JsonProperty] public S2TResult[] Results { get; set; }
    
}

[JsonObject]
public class S2TResult
{
    [JsonProperty] public S2TAlternatives[] Alternatives { get; set; }
    [JsonProperty] public string LanguageCode { get; set; }
}

[JsonObject]
public class S2TAlternatives
{
 [JsonProperty] public string Transcript { get; set; }  
 [JsonProperty] public string Confidence { get; set; }
}


// "results": [
// {
//     "alternatives": [
//     {
//         "transcript": "book a room",
//         "confidence": 0.9049122
//     }
//     ],
//     "languageCode": "en-us"
// }
// ]
