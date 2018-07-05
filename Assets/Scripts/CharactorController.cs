using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorController : MonoBehaviour {
	public Animator sushiAnim;


	void Update () {
		if(RestfulMainScript.isStartAnim){
			if(RestfulMainScript.nlpAnswer.Split(',')[0] == "Sorry"){
				sushiAnim.SetTrigger("Sorry");
				RestfulMainScript.isStartAnim = false;
			}
			else{
				sushiAnim.SetTrigger("Order");
				RestfulMainScript.isStartAnim = false;
			}
		}
	}
}
