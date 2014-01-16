using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
[AddComponentMenu("Scripts/Washi/Hold Listener")]
public class HoldListener : MonoBehaviour
{
	/// <summary>
	/// Gets or sets the screen.
	/// </summary>
	/// <value>The screen.</value>
	PseudoScreen screen{get;set;}

	public float distance;

	// Use this for initialization
	void Start ()
	{
		if(this.distance < Camera.main.nearClipPlane)
		{
			this.distance = Camera.main.nearClipPlane;
		}

		GameObject obj = GameObject.Find("PseudoScreen");
		if(obj)
		{
			this.screen = obj.GetComponent<PseudoScreen>();
		}
	}
	
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
			{
				Vector3 screenPoint = this.screen.firstTouchPoint;
				screenPoint.z       = this.distance;

				Vector3 worldPoint  = Camera.main.ScreenToWorldPoint(screenPoint);
				this.particleSystem.transform.position = worldPoint;

				this.particleSystem.Play();
				break;
			}

			case TouchPhase.Moved:
			{
				this.particleSystem.Stop();
				break;
			}

			case TouchPhase.Stationary:
			{
				if(this.particleSystem.isPlaying)
				{
					// Do nothing.
				}
				else
				{
					Vector3 screenPoint = this.screen.lastTouchPoint;
					screenPoint.z       = this.distance;

					Vector3 worldPoint  = Camera.main.ScreenToWorldPoint(screenPoint);
					this.particleSystem.transform.position = worldPoint;
					this.particleSystem.Play();
				}
				break;
			}

			case TouchPhase.Ended:
			{
				this.particleSystem.Stop();
				break;
			}

			case TouchPhase.Canceled:
			{
				this.particleSystem.Stop();
				break;
			}
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
