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

	Collider cachedCollider{get;set;}
	
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
				this.timer = 0;
				Ray ray = Camera.main.ScreenPointToRay(this.screen.firstTouchPoint);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, 100))
				{
					this.cachedCollider = hit.collider;
					Renderer renderer = this.cachedCollider.gameObject.GetComponent<Renderer>();
					if(renderer)
					{
						renderer.material.color = Color.green;
					}
				}
				break;
			}

			case TouchPhase.Moved:
			{
				// Use cached collider.
				if(this.cachedCollider)
				{
					Ray ray = Camera.main.ScreenPointToRay(this.screen.lastTouchPoint);
					RaycastHit hit;
					if(this.cachedCollider.Raycast(ray, out hit, 100))
					{
						Renderer renderer = this.cachedCollider.gameObject.renderer;
						if(renderer)
						{
							renderer.material.color = Color.blue;
						}
					}
					else
					{
						// If there are no hit object, 
						Renderer renderer = this.cachedCollider.gameObject.renderer;
						if(renderer)
						{
							renderer.material.color = Color.white;
						}
						this.cachedCollider = null;
					}
				}
				else
				{
					// Find a hit object from all objects in the scene.
					// This process takes longer than the cached one...
					Ray ray = Camera.main.ScreenPointToRay(this.screen.lastTouchPoint);
					RaycastHit hit;
					if(Physics.Raycast(ray, out hit, 100))
					{
						this.cachedCollider = hit.collider;
						Renderer renderer = this.cachedCollider.gameObject.renderer;
						if(renderer)
						{
							renderer.material.color = Color.green;
						}
					}
				}
				this.timer = 0;
				this.particleSystem.Stop();
				break;
			}

			case TouchPhase.Stationary:
			{
				this.timer += Time.deltaTime;

				if(this.timer > this.thresholdSec)
				{
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

						// Change material color
						if(this.cachedCollider)
						{
							Renderer renderer = this.cachedCollider.gameObject.renderer;
							if(renderer)
							{
								renderer.material.color = Color.red;
							}
						}
					}
				}
				break;
			}

			case TouchPhase.Ended:
			{
				if(this.cachedCollider)
				{
					Renderer renderer = this.cachedCollider.gameObject.GetComponent<Renderer>();
					renderer.material.color = Color.white;
				}

				this.particleSystem.Stop();
				break;
			}

			case TouchPhase.Canceled:
			{
				if(this.cachedCollider)
				{
					Renderer renderer = this.cachedCollider.gameObject.GetComponent<Renderer>();
					renderer.material.color = Color.white;
				}

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
