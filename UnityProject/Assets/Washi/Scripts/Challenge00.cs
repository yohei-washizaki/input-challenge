using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Washi/Challenge 00")]
public class Challenge00 : MonoBehaviour {

	private const int WIDTH = 100;
	private const int HEIGHT = 90;

	// Use this for initialization
	void Start () {
	
	}

	void OnGUI()
	{
		// Make a background box
		if(GUI.Button(new Rect(Screen.width/2 - WIDTH/2,
		                       Screen.height/2 - HEIGHT/2,
		                       WIDTH,
		                       HEIGHT),
		              "Touch me!"))
		{
			Debug.Log(":-)");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
