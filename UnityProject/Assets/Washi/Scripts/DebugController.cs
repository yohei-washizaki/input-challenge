using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

[AddComponentMenu("Scripts/Washi/Debug Controller")]
public class DebugController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Push [ESC] to quit editor player.
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				Type type = asm.GetType("UnityEditor.EditorApplication");
				if(type != null)
				{
					PropertyInfo prop = type.GetProperty("isPlaying");
					if(prop != null)
					{
						prop.SetValue(null, false, null);
					}
				}
			}
		}
	}
}
