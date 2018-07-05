using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;
using Proyecto26;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MobileCam : MonoBehaviour {
	private readonly string basePath_fromai = "http://61.85.36.59:8000/polls/upload_file";
	private bool camAvailable;
	private WebCamTexture cameraTexture;
	private Texture defaultBackground;

	public RawImage background;
	public AspectRatioFitter fit;
	public bool frontFacing;
	private int fileIndex;
	private string fileName;
	[SerializeField]
	public class ServerResponse {
		public bool isHuman;
	}
	// [Serializable]


	// Use this for initialization
	void Start () {
		defaultBackground = background.texture;
		WebCamDevice[] devices = WebCamTexture.devices;

		if (devices.Length == 0)
			return;

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
			// Debug.Log("cameraTexture is null");
			return;
		}

		cameraTexture.Play (); // Start the camera
		background.texture = cameraTexture; // Set the texture

		camAvailable = true; // Set the camAvailable for future purposes.
		while (camAvailable)
		{
			StartCoroutine(camTexturePost(cameraTexture));
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!camAvailable)
			return;

		float ratio = (float)cameraTexture.width / (float)cameraTexture.height;
		fit.aspectRatio = ratio; // Set the aspect ratio

		float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f; // Find if the camera is mirrored or not
		background.rectTransform.localScale = new Vector3(1f, scaleY, 1f); // Swap the mirrored camera

		int orient = -cameraTexture.videoRotationAngle;
		background.rectTransform.localEulerAngles = new Vector3(0,0, orient);
	}


	IEnumerator camTexturePost(WebCamTexture camTexture){
		yield return new WaitForSeconds(5.0f);
		fileIndex++;
		fileName = "photo_"+ fileIndex.ToString();
		// RestClient.Request(new RequestHelper { 
		// Uri = "https://jsonplaceholder.typicode.com/photos",
		// Method = "POST",
		// Timeout = 10000,
		// Headers = new Dictionary<string, string> {
		// 	{ "Authorization", "Bearer JWT_token..." }
		// },
		// Body = newPost, //Content-Type: application/json
		// BodyString = "Use it instead of 'Body' if you want to use other tool to serialize the JSON",
		// SimpleForm = new Dictionary<string, string> {}, //Content-Type: application/x-www-form-urlencoded
		// FormSections = new List<IMultipartFormSection>() {}, //Content-Type: multipart/form-data
		// ChunkedTransfer = true,
		// IgnoreHttpException = true, //Prevent to catch http exceptions
		// }).Then(response => {
		// EditorUtility.DisplayDialog("Status", response.StatusCode.ToString(), "Ok");
		// });

	}
}
