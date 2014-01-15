using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Washi/PseudoScreen")]
public class PseudoScreen : MonoBehaviour
{
	int width{get;set;}
	int height{get;set;}

	public bool _isActive = true;
	public bool isActive
	{
		get{return this._isActive;}
		private set{this._isActive = value;}
	}

	bool prevInput{get;set;}
	Vector2 prevMousePoint{get; set;}
	Vector2 curMousePoint{get;set;}

	public float _sensibility = 1.0F;
	public float sensibility
	{
		get{return this._sensibility;}
		set{this._sensibility = value;}
	}

	Vector2 startPoint{get; set;}

	private enum TouchState
	{
		None,
		Began,
		Ended,
		Canceled,
		Touching
	}
	TouchState touchState{get;set;}

	public int touchCount
	{
		get;
		private set;
	}

	// Use this for initialization
	void Start ()
	{
		this.width  = Screen.width;
		this.height = Screen.height;
		this.touchState = TouchState.None;
		this.touchCount = 0;
	}

	// Update is called once per frame
	void Update ()
	{
		if(!this.isActive)
		{
			return;
		}

		// Change state.
		switch(this.touchState)
		{
		case TouchState.None:
			// No change.
			break;
		case TouchState.Began:
			this.touchState = TouchState.Touching;
			break;
		case TouchState.Touching:
			// No change.
			break;
		case TouchState.Ended:
			this.touchState = TouchState.None;
			--this.touchCount;
			break;
		case TouchState.Canceled:
			this.touchState = TouchState.None;
			--this.touchCount;
			break;
		default:
			break;
		}

		this.prevMousePoint = this.curMousePoint;
		this.curMousePoint  = Input.mousePosition;
		
		// Handle state
		if(this.touchState == TouchState.None)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Debug.Log("Touch began.");
				this.startPoint  = this.curMousePoint;
				this.touchState  = TouchState.Began;
				++this.touchCount;

				// TODO: Notify listeners of the event.
			}
		}
		else if(this.touchState == TouchState.Touching)
		{
			if(Input.GetMouseButtonUp(0))
			{
				Debug.Log("Touch ended.");
				this.touchState = TouchState.Ended;

				// TODO: Notify listeners of the event.
			}
			else if(Input.GetMouseButton(0))
			{
				if(Input.GetMouseButtonDown(1))
				{
					Debug.Log("Touch canceled.");
					this.touchState = TouchState.Canceled;

					// TODO: Notify listeners of the event.
				}
				else
				{
					this.touchState = TouchState.Touching;
					Vector2 deltaMove = this.prevMousePoint - this.curMousePoint;
					if(deltaMove.sqrMagnitude > this.sensibility)
					{
						Debug.Log("Touch moved.");

						// TODO: Notify listeners of the event.
					}
					else
					{
						// Debug.Log("Touch stationary.");
					}
				}
			}
		}

		// Go through this path only on mobile devices.
		foreach(UnityEngine.Touch touch in Input.touches)
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