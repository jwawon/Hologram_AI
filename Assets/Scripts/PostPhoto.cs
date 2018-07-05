using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
// #if UNITY_EDITOR
// using UnityEditor;
// #endif
public class PostPhoto : MonoBehaviour {

	[SerializeField]
	string basePath_fromai;

	IEnumerator Start() {
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
        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);


        // Create a Web Form
        WWWForm form = new WWWForm();
        form.AddField("frameCount", Time.frameCount.ToString());
        form.AddBinaryData("fileUpload", bytes);

        // Upload to a cgi script
        var w = UnityWebRequest.Post(basePath_fromai + ":8000/polls/upload_file", form);
        yield return w.SendWebRequest();
        Debug.Log(w);

        if (w.isNetworkError || w.isHttpError)
        {
            Debug.Log(w.error);
        }
        else
        {
            Debug.Log("Finished Uploading Screenshot");
        }
    }

	public void getDataFromCV() {
        //sample 로 일단 http 처리를 하겠습니다.
        StartCoroutine(PostFileUpload("test"));
    }

	public IEnumerator PostFileUpload(string title) {
        string filePath = "/Users/Jwawon/Downloads/";
        string filename = "111.png";
        //string title = "test";
        //image load
        WWW localfile = new WWW ("file:///" + filePath + filename);
        yield return localfile;
		Debug.Log(localfile);
        if (localfile.error != null) {
            // EditorUtility.DisplayDialog ("Error", localfile.error , "Ok");
        }
        //set post parameter (key:value)
        WWWForm postForm = new WWWForm ();
		Debug.Log(postForm);
        //postForm.AddField ("title", title);
        postForm.AddBinaryData ("photo", localfile.bytes,filePath + filename, "text/plain");
        //post request
        WWW www = new WWW(basePath_fromai + ":8000/polls/upload_file", postForm);
        yield return www;
		Debug.Log(www);
        //result
        // if (www.error == null) {
        //     EditorUtility.DisplayDialog ("Error", www.text, "Ok");
        // } else {
        //     EditorUtility.DisplayDialog ("Error", www.error , "Ok");
        // }
    }
}
