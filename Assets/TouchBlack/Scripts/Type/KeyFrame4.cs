using UnityEngine;
using System.Collections;

public class Keyframe4
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

	public Keyframe w
	{
		get { return m_W; }
		private set { m_W = value; }
	}
	private Keyframe m_W;

	#endregion

	#region LifeCycle

	public Keyframe4(float time, Quaternion quaternion)
	{
		x = new Keyframe(quaternion.x, time);
		y = new Keyframe(quaternion.y, time);
		z = new Keyframe(quaternion.z, time);
		w = new Keyframe(quaternion.w, time);
	}

	#endregion

}