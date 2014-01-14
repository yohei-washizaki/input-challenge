using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Washi/PseudoScreen")]
public class PseudoScreen : MonoBehaviour
{
	int width;
	public int Width
	{
		get{return width;}
	}

	int height;
	public int Height
	{
		get{return height;}
	}

	public bool touchEnable = true;
	public bool TouchEnable
	{
		get;
		set;
	}

	// Use this for initialization
	void Start ()
	{
		this.width  = Screen.width;
		this.height = Screen.height;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!this.TouchEnable)
		{
			return;
		}

		if(Input.GetMouseButtonDown(0))
		{
		}
		int touchCount = 0;
		foreach(Touch touch in Input.touches)
		{
			switch(touch.phase)
			{
			case TouchPhase.Began:
				Debug.Log("Touch began.");
				break;
			case TouchPhase.Canceled:
				Debug.Log("Touch canceled.");
				break;
			case TouchPhase.Ended:
				Debug.Log("Touch end.");
				break;
			case TouchPhase.Moved:
				Debug.Log("Touch moved.");
				break;
			case TouchPhase.Stationary:
				Debug.Log ("Touch stationary.");
				break;
			}
		}
	}
}
