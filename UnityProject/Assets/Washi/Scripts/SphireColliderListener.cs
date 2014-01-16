using UnityEngine;
using System.Collections;

public class SphireColliderListener : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		this.renderer.material.color = Color.green;
	}

	void OnTriggerStay(Collider other)
	{
		this.renderer.material.color = Color.black;
	}
}
