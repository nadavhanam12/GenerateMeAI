using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class WebService : MonoBehaviour
{
    // [SerializeField] PostRequestHandler m_postRequestHandler;
    // [SerializeField] GetRequestHandler m_getRequestHandler;
    // [SerializeField] DownloadPngHandler m_downloadPngHandler;

    const string URL = "https://api.thenextleg.io/v2/";
    const string TOKEN = "7a4715c6-e482-49fd-bb42-54e61a798346";
    private WaitForSeconds m_progressWaitTime;
    private int progress;
    private string messageId;
    private string downloadUrl;
    private Texture2D m_generatedTexture;
    private bool m_available = true;
    [SerializeField] EnterPromptController m_enterPromptController;
    [SerializeField] private bool m_connection;
    [SerializeField] private float m_getImageInterval;

    public void GenerateImage(string themePrompt, string playerPrompt)
    {
        if (!m_connection)
        {
            Debug.Log("WebService: no connection");
            return;
        }
        if (!m_available)
        {
            Debug.Log("WebService: not available");
            return;
        }
        m_available = false;
        string prompt = themePrompt + "::" + playerPrompt;
        m_progressWaitTime = new WaitForSeconds(m_getImageInterval);
        Debug.Log(prompt);
        StartCoroutine(GenerateImages(prompt));
    }
    IEnumerator GenerateImages(string prompt)
    {
        // post imagine command and set messageId
        // messageId="" if fail
        yield return StartCoroutine(PostImagineCommand(prompt));
        if (messageId == "")
        {
            ProcessFail("Post Imagine Command");
            yield break;
        }

        //messageId = "war9nUZovSlyBaVFGsys";
        // 2. Send a GET request every 1 second to check progress
        yield return StartCoroutine(GetImageURL());
        if (downloadUrl == "")
        {
            ProcessFail("Get Image URL");
            yield break;
        }

        // 3. If progress is 100, download the PNG from the URL received in step 2
        yield return StartCoroutine(DownloadImage());
        if (m_generatedTexture == null)
        {
            ProcessFail("Download Image");
            yield break;
        }
        else
            m_enterPromptController.UpdateNewImage(m_generatedTexture);
    }

    IEnumerator PostImagineCommand(string prompt)
    {
        PostRequestData requestData = new PostRequestData(prompt);
        string data = JsonConvert.SerializeObject(requestData); ;
        string postUrl = URL + "imagine";

        // 1. Send a POST request to an API to get a message ID
        using (UnityWebRequest postRequest = UnityWebRequest.Post
            (postUrl,
             data))
        {
            postRequest.SetRequestHeader("Authorization", "Bearer " + TOKEN);
            postRequest.SetRequestHeader("Content-Type", "application/json");

            postRequest.uploadHandler.contentType = "application/json";
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(data);
            postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);

            yield return postRequest.SendWebRequest();

            if (postRequest.result == UnityWebRequest.Result.Success)
            {
                PostResponseData responseData = JsonConvert.DeserializeObject<PostResponseData>(postRequest.downloadHandler.text);
                messageId = responseData.messageId;
                //Debug.Log(messageId);
            }
            else
            {
                Debug.Log("Error: " + postRequest.error);
                Debug.Log(postRequest.downloadHandler.text);
                messageId = "";
            }
        }
    }
    IEnumerator GetImageURL()
    {
        print(messageId);
        progress = 0;
        while (progress < 100)
        {
            yield return m_progressWaitTime; ;
            using (UnityWebRequest progressRequest = UnityWebRequest.Get(URL + "message/" + messageId))
            {
                progressRequest.SetRequestHeader("Authorization", "Bearer " + TOKEN);
                progressRequest.SetRequestHeader("Content-Type", "application/json");

                yield return progressRequest.SendWebRequest();
                if (progressRequest.result == UnityWebRequest.Result.Success)
                {
                    GetResponseData responseData = JsonConvert.DeserializeObject<GetResponseData>(progressRequest.downloadHandler.text);
                    progress = responseData.progress;
                    if (progress == 100)
                        downloadUrl = responseData.response.imageUrl;
                }
                else
                {
                    Debug.Log("Error: " + progressRequest.error);
                    downloadUrl = "";
                }
            }
            print("progress: " + progress);
        }
    }
    IEnumerator DownloadImage()
    {
        Debug.Log(downloadUrl);
        using (UnityWebRequest pngRequest = UnityWebRequestTexture.GetTexture(downloadUrl))
        {
            yield return pngRequest.SendWebRequest();

            if (pngRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("pngRequest finished Successfully");
                m_generatedTexture = DownloadHandlerTexture.GetContent(pngRequest);
            }
            else
            {
                Debug.Log("Error: " + pngRequest.error);
                m_generatedTexture = null;
            }
        }
    }
    // IEnumerator Start()
    // {
    //     using (WWW www = new WWW(url))
    //     {
    //         yield return www;

    //         if (string.IsNullOrEmpty(www.error))
    //         {
    //             Texture2D texture = WebP.LoadTexture(www.bytes);
    //             targetRenderer.material.mainTexture = texture;
    //         }
    //         else
    //         {
    //             Debug.LogError(www.error);
    //         }
    //     }
    // }


    private void ProcessFail(string failFunction)
    {
        m_enterPromptController.UpdateFailImageProcces();
        Debug.Log("Fail Image Process: " + failFunction);
    }
}


