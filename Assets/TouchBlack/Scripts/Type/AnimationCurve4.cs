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
	
	public void AddKey(float time, Quaternion point)
	{
		x.AddKey(time, point.x);
		y.AddKey(time, point.y);
		z.AddKey(time, point.z);
		w.AddKey(time, point.w);
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
	
	public void DeleteAfterTime(float time)
	{
		int deleteAferIndex = int.MaxValue;
		for (int i = 0; i < x.keys.Length; ++i)
		{
			if (x.keys[i].time > time)
			{
				deleteAferIndex = i - 1;
				break;
			}		
		}
		for (int i = x.keys.Length - 1; i > deleteAferIndex; --i)
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
