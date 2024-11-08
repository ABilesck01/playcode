using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ApiController : MonoBehaviour
{
    public string api = "";

    public static ApiController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void Start()
    {
        LoadApiUrl();
    }

    public void SendRequest<T>(RequestType type, string endPoint, Action<T> onSuccess, Action<string> onError, object body = null)
    {
        Debug.Log($"Request sent to {api}{endPoint}");
        string jsonData = JsonUtility.ToJson(body);
        StartCoroutine(SendRequestCoroutine<T>(type, endPoint, onSuccess, onError, jsonData));
    }

    public void SendRequestArray<T>(RequestType type, string endPoint, Action<T[]> onSuccess, Action<string> onError, object body = null)
    {
        Debug.Log($"Request sent to {api}{endPoint}");
        string jsonData = JsonUtility.ToJson(body);
        StartCoroutine(SendRequestCoroutineArray<T>(type, endPoint, onSuccess, onError, jsonData));
    }

    private IEnumerator SendRequestCoroutine<T>(RequestType type, string endPoint, Action<T> onSuccess, Action<string> onError, string body = null)
    {
        UnityWebRequest request = UnityWebRequest.Get(api + endPoint);

        switch (type)
        {
            case RequestType.GET:
                request = UnityWebRequest.Get(api + endPoint);
                break;
            case RequestType.POST:
                
                request = UnityWebRequest.Post(api + endPoint, body, "application/json");
                break;
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            onError?.Invoke(request.error);
            yield break;
        }

        Debug.Log("API response:/n" + request.downloadHandler.text);

        try
        {
            T response = typeof(T) == typeof(string)
            ? (T)(object)request.downloadHandler.text
            : JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                onSuccess?.Invoke(response);
        }
        catch (Exception e)
        {
            onError?.Invoke($"Erro ao deserializar resposta: {e.Message}");
        }
    }

    private IEnumerator SendRequestCoroutineArray<T>(RequestType type, string endPoint, Action<T[]> onSuccess, Action<string> onError, string body = null)
    {
        UnityWebRequest request = UnityWebRequest.Get(api + endPoint);

        switch (type)
        {
            case RequestType.GET:
                request = UnityWebRequest.Get(api + endPoint);
                break;
            case RequestType.POST:

                request = UnityWebRequest.Post(api + endPoint, body, "application/json");
                break;
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            onError?.Invoke(request.error);
            yield break;
        }

        try
        {
            T[] response = JsonHelper.FromJson<T>(request.downloadHandler.text);
            onSuccess?.Invoke(response);
        }
        catch (Exception e)
        {
            onError?.Invoke($"Erro ao deserializar resposta: {e.Message}");
        }
    }

    private void LoadApiUrl()
    {
        // Caminho do arquivo de configuração
        string path = Path.Combine(Application.streamingAssetsPath, "config.txt");

        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (line.StartsWith("ApiUrl="))
                {
                    api = line.Substring("ApiUrl=".Length);
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Arquivo de configuração não encontrado!");
        }
    }
}


public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}

public enum RequestType
{
    GET,
    POST
}