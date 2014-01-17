using UnityEngine;
using System.Collections;

public class Challenge00 : MonoBehaviour
{
	/// <summary>
	/// The hold listener.
	/// </summary>
	public HoldListener holdListener;

	/// <summary>
	/// The mesh renderer.
	/// </summary>
	MeshRenderer meshRenderer;

	/// <summary>
	/// The mesh collider.
	/// </summary>
	MeshCollider meshCollider;

	public float clearTime = 3.0F;
	float timer;
	public float Score{get;set;}

	enum Progress
	{
		NotStarted,
		InProgress,
		Finished
	}

	Progress progress;

	void reset()
	{
		this.progress = Progress.NotStarted;
		
		this.timer = 0;
		
		// Reset score
		this.Score = 0;

		this.meshRenderer.material.color = Color.white;
	}

	GUIText buttonLabel;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		// Cache components for convenience.
		this.meshRenderer = this.renderer as MeshRenderer;
		this.meshCollider = this.collider as MeshCollider;

		this.buttonLabel = GetComponentInChildren<GUIText>();

		this.reset();

		// Add events.
		if(this.holdListener)
		{
			this.holdListener.OnHoldBegan    += this.OnHoldBegan;
			this.holdListener.OnHoldStay     += this.OnHoldStay;
			this.holdListener.OnHoldMoved    += this.OnHoldMoved;
			this.holdListener.OnHoldCanceled += this.OnHoldCanceled;
			this.holdListener.OnHoldEnded    += this.OnHoldEnded;
		}
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		switch(this.progress)
		{
		case Progress.NotStarted:
			// Waiting for start.
			break;

		case Progress.InProgress:
			this.timer += Time.deltaTime;
			if(this.timer > this.clearTime)
			{
				this.progress = Progress.Finished;
			}
			break;

		case Progress.Finished:
			// Evaluate score.
			if(this.timer > 0)
			{
				this.Score = this.timer / this.clearTime;
				if(this.Score >= 1.0F)
				{
					this.Score = 1.0F;
					this.meshRenderer.material.color = Color.green;
				}
				else
				{
					this.meshRenderer.material.color = Color.yellow;
				}
			}
			else
			{
				this.Score = 0;
			}
			break;
		}
	}

	/// <summary>
	/// Raises the hold began event.
	/// </summary>
	/// <param name="point">Point.</param>
	void OnHoldBegan(Vector2 point)
	{
		if(this.progress == Progress.NotStarted)
		{
			this.timer = 0;

			Ray ray = Camera.main.ScreenPointToRay(point);
			RaycastHit hit;
			if(this.meshCollider.Raycast(ray, out hit, float.MaxValue))
			{
				this.meshRenderer.material.color = Color.red;
				this.progress = Progress.InProgress;
			}
		}
	}

	/// <summary>
	/// Raises the hold stay event.
	/// </summary>
	/// <param name="point">Point.</param>
	void OnHoldStay(Vector2 point)
	{
	}

	/// <summary>
	/// Raises the hold moved event.
	/// </summary>
	/// <param name="point">Point.</param>
	void OnHoldMoved(Vector2 point)
	{
		Ray ray = Camera.main.ScreenPointToRay(point);
		RaycastHit hit;
		if(this.meshCollider.Raycast(ray, out hit, float.MaxValue))
		{
			// Moved into inside
			// Nothing to do.
		}
		else
		{
			// Moved to outside => Challenge failed.
			if(this.progress == Progress.InProgress)
			{
				this.progress = Progress.Finished;
			}
		}
	}
	
	/// <summary>
	/// Raises the hold ended event.
	/// </summary>
	/// <param name="point">Point.</param>
	void OnHoldEnded(Vector2 point)
	{
		if(this.progress == Progress.InProgress)
		{
			this.progress = Progress.Finished;
		}
	}

	/// <summary>
	/// Raises the hold canceled event.
	/// </summary>
	/// <param name="point">Point.</param>
	void OnHoldCanceled(Vector2 point)
	{
		if(this.progress == Progress.InProgress)
		{
			this.progress = Progress.Finished;
		}
	}

	/// <summary>
	/// Raises the GUI event.
	/// </summary>
	void OnGUI()
	{
		if(GUI.Button(new Rect(0,0, 100, 30) ,"Retry?"))
		{
			reset();
		}

		if(this.progress == Progress.Finished)
		{
			this.buttonLabel.text = string.Format("Score: {0}", this.Score);
			this.buttonLabel.color = Color.white;
		}
	}
}
