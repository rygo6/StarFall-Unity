using UnityEngine;

[System.Serializable]
public class AnimationCurve3 
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
	
	#endregion
	
	#region Methods
	
	public void AddKey(float time, Vector3 point)
	{
		x.AddKey(time, point.x);
		y.AddKey(time, point.y);
		z.AddKey(time, point.z);
	}
	
	public Vector3 Evaluate(float time)
	{
		Vector3 vector3 = new Vector3();
		vector3.x = x.Evaluate(time);
		vector3.y = y.Evaluate(time);
		vector3.z = z.Evaluate(time);
		return vector3;
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
		}
	}
	
	public float LastTime()
	{
		return x.keys[x.length - 1].time;
	}
	
	#endregion

}
