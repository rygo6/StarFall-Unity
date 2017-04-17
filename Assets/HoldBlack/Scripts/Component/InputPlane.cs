using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using EC;

public class InputPlane : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
	private const int FingerCount = 10;

	private const float TimeScaleMultiplier = 4f;

	[SerializeField]
	[Header("Prefab containing Finger component to instantiate.")]
	private GameObject _fingerPrefab;

	private readonly Finger[] Fingers = new Finger[FingerCount];

	private GameTime _gameTime;

	private void Awake()
	{
		for (int i = 0; i < FingerCount; ++i)
		{
			GameObject instance = Instantiate(_fingerPrefab);
			Fingers[i] = instance.GetComponent<Finger>();
			Fingers[i].gameObject.SetActive(false);
		}

		_gameTime = FindObjectOfType<GameTime>();
	}

	public void ResetFingers()
	{
		for (int i = 0; i < Fingers.Length; ++i)
		{
			Fingers[i].gameObject.SetActive(false);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_gameTime.TimeScaleDirection = TimeScaleMultiplier;
	
		int id = eventData.pointerId.NoNegative();
		Fingers[id].gameObject.SetActive(true);
		Fingers[id].transform.position = eventData.pointerCurrentRaycast.worldPosition;	
	}
	
	public void OnDrag(PointerEventData eventData)
	{
		int id = eventData.pointerId.NoNegative();
		Fingers[id].MovePosition(eventData.pointerCurrentRaycast.worldPosition);
	}
	
	public void OnPointerUp(PointerEventData eventData)
	{
		_gameTime.TimeScaleDirection = -TimeScaleMultiplier;
	
		int id = eventData.pointerId.NoNegative();
		Fingers[id].gameObject.SetActive(false);		
	}
}
