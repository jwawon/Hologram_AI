using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAnim : MonoBehaviour {

	public Animator testAnim;
	public Text HelloText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)){
			if(HelloText.text == "Hello"){
				testAnim.SetTrigger("Hello");
			}
		}
	}
}
