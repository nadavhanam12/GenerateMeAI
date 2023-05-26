using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class WebService : MonoBehaviour
{


    const string URL = "https://api.thenextleg.io/v2/";
    const string TOKEN = "7a4715c6-e482-49fd-bb42-54e61a798346";
    private WaitForSeconds m_progressWaitTime;
    private int progress;
    private string messageId;
    private string downloadUrl;
    private Texture2D m_generatedTexture;
    private string m_prompt;
    private bool m_available = true;
    private int m_tryCounts = 0;
    private bool m_stopCoroutineFlag = false;

    [SerializeField] GenerateImageController m_generateImageController;
    [SerializeField] private bool m_connection;
    [SerializeField] private bool m_midJourneyV4 = true;
    [SerializeField] private bool m_lowQuality = true;
    [SerializeField] private bool m_withLogs;

    [SerializeField] private float m_getImageInterval;
    [SerializeField] private float m_dummyGenerationStageDuration;
    [SerializeField] private Texture2D m_dummyImage;


    public void GenerateImage(string themePrompt, string playerPrompt)
    {
        if (!m_connection)
        {
            Debug.Log("WebService: no connection");
            StartCoroutine(GenerateDummyImage());
            return;
        }
        if (!m_available)
        {
            Debug.Log("WebService: not available");
            return;
        }

        m_available = false;
        m_prompt = themePrompt + "::" + playerPrompt;
        if (m_midJourneyV4)
            m_prompt = m_prompt + " --v 4";
        if (m_lowQuality)
            m_prompt = m_prompt + " --q .25";
        m_progressWaitTime = new WaitForSeconds(m_getImageInterval);
        if (m_withLogs)
            Debug.Log(m_prompt);
        StartCoroutine(GenerateImage());
    }
    IEnumerator GenerateImage()
    {
        // Step 1
        // post imagine command and set messageId
        // messageId="" if fail
        if (m_withLogs)
            Debug.Log(m_prompt);
        UpdateImageProcessText("Generating... 0/3");
        yield return StartCoroutine(PostImagineCommand(m_prompt));
        if (m_stopCoroutineFlag)
            yield break;

        // Step 2
        //messageId = "mFJJgmUpNRk93vOJhtZn";
        // 2. Send a GET request every 1 second to check progress
        yield return StartCoroutine(GetImageURL());
        if (m_stopCoroutineFlag)
            yield break;

        // Step 3
        // check image is png format
        string imageFormatType = downloadUrl.Substring(downloadUrl.Length - 3);
        if (imageFormatType != "png")
            //means its not png format
            if (m_tryCounts == 0)
            {
                Debug.Log("Got no png format - " + m_prompt);
                m_tryCounts++;
                StartCoroutine(GenerateImage());
                yield break;
            }
            else
            if (m_tryCounts == 1)
            {
                Debug.Log("Got no png format - " + m_prompt);
                m_tryCounts++;
                StartCoroutine(GenerateImage());
                yield break;
            }
            else
            {
                StopAllCoroutines();
                UpdateImageProcessText("Failed: Download Image On Second try");
                yield break;
            }

        // Step 4
        // 3. If progress is 100, download the PNG from the URL received in step 2
        yield return StartCoroutine(DownloadImage());
        if (m_stopCoroutineFlag)
            yield break;
        m_generateImageController.UpdateNewImage(m_generatedTexture);
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

        if (messageId == "")
        {
            UpdateImageProcessText("Failed: Post Imagine Command");
            m_stopCoroutineFlag = true;
        }
        else
            UpdateImageProcessText("Generating... 1/3");
    }

    IEnumerator GetImageURL()
    {
        if (m_withLogs)
            Debug.Log(messageId);
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
            UpdateImageProcessText("Generating... 1/3 " + progress + "%");
        }
        if (m_withLogs)
            Debug.Log(downloadUrl);
        if (downloadUrl == "")
        {
            UpdateImageProcessText("Failed: Get Image URL");
            m_stopCoroutineFlag = true;
        }
        else
            UpdateImageProcessText("Generating... 2/3");
    }
    IEnumerator DownloadImage()
    {
        using (UnityWebRequest pngRequest = UnityWebRequestTexture.GetTexture(downloadUrl))
        {
            yield return pngRequest.SendWebRequest();
            if (pngRequest.result == UnityWebRequest.Result.Success)
            {
                if (m_withLogs)
                    Debug.Log("pngRequest finished Successfully");
                m_generatedTexture = DownloadHandlerTexture.GetContent(pngRequest);
            }
            else
            {
                Debug.Log("Error: " + pngRequest.error);
                m_generatedTexture = null;
            }
        }

        if (m_generatedTexture == null)
        {
            UpdateImageProcessText("Failed: Download Image");
            m_stopCoroutineFlag = true;
        }
        else
        {
            UpdateImageProcessText("Generating... 3/3");
        }


    }

    private void UpdateImageProcessText(string message)
    {
        m_generateImageController.UpdateImageProcessText(message);
        if (m_withLogs)
            Debug.Log(message);
    }

    IEnumerator GenerateDummyImage()
    {
        UpdateImageProcessText("Dum Generating... 0/3");
        yield return new WaitForSeconds(m_dummyGenerationStageDuration);
        UpdateImageProcessText("Dum Generating... 1/3");
        yield return new WaitForSeconds(m_dummyGenerationStageDuration);
        UpdateImageProcessText("Dum Generating... 2/3 0%");
        yield return new WaitForSeconds(m_dummyGenerationStageDuration);
        UpdateImageProcessText("Dum Generating... 2/3 50%");
        yield return new WaitForSeconds(m_dummyGenerationStageDuration);
        UpdateImageProcessText("Dum Generating... 3/3");
        m_generateImageController.UpdateNewImage(m_dummyImage);
    }

    public void CancelProcess()
    {
        m_stopCoroutineFlag = true;
        StopAllCoroutines();
    }
}


