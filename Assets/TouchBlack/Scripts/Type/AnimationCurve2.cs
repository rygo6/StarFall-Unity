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
	
	public void AddKey(float time, Vector2 point)
	{
		x.AddKey(time, point.x);
		y.AddKey(time, point.y);
	}
	
	public Vector3 Evaluate(float time)
	{
		Vector2 vector2 = new Vector2();
		vector2.x = x.Evaluate(time);
		vector2.y = y.Evaluate(time);
		return vector2;
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
		}
	}
	
	public float LastTime()
	{
		return x.keys[x.length - 1].time;
	}
	
	#endregion

}
