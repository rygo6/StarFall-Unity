using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviourBase
{
	
	#region Properties
	
	private int level 
	{ 
		get { return m_Level; } 
		set { m_Level = value; } 
	}
	private int m_Level;
	
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

	private AnimationCurve activeHistoryCurve 
	{ 
		get { return m_ActiveHistoryCurve; } 
	}
	[SerializeField]
	private AnimationCurve m_ActiveHistoryCurve = new AnimationCurve();
	
	#endregion
	
	#region MonoBehaviour
	
	private void Awake()
	{
		
	}
	
	private void Start() 
	{
		
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		componentCache.spriteRenderer.color = Color.red;
	}

	public void OnCollisionExit2D(Collision2D collision)
	{
		componentCache.spriteRenderer.color = Color.white;
	}
	
	#endregion
	
	#region Methods
	
	public void RecordTransformHistory(float time)
	{
		positionHistoryCurve.AddKey(time, transform.position);
		rotationHistoryCurve.AddKey(time, transform.rotation);
		velocityHistoryCurve.AddKey(time, componentCache.rigidBody2D.velocity);
		angularVelocityHistoryCurve.AddKey(time, componentCache.rigidBody2D.angularVelocity);
	}
	
	public void EvaluateTransformHistory(float time)
	{
		transform.position = positionHistoryCurve.Evaluate(time);
		transform.rotation = rotationHistoryCurve.Evaluate(time);
		componentCache.rigidBody2D.velocity = velocityHistoryCurve.Evaluate(time);
		componentCache.rigidBody2D.angularVelocity = angularVelocityHistoryCurve.Evaluate(time);
	}
	
	public void DeleteHistoryAfterTime(float time)
	{
		positionHistoryCurve.DeleteAfterTime(time);
		rotationHistoryCurve.DeleteAfterTime(time);
		velocityHistoryCurve.DeleteAfterTime(time);
		angularVelocityHistoryCurve.DeleteAfterTime(time);
	}

	/// <summary>
	/// Holds physics object in place of actual transform
	/// until the fixedUpdate runs to update it's place
	/// in the physics simulation.
	/// </summary>
	public IEnumerator UpdatePhysicsToActualPosition()
	{
		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;
		yield return new WaitForFixedUpdate();
		transform.position = position;
		transform.rotation = rotation;		
	}
	
	#endregion

}