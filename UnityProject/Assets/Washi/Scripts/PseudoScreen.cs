using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Washi/PseudoScreen")]
public class PseudoScreen : MonoBehaviour
{
	int width;
	int height;

	public bool _isActive = true;
	public bool isActive
	{
		get{return this._isActive;}
		private set{this._isActive = value;}
	}

	/// <summary>
	/// Gets or sets the previous touch point.
	/// </summary>
	/// <value>The previous touch point.</value>
	Vector2 previousTouchPoint{get; set;}

	/// <summary>
	/// Gets the last touch point.
	/// </summary>
	/// <value>The last touch point.</value>
	public Vector2 lastTouchPoint
	{
		get;
		private set;
	}

	public float _sensibility = 1.0F;

	/// <summary>
	/// Gets or sets the sensibility.
	/// </summary>
	/// <value>The sensibility.</value>
	public float sensibility
	{
		get{return this._sensibility;}
		set{this._sensibility = value;}
	}

	/// <summary>
	/// Gets or sets the first touch point.
	/// </summary>
	/// <value>The first touch point.</value>
	public Vector2 firstTouchPoint{get; set;}

	// The internal state of touch sequence.
	private enum TouchState
	{
		None,
		Began,
		Ended,
		Canceled,
		Touching
	}
	/// <summary>
	/// Gets or sets the state of the touch.
	/// </summary>
	/// <value>The state of the touch.</value>
	TouchState touchState;

	/// <summary>
	/// Gets the touch phase.
	/// </summary>
	/// <value>The touch phase.</value>
	/// <description>
	/// The external state of touch sequence.
	/// </description>
	public TouchPhase touchPhase
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the touch count.
	/// </summary>
	/// <value>The touch count.</value>
	public int touchCount
	{
		get;
		private set;
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		this.width      = Screen.width;
		this.height     = Screen.height;
		this.touchState = TouchState.None;
		this.touchCount = 0;
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
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

		this.previousTouchPoint = this.lastTouchPoint;

#if UNITY_STANDALONE
		// Windows codes here.
		this.lastTouchPoint     = Input.mousePosition;
		
		// Handle state
		if(this.touchState == TouchState.None)
		{
			if(Input.GetMouseButtonDown(0))
			{
				this.firstTouchPoint  = this.lastTouchPoint;
				this.touchState       = TouchState.Began;
				this.touchPhase       = TouchPhase.Began;
				++this.touchCount;

				// TODO: Notify listeners of the event.
			}
		}
		else if(this.touchState == TouchState.Touching)
		{
			if(Input.GetMouseButtonUp(0))
			{
				this.touchState = TouchState.Ended;
				this.touchPhase = TouchPhase.Ended;

				// TODO: Notify listeners of the event.
			}
			else if(Input.GetMouseButton(0))
			{
				if(Input.GetMouseButtonDown(1))
				{
					this.touchState = TouchState.Canceled;
					this.touchPhase = TouchPhase.Canceled;

					// TODO: Notify listeners of the event.
				}
				else
				{
					this.touchState = TouchState.Touching;
					Vector2 deltaMove = this.previousTouchPoint - this.lastTouchPoint;
					if(deltaMove.sqrMagnitude > this.sensibility)
					{
						this.touchPhase = TouchPhase.Moved;

						// TODO: Notify listeners of the event.
					}
					else
					{
						this.touchPhase = TouchPhase.Stationary;
						// Debug.Log("Touch stationary.");
					}
				}
			}
		}
#else
		// iOS codes here.
		// TODO: Change state of this instance when each event comes.
		// TODO: Handle multiple events properly if needed.
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
#endif
	}
}