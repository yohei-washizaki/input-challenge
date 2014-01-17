using UnityEngine;
using System.Collections;

/// <summary>
/// Hold listener delegate.
/// </summary>
public delegate void HoldListenerDelegate(Vector2 point);

[RequireComponent(typeof(ParticleSystem))]
[AddComponentMenu("Scripts/Washi/Hold Listener")]
public class HoldListener : MonoBehaviour
{
	/// <summary>
	/// Gets or sets the screen.
	/// </summary>
	/// <value>The screen.</value>
	PseudoScreen screen{get;set;}

	/// <summary>
	/// The distance from screen.
	/// </summary>
	public float distanceFromScreen;

	/// <summary>
	/// The threshold sec.
	/// </summary>
	public float thresholdSec = 1.0F;

	private float timer {get; set;}

	// Use this for initialization
	void Start ()
	{
		this.timer = 0;

		if(this.distanceFromScreen < Camera.main.nearClipPlane)
		{
			this.distanceFromScreen = Camera.main.nearClipPlane;
		}

		GameObject obj = GameObject.Find("PseudoScreen");
		if(obj)
		{
			this.screen = obj.GetComponent<PseudoScreen>();
		}
	}

	public event HoldListenerDelegate OnHoldBegan;
	public event HoldListenerDelegate OnHoldStay;
	public event HoldListenerDelegate OnHoldMoved;
	public event HoldListenerDelegate OnHoldEnded;
	public event HoldListenerDelegate OnHoldCanceled;

	bool isFirstNotification = true;
	
	// Update is called once per frame
	void Update ()
	{
		if(!this.screen)
		{
			return;
		}

		if(!this.particleSystem)
		{
			return;
		}

		if(this.screen.touchCount > 0)
		{
			switch(this.screen.touchPhase)
			{
			case TouchPhase.Began:
				this.timer = 0;
				this.isFirstNotification = true;
				break;

			case TouchPhase.Moved:
				this.timer = 0;
				this.particleSystem.Stop();
				
				if(this.OnHoldMoved != null)
				{
					this.OnHoldMoved(this.screen.lastTouchPoint);
				}

				break;

			case TouchPhase.Stationary:
				this.timer += Time.deltaTime;

				if(this.timer > this.thresholdSec)
				{
					if(this.isFirstNotification)
					{
						if(this.OnHoldBegan != null)
						{
							this.OnHoldBegan(this.screen.lastTouchPoint);
						}
						this.isFirstNotification = false;
					}
					else
					{
						if(this.OnHoldStay != null)
						{
							this.OnHoldStay(this.screen.lastTouchPoint);
						}
					}

					if(this.particleSystem.isPlaying)
					{
						// Do nothing.
					}
					else
					{
						Vector3 screenPoint = this.screen.lastTouchPoint;
						screenPoint.z       = this.distanceFromScreen;
						
						Vector3 worldPoint  = Camera.main.ScreenToWorldPoint(screenPoint);
						this.particleSystem.transform.position = worldPoint;

						this.particleSystem.Play();
					}
				}
				break;

			case TouchPhase.Ended:
				if(this.OnHoldEnded != null)
				{
					this.OnHoldEnded(this.screen.lastTouchPoint);
				}
				this.particleSystem.Stop();
				break;

			case TouchPhase.Canceled:
				if(this.OnHoldCanceled != null)
				{
					this.OnHoldCanceled(this.screen.lastTouchPoint);
				}
				this.particleSystem.Stop();
				break;
			}
		}
	}

	void OnDisable()
	{
		if(this.particleSystem)
		{
			this.particleSystem.Stop();
		}
	}
}
