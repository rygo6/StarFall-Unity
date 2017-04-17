using UnityEngine;
using System.Collections;

public class Keyframe3
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

	public Keyframe z
	{
		get { return m_Z; }
		private set { m_Z = value; }
	}
	private Keyframe m_Z;

	#endregion

	#region LifeCycle

	public Keyframe3(float time, Vector3 vector3)
	{
		x = new Keyframe(vector3.x, time);
		y = new Keyframe(vector3.y, time);
		z = new Keyframe(vector3.z, time);
	}

	#endregion

}