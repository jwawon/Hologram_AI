using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
public class CameraImagePost : MonoBehaviour {

	static WebCamTexture backCam;
	public RawImage background;
	public GameObject speechToText;
	public Text cvText;
	public GameObject model;
    bool cvResult;
	private readonly string basePath_fromai = "http://61.85.36.59:8000/polls/binary_receive/";
	private bool isCamAvailable;
	void Start () {
		Debug.Log(WebCamTexture.devices[0].name);
		WebCamDevice[] devices = WebCamTexture.devices;
		for (int i = 0; i < devices.Length; i++)
		{
			var curr = devices[i];

			if (backCam==null){
				backCam = new WebCamTexture(curr.name,Screen.width,Screen.height);
			}
		}
		
		background.texture = backCam;
		if(!backCam.isPlaying){
			backCam.Play();
			isCamAvailable = true;
			StartCoroutine(camTexturePost(backCam));
		}
	}
	
	
	IEnumerator camTexturePost(WebCamTexture camTexture){
		int i = 0;
		yield return new WaitForSeconds(3f);
        while(isCamAvailable){
			Stopwatch sw = new Stopwatch();
			sw.Start();
			cvText.text = "Start CV Analysis";
			// We should only read the screen buffer after rendering is complete
			yield return new WaitForEndOfFrame();

			// Create a texture the size of the screen, RGB24 format
			int width = camTexture.width;
			int height = camTexture.height;
			Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
	        // tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			tex.SetPixels(camTexture.GetPixels());
			tex.Apply();

			// Encode texture into PNG
			byte[] bytesImage = tex.EncodeToPNG();
			Object.Destroy(tex);
			// For testing purposes, also write to a file in the project folder
        	// File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytesImage);
			// Upload to a cgi script
			var www = UnityWebRequest.Put(basePath_fromai, bytesImage);

			yield return www.SendWebRequest();
			// [{"label": "person", "confidence": 0.9353375434875488}]
			// string test = "[{label: person, confidence: 0.9353375434875488}]";
			if(www.isDone){
				Debug.Log((sw.ElapsedMilliseconds/1000f).ToString() + "sec");
				cvResult = ResultHandler(www.downloadHandler.text);
				Debug.Log(cvResult);
				if(cvResult){
					yield return new WaitForSeconds(1f);
					speechToText.SetActive(true);
					model.SetActive(true);
					isCamAvailable = false;
					this.gameObject.SetActive(false);
				}
			}

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log("Finished Uploading Screenshot");
				// cvText.text = "Finished Uploading Screenshot";
			}
			// yield return new WaitForSeconds(5f);
		}
    }  
    public bool ResultHandler(string resultText){
        bool isPerson = false;
		cvText.text = resultText;
		Debug.Log(resultText);
        if (resultText.Split(',').Length > 1){
			string result1 = resultText.Split(',')[0].Split(':')[1];
			string result2 = resultText.Split(',')[1].Split(':')[1];

            if(result1.Contains("person") || result2.Contains("person")){
                isPerson = true;
            }
        }	
        return isPerson;
    }

}
