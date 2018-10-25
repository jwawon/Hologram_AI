using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechSynthesisManager : MonoBehaviour {

	AudioSource source;
	string url = "";
	string TTSurl;

	void OnEnable() {
		TTSurl = url + RestfulMainScript.nlpAnswer +".mp3";
		StartCoroutine(PlayText());
	}

    IEnumerator PlayText()
    {
		// TTSurl = url +"Hello";
		Debug.Log(TTSurl);
        source = GetComponent<AudioSource>();
		var www = new WWW(TTSurl);
		yield return www;
		Debug.Log(www.GetType());
		source.clip = www.GetAudioClip();
        if (!source.isPlaying)
        {
            source.Play();
			// this.gameObject.SetActive(false);
        }

    }
}
