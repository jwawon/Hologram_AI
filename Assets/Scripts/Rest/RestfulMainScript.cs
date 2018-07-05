using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Models;
using Proyecto26;
using System.Collections.Generic;
using LitJson;
using UnityEngine.UI;

public class RestfulMainScript : MonoBehaviour {
	[SerializeField]
	string basePath_fromai;
	
	public Text nlpAnswerText;
	public static string nlpAnswer;
	public GameObject ssManager;
	public ExampleStreaming SpeechToTextManager;
	public static bool isNlpComplete;
	public static bool isStartAnim;
	private void OnEnable() {
		if(ExampleStreaming.isComplete){
			SpeechToTextManager.StopRecording();
			getDataFromNLP();
		}
	}
	
	public void getDataFromNLP() {
		//sample 로 일단 http 처리를 하겠습니다.
		// string question = "hello world";
		string question = ExampleStreaming.ttsQuestion;

		Debug.Log(question);
		RestClient.Get(basePath_fromai + "/api/rest/v1.0/ask?question="+question).Then (res => {
			//success 
			JsonData tmp_array_data = JsonMapper.ToObject(res.Text);
			JsonData tmp_response_data = tmp_array_data[0];
			JsonData tmp_json_data = tmp_response_data["response"];
			nlpAnswer = (string) tmp_json_data["answer"];
			nlpAnswerText.text = nlpAnswer;
			isNlpComplete = true;
			isStartAnim = true;
			ExampleStreaming.isComplete = false;
			ssManager.SetActive(true);
			this.gameObject.SetActive(false);
		});
	}

}