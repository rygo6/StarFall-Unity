using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class Enemy : MonoBehaviour
{
    public RetireState RetireState { get; set; }

	[SerializeField]
	AnimationCurve3 _positionHistoryCurve = new AnimationCurve3();

	[SerializeField]
	AnimationCurve4 _rotationHistoryCurve = new AnimationCurve4();

	[SerializeField]
	AnimationCurve2 _velocityHistoryCurve = new AnimationCurve2();

	[SerializeField]
	AnimationCurve _angularVelocityHistoryCurve = new AnimationCurve();

	[SerializeField]
	AnimationCurve _activeHistoryCurve = new AnimationCurve();

	[SerializeField]
	AnimationCurve _retireStateHistoryCurve = new AnimationCurve();

	Rigidbody2D _rigidBody2D;

	private void Awake()
	{
		_rigidBody2D = GetComponent<Rigidbody2D>();
	}
	
	private void Start()
	{
		
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{

	}

	public void OnCollisionExit2D(Collision2D collision)
	{

	}


	public void RecordHistory(float time)
	{
		switch (RetireState)
		{
		case RetireState.NotRetired:
//			Debug.Log("NotRetired");
			_positionHistoryCurve.AddKey(time, transform.position);
			_rotationHistoryCurve.AddKey(time, transform.rotation);
			_velocityHistoryCurve.AddKey(time, _rigidBody2D.velocity);
			_angularVelocityHistoryCurve.AddKey(time, _rigidBody2D.angularVelocity);
			_retireStateHistoryCurve.AddKey(time, (float)RetireState, Mathf.Infinity, Mathf.Infinity);
			break;
		case RetireState.JustRetired:
//			Debug.Log("JustRetired");			
			_positionHistoryCurve.AddKey(time, transform.position, null, Mathf.Infinity);
			_rotationHistoryCurve.AddKey(time, transform.rotation, null, Mathf.Infinity);
			_velocityHistoryCurve.AddKey(time, _rigidBody2D.velocity, null, Mathf.Infinity);
			_angularVelocityHistoryCurve.AddKey(time, _rigidBody2D.angularVelocity, null, Mathf.Infinity);
			_retireStateHistoryCurve.AddKey(time, (float)RetireState, Mathf.Infinity, Mathf.Infinity);
			RetireState = RetireState.PostJustRetired;
			break;
		//PostJustRetired exists to ensure that atleast one full linear key gets placed
		//between the JustRetired and JustUnretired keys, otherwise the smoothing between these
		//may get messed up
		case RetireState.PostJustRetired:
//			Debug.Log("PostJustRetired");		
			_positionHistoryCurve.AddKey(time, transform.position, Mathf.Infinity, Mathf.Infinity);
			_rotationHistoryCurve.AddKey(time, transform.rotation, Mathf.Infinity, Mathf.Infinity);
			_velocityHistoryCurve.AddKey(time, _rigidBody2D.velocity, Mathf.Infinity, Mathf.Infinity);
			_angularVelocityHistoryCurve.AddKey(time, _rigidBody2D.angularVelocity, Mathf.Infinity, Mathf.Infinity);
			_retireStateHistoryCurve.AddKey(time, (float)RetireState, Mathf.Infinity, Mathf.Infinity);
			RetireState = RetireState.Retired;
			break;
		case RetireState.Retired:
//			Debug.Log("Retired");	
			_positionHistoryCurve.AddKey(time, transform.position, Mathf.Infinity, Mathf.Infinity);
			_rotationHistoryCurve.AddKey(time, transform.rotation, Mathf.Infinity, Mathf.Infinity);
			_velocityHistoryCurve.AddKey(time, _rigidBody2D.velocity, Mathf.Infinity, Mathf.Infinity);
			_angularVelocityHistoryCurve.AddKey(time, _rigidBody2D.angularVelocity, Mathf.Infinity, Mathf.Infinity);
			_retireStateHistoryCurve.AddKey(time, (float)RetireState, Mathf.Infinity, Mathf.Infinity);
			break;
		case RetireState.JustUnRetired:
//			Debug.Log("JustUnRetired");	
			_positionHistoryCurve.AddKey(time, transform.position, Mathf.Infinity, null);
			_rotationHistoryCurve.AddKey(time, transform.rotation, Mathf.Infinity, null);
			_velocityHistoryCurve.AddKey(time, _rigidBody2D.velocity, Mathf.Infinity, null);
			_angularVelocityHistoryCurve.AddKey(time, _rigidBody2D.angularVelocity, Mathf.Infinity, null);
			_retireStateHistoryCurve.AddKey(time, (float)RetireState, Mathf.Infinity, Mathf.Infinity);
			RetireState = RetireState.PostJustUnRetired;
			break;
		//PostJustUnRetired exists because keys must be added through this 
		//method in order to not smooth the linear keyframes added in JustUnRetired state
		case RetireState.PostJustUnRetired:
//			Debug.Log("PostJustUnRetired");	
			_positionHistoryCurve.AddKey(time, transform.position, null, null);
			_rotationHistoryCurve.AddKey(time, transform.rotation, null, null);
			_velocityHistoryCurve.AddKey(time, _rigidBody2D.velocity, null, null);
			_angularVelocityHistoryCurve.AddKey(time, _rigidBody2D.angularVelocity, null, null);
			_retireStateHistoryCurve.AddKey(time, (float)RetireState, Mathf.Infinity, Mathf.Infinity);
			RetireState = RetireState.NotRetired;
			break;
		}

		//always record active state and record state in linear
		_activeHistoryCurve.AddKey(time, System.Convert.ToInt32(gameObject.activeSelf), Mathf.Infinity, Mathf.Infinity);
	}
	
	public void EvaluateTransformHistory(float time)
	{
		transform.position = _positionHistoryCurve.Evaluate(time);
		transform.rotation = _rotationHistoryCurve.Evaluate(time);
		_rigidBody2D.velocity = _velocityHistoryCurve.Evaluate(time);
		_rigidBody2D.angularVelocity = _angularVelocityHistoryCurve.Evaluate(time);
		bool curveActiveState = System.Convert.ToBoolean(_activeHistoryCurve.Evaluate(time));
		if (gameObject.activeSelf != curveActiveState)
		{
			gameObject.SetActive(curveActiveState);
		}
		RetireState curveRetireState = (RetireState)Mathf.RoundToInt(_retireStateHistoryCurve.Evaluate(time));
		if (RetireState != curveRetireState)
		{
			RetireState = curveRetireState;
		}
		//force retire if begining
		if (time <= 0f)
			RetireState = RetireState.Retired;
	}
	
	public void RemoveHistoryAfterTime(float time)
	{
		_positionHistoryCurve.RemoveAfterTime(time);
		_rotationHistoryCurve.RemoveAfterTime(time);
		_velocityHistoryCurve.RemoveAfterTime(time);
		_angularVelocityHistoryCurve.RemoveAfterTime(time);
		_activeHistoryCurve.RemoveAfterTime(time);
		_retireStateHistoryCurve.RemoveAfterTime(time);
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
}