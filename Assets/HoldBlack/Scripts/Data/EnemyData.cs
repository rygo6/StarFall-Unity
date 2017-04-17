using UnityEngine;
using System.Collections;

public class EnemyData 
{

	#region Properties

	private AnimationCurve3 positionHistoryCurve 
	{ 
		get { return m_PositionHistoryCurve; } 
	}
	[SerializeField]
	private AnimationCurve3 m_PositionHistoryCurve = new AnimationCurve3();

	private AnimationCurve4 rotationHistoryCurve 
	{ 
		get { return m_RotationHistoryCurve; } 
	}
	[SerializeField]
	private AnimationCurve4 m_RotationHistoryCurve = new AnimationCurve4();

	private AnimationCurve2 velocityHistoryCurve 
	{ 
		get { return m_VelocityHistoryCurve; } 
	}
	[SerializeField]
	private AnimationCurve2 m_VelocityHistoryCurve = new AnimationCurve2();

	private AnimationCurve angularVelocityHistoryCurve 
	{ 
		get { return m_AngularVelocityHistoryCurve; } 
	}
	[SerializeField]
	private AnimationCurve m_AngularVelocityHistoryCurve = new AnimationCurve();

	#endregion

	#region Methods

	

	#endregion

}