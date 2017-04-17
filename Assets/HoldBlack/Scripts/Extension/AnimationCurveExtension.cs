using UnityEngine;
using System.Collections;

static public class AnimationCurveExtension
{
	
	static public void RemoveAfterTime(this AnimationCurve curve, float time)
	{	
		int deleteAferIndex = int.MaxValue;
		Keyframe[] keys = curve.keys;
		for (int i = 0; i < keys.Length; ++i)
		{
			if (keys[i].time > time)
			{
				deleteAferIndex = i - 1;
				break;
			}		
		}
		for (int i = keys.Length - 1; i > deleteAferIndex; --i)
        {
			curve.RemoveKey(i);
        }
    }

	static public void AddKey(this AnimationCurve curve, float time, float value, float? inTangent, float? outTangent)
	{
		Keyframe key = new Keyframe(time, value);

		if (inTangent != null)
		{
			key.inTangent = inTangent.Value;
		}
		if (outTangent != null)
		{
			key.outTangent = outTangent.Value;
		}

		curve.AddKey(key);
	}
	
}
