using UnityEngine;

[System.Serializable]
public class AnimationCurve4
{
	
	#region Properties

	private AnimationCurve x 
	{ 
		get { return m_X; } 
	}
	[SerializeField]
	private AnimationCurve m_X = new AnimationCurve();
	
	private AnimationCurve y 
	{ 
		get { return m_Y; } 
	}
	[SerializeField]
	private AnimationCurve m_Y = new AnimationCurve();
	
	private AnimationCurve z 
	{ 
		get { return m_Z; } 
	}
	[SerializeField]
	private AnimationCurve m_Z = new AnimationCurve();
	
	private AnimationCurve w 
	{ 
		get { return m_W; } 
	}
	[SerializeField]
	private AnimationCurve m_W = new AnimationCurve();
	
	#endregion
	
	#region Methods

	public void AddKey(float time, Quaternion quaternion, float? inTangent, float? outTangent)
	{
		Keyframe xKey = new Keyframe(time, quaternion.x);
		Keyframe yKey = new Keyframe(time, quaternion.y);
		Keyframe zKey = new Keyframe(time, quaternion.z);
		Keyframe wKey = new Keyframe(time, quaternion.w);

		if (inTangent != null)
		{
			xKey.inTangent = inTangent.Value;
			yKey.inTangent = inTangent.Value;
			zKey.inTangent = inTangent.Value;
			wKey.inTangent = inTangent.Value;
		}
		if (outTangent != null)
		{
			xKey.outTangent = outTangent.Value;
			yKey.outTangent = outTangent.Value;
			zKey.outTangent = outTangent.Value;
			wKey.outTangent = outTangent.Value;
		}

		x.AddKey(xKey);
		y.AddKey(yKey);
		z.AddKey(zKey);
		w.AddKey(zKey);
	}

	public void AddKey(float time, Quaternion quaternion)
	{
		x.AddKey(time, quaternion.x);
		y.AddKey(time, quaternion.y);
		z.AddKey(time, quaternion.z);
		w.AddKey(time, quaternion.w);
	}
	
	public Quaternion Evaluate(float time)
	{
		Quaternion quaternion = new Quaternion();
		quaternion.x = x.Evaluate(time);
		quaternion.y = y.Evaluate(time);
		quaternion.z = z.Evaluate(time);
		quaternion.w = w.Evaluate(time);
		return quaternion;
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
			w.RemoveKey(i);
        } 
    }
	
	public float LastTime()
	{
		return x.keys[x.length - 1].time;
	}
	
	#endregion

}
