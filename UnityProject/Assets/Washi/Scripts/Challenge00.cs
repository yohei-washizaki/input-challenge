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

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		// Cache components for convenience.
		this.meshRenderer = this.renderer as MeshRenderer;
		this.meshCollider = this.collider as MeshCollider;

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
	}

	/// <summary>
	/// Raises the hold began event.
	/// </summary>
	/// <param name="point">Point.</param>
	void OnHoldBegan(Vector2 point)
	{
		Ray ray = Camera.main.ScreenPointToRay(point);
		RaycastHit hit;
		if(this.meshCollider.Raycast(ray, out hit, float.MaxValue))
		{
			this.meshRenderer.material.color = Color.red;
		}
	}

	/// <summary>
	/// Raises the hold stay event.
	/// </summary>
	/// <param name="point">Point.</param>
	void OnHoldStay(Vector2 point)
	{
		Debug.Log("Hold stay" + point);
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
			this.meshRenderer.material.color = Color.red;
		}
		else
		{
			this.meshRenderer.material.color = Color.white;
		}
	}

	/// <summary>
	/// Raises the hold ended event.
	/// </summary>
	/// <param name="point">Point.</param>
	void OnHoldEnded(Vector2 point)
	{
		this.meshRenderer.material.color = Color.white;
	}

	/// <summary>
	/// Raises the hold canceled event.
	/// </summary>
	/// <param name="point">Point.</param>
	void OnHoldCanceled(Vector2 point)
	{
		this.meshRenderer.material.color = Color.white;
	}

	/// <summary>
	/// Raises the GUI event.
	/// </summary>
	void OnGUI()
	{

	}
}
