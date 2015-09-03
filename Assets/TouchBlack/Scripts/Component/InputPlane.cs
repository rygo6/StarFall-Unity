using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputPlane : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

	#region Properties
	
	private int fingerCount 
	{ 
		get { return m_FingerCount; } 
	}
	[SerializeField]
	[Header("Amount of Finger colliders to instantiate.")]
	private int m_FingerCount = 10;

	private GameObject fingerPrefab 
	{ 
		get { return m_FingerPrefab; } 
	}
	[SerializeField]
	[Header("Prefab containing Finger component to instantiate.")]
	private GameObject m_FingerPrefab;
	
	private float timeScaleMultiplier 
	{ 
		get { return m_TimeScaleMultiplier; } 
	}
	[SerializeField]
	private float m_TimeScaleMultiplier = 4f;
	
	private Finger[] fingerArray
	{
		get
		{
			if (m_FingerArray == null)
			{
				m_FingerArray = new Finger[fingerCount];
				for (int i = 0; i < fingerCount; ++i)
				{
					GameObject instance = Instantiate(fingerPrefab);
					m_FingerArray[i] = instance.GetComponent<Finger>();
					m_FingerArray[i].gameObject.SetActive(false);
				}
			}
			return m_FingerArray;
		}	
	}
	private Finger[] m_FingerArray;
	
	#endregion
	
	#region MonoBehaviour
	
	private void Awake()
	{
		
	}

	#endregion
	
	#region EventSystems

	public void OnPointerDown(PointerEventData eventData)
	{
		EnemyManager.sharedManager.timeScaleDirection = timeScaleMultiplier;
	
		int id = eventData.pointerId.NoNegative();
		fingerArray[id].gameObject.SetActive(true);
		//immediately move finger to position, update via MovePosition in OnDrag so that it's movement affects physics
		fingerArray[id].transform.position = eventData.pointerCurrentRaycast.worldPosition;	
	}
	
	public void OnDrag(PointerEventData eventData)
	{
		int id = eventData.pointerId.NoNegative();
		fingerArray[id].componentCache.rigidBody2D.MovePosition(eventData.pointerCurrentRaycast.worldPosition);
	}
	
	public void OnPointerUp(PointerEventData eventData)
	{
		EnemyManager.sharedManager.timeScaleDirection = -timeScaleMultiplier;
	
		int id = eventData.pointerId.NoNegative();
		fingerArray[id].gameObject.SetActive(false);		
	}
	
	#endregion

}
