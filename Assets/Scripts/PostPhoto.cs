using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Proyecto26;
using LitJson;

public class PostPhoto : MonoBehaviour {

    bool cvResult;
    [SerializeField]
	string basePath_fromai;
    float initTime;
	IEnumerator Start() {
        initTime = Time.time;
		// getDataFromCV();
        yield return UploadPNG();
	}

    IEnumerator UploadPNG()
    {
        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytesImage = tex.EncodeToPNG();
        Object.Destroy(tex);

        // Upload to a cgi script
        var www = UnityWebRequest.Put(basePath_fromai + ":8000/polls/binary_receive/", bytesImage);
        yield return www.SendWebRequest();
        // [{"label": "person", "confidence": 0.9353375434875488}]
        // string test = "[{label: person, confidence: 0.9353375434875488}]";
        cvResult = resultHandler(www.downloadHandler.text);
        // cvResult = resultHandler(test);
        Debug.Log(cvResult);
        Debug.Log(Time.time - initTime);
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Finished Uploading Screenshot");
        }
    }  
    public bool resultHandler(string resultText){
        bool isPerson = false;
        if (resultText.Split(':').Length > 1){
            if(resultText.Split(':')[1].Contains("person")){
                isPerson = true;
            }
        }
        return isPerson;
    }
}
