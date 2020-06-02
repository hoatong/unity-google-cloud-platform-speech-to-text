using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


public class GCPSpeech2TextClient : MonoBehaviour
{
    /// <summary>
    /// The object that defines the service settings.
    /// </summary>
    public ServiceSettings accessSettings = null;

    /// <summary>
    /// Delegate for handling errors received after a detectIntent request.
    /// </summary>
    /// <param name="error">The error response.</param>
    public delegate void DetectIntentErrorHandler(S2TErrorResponse error);

    /// <summary>
    /// Event fired at each error received from DetectIntent.
    /// </summary>
    public event DetectIntentErrorHandler DetectIntentError;

    /// <summary>
    /// Delegate for handling responses from the DF2 server.
    /// </summary>
    /// <param name="response">The received response.</param>
    public delegate void ServerResponseHandler(S2TResponse response);

    /// <summary>
    /// Event fired at each response from the chatbot.
    /// </summary>
    public event ServerResponseHandler ChatbotResponded;

    
    /// <summary>
    /// The default detectIntent URL where project ID and session ID are missing. 
    /// </summary>
    internal static readonly string PARAMETRIC_DETECT_INTENT_URL =
        "https://speech.googleapis.com/v1p1beta1/speech:recognize";


    //@hoatong
    /// <summary>
    /// Makes a POST request to Dialogflow for detecting an intent from text.
    /// </summary>
    /// <param name="talker">The ID of the entity who is talking to the bot.</param>
    /// <param name="languageCode">The language code of the request.</param>
    public void DetectIntentFromAudio(string audio64, string languageCode = "")
    {
        if (languageCode.Length == 0)
            languageCode = accessSettings.LanguageCode;

        StartCoroutine(DetectIntent(audio64));
    }


    /// <summary>
    /// Sends a <see cref="DF2QueryInput"/> object as a HTTP request to the remote
    /// chatbot.
    /// </summary>
    /// <param name="queryInput">The input request.</param>
    /// <param name="session">The session ID, i.e., the ID of the user who talks to the chatbot.</param>
    private IEnumerator DetectIntent(string audio = "")
    {
        // Gets the JWT access token.
        string accessToken = string.Empty;
        while (!JwtCache.TryGetToken(accessSettings.ServiceAccount, out accessToken))
            yield return JwtCache.GetToken(accessSettings.CredentialsFileName,
                accessSettings.ServiceAccount);

        Debug.Log(accessToken);
        // Prepares the HTTP request.
        var settings = new JsonSerializerSettings();
        settings.NullValueHandling = NullValueHandling.Ignore;
        settings.ContractResolver = new CamelCasePropertyNamesContractResolver();


        S2TRequest request = new S2TRequest();

        request.Audio = new S2TAudio();
        request.Audio.Content = audio;

        request.Config = new S2TConfig();
        request.Config.LanguageCode = "en-US";

        string jsonInput = JsonConvert.SerializeObject(request, settings);

        Debug.Log("Json: " + jsonInput);

        byte[] body = Encoding.UTF8.GetBytes(jsonInput);


        UnityWebRequest df2Request = new UnityWebRequest(PARAMETRIC_DETECT_INTENT_URL, "POST");
        df2Request.SetRequestHeader("Authorization", "Bearer " + accessToken);
        df2Request.SetRequestHeader("Content-Type", "application/json");
        df2Request.uploadHandler = new UploadHandlerRaw(body);
        df2Request.downloadHandler = new DownloadHandlerBuffer();

        yield return df2Request.SendWebRequest();

        // Processes response.
        if (df2Request.isNetworkError || df2Request.isHttpError)
            DetectIntentError?.Invoke(JsonConvert.DeserializeObject<S2TErrorResponse>(df2Request.downloadHandler.text));
        else
        {
            string response = Encoding.UTF8.GetString(df2Request.downloadHandler.data);

            S2TResponse resp = JsonConvert.DeserializeObject<S2TResponse>(response);
            ChatbotResponded?.Invoke(resp);
            // if(resp.Results[0] !=null)
            // {
            // 	Debug.Log("transcript: "+resp.Results[0].Alternatives[0].Transcript);
            // }
        }
    }
}