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


public class MobileCam : MonoBehaviour {
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
			// camAvailable = false;
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
			// Debug.Log("cameraTexture is null");
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


}
