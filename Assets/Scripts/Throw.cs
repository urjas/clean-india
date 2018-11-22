using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour {

	public GameObject ball;
	public GameObject ARCamera;


	void Start(){
		ball.SetActive (false);
	}
	public void OnClick(){
		ball.SetActive (true);
		ball.GetComponent<Transform> ().Translate (ARCamera.GetComponent<Transform>().position);
	}
}
