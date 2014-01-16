using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Touch listener.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
[AddComponentMenu("Scripts/Washi/Touch Listener")]
public class TouchListener : MonoBehaviour
{
	/// <summary>
	/// Gets or sets the screen.
	/// </summary>
	/// <value>The screen.</value>
	PseudoScreen screen{get;set;}

	/// <summary>
	/// Gets or sets the line points.
	/// </summary>
	/// <value>The line points.</value>
	List<Vector3> linePoints{get;set;}

	/// <summary>
	/// Gets or sets the line renderer.
	/// </summary>
	/// <value>The line renderer.</value>
	LineRenderer lineRenderer{get;set;}

	/// <summary>
	/// Gets or sets the camera.
	/// </summary>
	/// <value>The camera.</value>
	Camera mainCamera{get;set;}

	public float threshold  = 0.001F;
	public float startWidth = 0.01F;
	public float endWidth   = 0.01F;
	public float distanceFromScreen = 1.0F;

	// Use this for initialization
	void Start ()
	{
		this.mainCamera   = Camera.main;
		this.lineRenderer = GetComponent<LineRenderer>();
		this.linePoints   = new List<Vector3>();

		GameObject obj = GameObject.Find("PseudoScreen");
		if(obj)
		{
			this.screen = obj.GetComponent<PseudoScreen>();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(this.screen)
		{
			if(this.screen.touchCount > 0)
			{
				switch(this.screen.touchPhase)
				{
				case TouchPhase.Began:
				{
					// Replace the linepoints container.
					this.linePoints.Clear();

					Vector3 screenTouchPoint = this.screen.firstTouchPoint;
					screenTouchPoint.z       = this.distanceFromScreen;

					// Convert screen coord to world coord.
					Vector3 worldTouchPoint  = this.mainCamera.ScreenToWorldPoint(screenTouchPoint);

					this.linePoints.Add(worldTouchPoint);
					break;
				}

				case TouchPhase.Moved:
				{
					Vector3 screenTouchPoint = this.screen.lastTouchPoint;
					screenTouchPoint.z       = this.distanceFromScreen;

					Vector3 worldTouchPoint  = this.mainCamera.ScreenToWorldPoint(screenTouchPoint);
					Vector3 lastTouchPoint   = this.linePoints[this.linePoints.Count - 1];

					if(Vector3.SqrMagnitude(worldTouchPoint - lastTouchPoint) > (this.threshold * this.threshold))
					{
						this.linePoints.Add(worldTouchPoint);
					}
					break;
				}

				case TouchPhase.Ended:
					// Remove linepoints from the container.
					this.linePoints.Clear();
					break;

				case TouchPhase.Canceled:
					// Remove linepoints from the container.
					this.linePoints.Clear();
					break;
				}
			}

			this.lineRenderer.SetWidth(this.startWidth, this.endWidth);
			this.lineRenderer.SetVertexCount(this.linePoints.Count);
			for(int i=0; i<this.linePoints.Count; ++i)
			{
				lineRenderer.SetPosition(i, this.linePoints[i]);
			}
		}
	}
}
