using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorController : MonoBehaviour {
	public Animator ChanAnim;
	public Text InputText;
	float InitTime;
	float ElapsedTime = 10f;
	void Start() {
		InitTime = Time.time;
	}
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)){
			if(InputText.text == "Hello" || InputText.text == "Hi") ChanAnim.SetTrigger("Hi");	
			if(InputText.text == "You're wrong") ChanAnim.SetTrigger("Wrong");	
			if(InputText.text == "You're right" || InputText.text == "Thank you") ChanAnim.SetTrigger("Yay");
			if(Time.time - InitTime > ElapsedTime){
				ChanAnim.SetBool("IsBored", true);
				InitTime = Time.time;
			}
			else 
				ChanAnim.SetBool("IsBored", false);
		}
	}
}
