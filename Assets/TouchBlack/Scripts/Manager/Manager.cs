using UnityEngine;
using System.Collections;

public class Manager<T> : MonoBehaviour where T : MonoBehaviour
{

	#region Properties

	static public T sharedManager
	{
		get
		{
			return MonoBehaviourUtility.GetManager<T>(ref s_SharedManager);
		}
	}
	static private T s_SharedManager;

	#endregion


}