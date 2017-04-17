using UnityEngine;

[System.Serializable]
public class AnimationCurve2
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
	
	#endregion
	
	#region Methods

	public void AddKey(float time, Vector2 vector2, float? inTangent, float? outTangent)
	{
		Keyframe xKey = new Keyframe(time, vector2.x);
		Keyframe yKey = new Keyframe(time, vector2.y);

		if (inTangent != null)
		{
			xKey.inTangent = inTangent.Value;
			yKey.inTangent = inTangent.Value;
		}
		if (outTangent != null)
		{
			xKey.outTangent = outTangent.Value;
			yKey.outTangent = outTangent.Value;
		}

		x.AddKey(xKey);
		y.AddKey(yKey);
	}

	public void AddKey(float time, Vector2 vector2)
	{
		x.AddKey(time, vector2.x);
		y.AddKey(time, vector2.y);
	}
	
	public Vector3 Evaluate(float time)
	{
		Vector2 vector2 = new Vector2();
		vector2.x = x.Evaluate(time);
		vector2.y = y.Evaluate(time);
		return vector2;
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
		}
	}
	
	public float LastTime()
	{
		return x.keys[x.length - 1].time;
	}
	
	#endregion

}
