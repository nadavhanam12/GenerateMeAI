// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEngine;
// using UnityEngine.Networking;

// public class WebService : MonoBehaviour
// {
//     const string Key = "e8680cafe1aa28a1f2b93392924ecc5c";
//     const string PostUrl = "https://fr4pc2ds7yz37mpait624qidg40jneoy.lambda-url.us-east-1.on.aws/";


//     public void SendRequest(string prompt)
//     {
//         m_bookModelList = books;
//         //Generate Data Json
//         DataJson dataJson = new DataJson();
//         dataJson.items = m_bookModelList;
//         dataJson.icons = m_iconModelList;

//         //Generate Post Request Model
//         PostRequestModel requestModel = new PostRequestModel(Key, dataJson);
//         SetData(requestModel);
//     }


//     public async Task InitData()
//     {
//         GetRequestModel requestModel = new GetRequestModel(Key);
//         DataJson response = await GetData(requestModel);
//         m_bookModelList = response.items;
//         m_iconModelList = response.icons;
//     }

//     async Task<DataJson> GetData(GetRequestModel getRequestModel)
//     {
//         WWWForm form = new WWWForm();
//         form.AddField("key", getRequestModel.key);
//         form.AddField("action", getRequestModel.action);
//         string responseString = await SendRequest(form);
//         DataJson response;
//         try
//         {
//             response = JsonUtility.FromJson<DataJson>(responseString);
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError(ex.Message);
//             return new DataJson();
//         }
//         if (response == null)
//         {
//             Debug.LogError("Response Deserialize failed");
//             response = new DataJson();
//         }
//         return response;
//     }

//     async void SetData(PostRequestModel postRequestModel)
//     {
//         WWWForm form = new WWWForm();
//         form.AddField("key", postRequestModel.key);
//         form.AddField("action", postRequestModel.action);
//         string dataJsonString = JsonUtility.ToJson(postRequestModel.data);
//         form.AddField("data", dataJsonString);
//         string responseString = await SendRequest(form);
//         if (responseString != "Success")
//             Debug.LogError("SendData Failed");
//     }

//     async Task<string> SendRequest(WWWForm form)
//     {
//         try
//         {
//             UnityWebRequest webRequest = UnityWebRequest.Post(PostUrl, form);
//             UnityWebRequestAsyncOperation operation = webRequest.SendWebRequest();

//             while (!operation.isDone)
//                 await Task.Yield();

//             if (webRequest.isNetworkError || webRequest.isHttpError)
//             {
//                 Debug.Log(webRequest.error);
//                 return null;
//             }
//             else
//             {
//                 Debug.Log("Send Request Complete!");
//                 return webRequest.downloadHandler.text;
//             }
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError(ex.Message);
//             return null;
//         }
//     }
//     async Task<Texture> GetIcon(string iconUrl)
//     {
//         try
//         {
//             UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(iconUrl);
//             UnityWebRequestAsyncOperation operation = webRequest.SendWebRequest();

//             while (!operation.isDone)
//                 await Task.Yield();

//             if (webRequest.isNetworkError || webRequest.isHttpError)
//             {
//                 Debug.Log(webRequest.error);
//                 return null;
//             }
//             else
//             {
//                 Debug.Log("Get Icon Completed!");
//                 Texture texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
//                 return texture;
//             }
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError(ex.Message);
//             return null;
//         }
//     }
// }
