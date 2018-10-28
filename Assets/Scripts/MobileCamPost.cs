using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;
using Proyecto26;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Networking;


public class MobileCamPost : MonoBehaviour {
	public GameObject speechToText;
	public Text cvText;
	public GameObject model;
    bool cvResult;
	private readonly string basePath_fromai = "http://61.85.36.59:8000/polls/binary_receive";
	private bool camAvailable;
	private WebCamTexture cameraTexture;
	private Texture defaultBackground;
	public RawImage background;
	public AspectRatioFitter fit;
	public bool frontFacing;

	void Start () {
		defaultBackground = background.texture;
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length == 0){
			camAvailable = false;
			return;
		}

		for (int i = 0; i < devices.Length; i++)
		{
			var curr = devices[i];

			if (!curr.isFrontFacing)
			{
				cameraTexture = new WebCamTexture(curr.name,Screen.width,Screen.height);
				break;
			}
		}	

		if (cameraTexture == null){
			Debug.Log("cameraTexture is null");
			return;
		}

		cameraTexture.Play (); // Start the camera
		background.texture = cameraTexture; // Set the texture

		camAvailable = true; // Set the camAvailable for future purposes.
		while (camAvailable)
		{
			// StartCoroutine(camTexturePost(cameraTexture));
		}
		
	}
	// void Update () {
	// 	if (!camAvailable)
	// 		return;

	// 	float ratio = (float)cameraTexture.width / (float)cameraTexture.height;
	// 	fit.aspectRatio = ratio; // Set the aspect ratio

	// 	float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f; // Find if the camera is mirrored or not
	// 	background.rectTransform.localScale = new Vector3(1f, scaleY, 1f); // Swap the mirrored camera

	// 	int orient = -cameraTexture.videoRotationAngle;
	// 	background.rectTransform.localEulerAngles = new Vector3(0,0, orient);
	// }


	IEnumerator camTexturePost(WebCamTexture camTexture){
        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		tex.SetPixels(camTexture.GetPixels());
        tex.Apply();

        // Encode texture into PNG
        byte[] bytesImage = tex.EncodeToPNG();
        Object.Destroy(tex);

        // Upload to a cgi script
        var www = UnityWebRequest.Put(basePath_fromai, bytesImage);
        yield return www.SendWebRequest();
        // [{"label": "person", "confidence": 0.9353375434875488}]
        // string test = "[{label: person, confidence: 0.9353375434875488}]";
        cvResult = resultHandler(www.downloadHandler.text);
        // cvResult = resultHandler(test);
        // Debug.Log(cvResult);
		if(cvResult){
			speechToText.SetActive(true);
			model.SetActive(true);
			camAvailable = false;
		}
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Finished Uploading Screenshot");
            cvText.text = "Finished Uploading Screenshot";
        }
		yield return new WaitForSeconds(10f);
    }  
    public bool resultHandler(string resultText){
        bool isPerson = false;
		cvText.text = resultText;
        if (resultText.Split(':').Length > 1){
            if(resultText.Split(':')[1].Contains("person")){
                isPerson = true;
            }
        }
        return isPerson;
    }
}
