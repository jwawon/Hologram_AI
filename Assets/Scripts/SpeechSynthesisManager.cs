using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechSynthesisManager : MonoBehaviour {

	AudioSource source;
	string url = "http://61.85.36.59:9000/synthesize?text=hello%20world";
	void Start () {
		StartCoroutine(PlayText());
	}
	
    IEnumerator PlayText()
    {
        source = GetComponent<AudioSource>();
		var www = new WWW(url);
		yield return www;
		Debug.Log(www);
		Debug.Log("test");
		source.clip = www.GetAudioClip();
        if (!source.isPlaying)
        {
            source.Play();
        }
    }
}
