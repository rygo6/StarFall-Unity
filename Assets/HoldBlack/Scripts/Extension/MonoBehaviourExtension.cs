using UnityEngine;
using System.Collections;

static public class MonoBehaviourExtension 
{

	public static void IsNull(this MonoBehaviour monoBehaviour)
	{
		if (monoBehaviour == null)
		{
			Debug.Log(monoBehaviour.name + " is null.");
		}
    }
   
}
