using UnityEngine;

[System.Serializable]
public class AnimationCurve3
{
	
	#region Properties

	private AnimationCurve x
	{ 
		get { return _x; } 
	}
	[SerializeField]
	private AnimationCurve _x = new AnimationCurve();

	private AnimationCurve y
	{ 
		get { return _y; } 
	}
	[SerializeField]
	private AnimationCurve _y = new AnimationCurve();

	private AnimationCurve z
	{ 
		get { return _z; } 
	}
	[SerializeField]
	private AnimationCurve _z = new AnimationCurve();

	private int keyLength
	{ 
		get { return _keyLength; } 
		set { _keyLength = value; } 
	}
	private int _keyLength;

	#endregion

	#region Methods

	private float CalculateLinearTangent(Keyframe key, Keyframe toKey)
	{
		return (float)(((double)key.value - (double)toKey.value) / ((double)key.time - (double)toKey.time));
	}

	public void AddKey(float time, Vector3 vector3, float? inTangent, float? outTangent)
	{
		Keyframe xKey = new Keyframe(time, vector3.x);
		Keyframe yKey = new Keyframe(time, vector3.y);
		Keyframe zKey = new Keyframe(time, vector3.z);

		if (inTangent != null)
		{
			xKey.inTangent = inTangent.Value;
			yKey.inTangent = inTangent.Value;
			zKey.inTangent = inTangent.Value;
		}
		if (outTangent != null)
		{
			xKey.outTangent = outTangent.Value;
			yKey.outTangent = outTangent.Value;
			zKey.outTangent = outTangent.Value;
		}

		x.AddKey(xKey);
		y.AddKey(yKey);
		z.AddKey(zKey);

		keyLength++;
	}

	public void AddKey(float time, Vector3 vector3)
	{
		x.AddKey(time, vector3.x);
		y.AddKey(time, vector3.y);
		z.AddKey(time, vector3.z);

		keyLength++;
	}
	
	public Vector3 Evaluate(float time)
	{
		Vector3 vector3 = new Vector3();
		vector3.x = x.Evaluate(time);
		vector3.y = y.Evaluate(time);
		vector3.z = z.Evaluate(time);
		return vector3;
	}
	
	public void RemoveAfterTime(float time)
	{	
		int deleteAferIndex = int.MaxValue;
		Keyframe[] keys = x.keys;
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
			x.RemoveKey(i);
			y.RemoveKey(i);
			z.RemoveKey(i);
			keyLength--;
		}
	}

	public float LastTime()
	{
		return x.keys[x.length - 1].time;
	}

	#endregion

}
