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

	private bool isActive = true;
	public bool IsActive
	{
		get{return this.isActive;}
		private set{this.isActive = value;}
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
		if(!this.IsActive)
		{
			return;
		}

		if(Input.GetMouseButtonDown(0))
		{
		}
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