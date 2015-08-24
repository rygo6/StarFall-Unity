using UnityEngine;
using System.Collections;

static public class AnimationCurveExtension
{
	
	static public void DeleteAfterTime(this AnimationCurve curve, float time)
	{	
		int deleteAferIndex = int.MaxValue;
		for (int i = 0; i < curve.keys.Length; ++i)
		{
			if (curve.keys[i].time > time)
			{
				deleteAferIndex = i - 1;
				break;
			}		
		}
		for (int i = curve.keys.Length - 1; i > deleteAferIndex; --i)
        {
			curve.RemoveKey(i);
        }
    }
	
}
