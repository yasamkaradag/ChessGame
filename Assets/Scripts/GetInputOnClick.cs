using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetInputOnClick : MonoBehaviour {

	// Use this for initialization
	public Button btnClick;
	public InputField InputUser;
	public Text OutputText;
	public Text BlockSizeText;
	public static string BlockSizeTextHolder;




	public void Start(){
		
//		InputUser = GameObject.GetComponent<InputField>();
		btnClick.onClick.AddListener (GetInputOnClickHandler);
		BlockSizeText.text = cameraScript.tempblockSize.ToString ();
		
	}
	public void Update(){
		//BlockSizeText.text = cameraScript.tempblockSize.ToString ();
		BlockSizeText.text= ("Tahta Boyutu : " + cameraScript.tempblockSize.ToString () + "x" + cameraScript.tempblockSize.ToString ());
		//OutputText=("Min distance : "cameraScript.DistanceHolder.ToString());
		OutputText.text = ("Min distance : " + cameraScript.DistanceHolder.ToString());
	}

	public void GetInputOnClickHandler(){
		Debug.Log ("Log Output" + InputUser.text);
		Debug.Log (BlockSizeText.text);
		if(InputUser.text != null){
			cameraScript.tempblockSize=int.Parse(InputUser.text);
			cameraScript.resetle = true;
		}
		

	}

}
