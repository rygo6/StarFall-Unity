using UnityEngine;
using System.Collections;

public class Keyframe2
{

	#region Properties

	public Keyframe x
	{
		get { return m_X; }
		private set { m_X = value; }
	}
	private Keyframe m_X;

	public Keyframe y
	{
		get { return m_Y; }
		private set { m_Y = value; }
	}
	private Keyframe m_Y;

	#endregion

	#region LifeCycle

	public Keyframe2(float time, Vector2 vector2)
	{
		x = new Keyframe(vector2.x, time);
		y = new Keyframe(vector2.y, time);
	}

	#endregion

}