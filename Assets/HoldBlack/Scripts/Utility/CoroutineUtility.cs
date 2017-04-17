using UnityEngine;
using System.Collections;

namespace EC
{
	public static class CoroutineUtility
	{
		public static IEnumerator WaitForUnscaledTime(float time)
		{
			float start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + time)
			{
				yield return null;
			}
		}
	}
}