using UnityEngine;
using System;
using System.Collections;

public class GameTime : MonoBehaviour 
{
	public float TimeScaleDirection { get; set; }

	public event Action<float> FirstTimeForward;

	public event Action<float, float> TimeForward;

	public event Action<float> FirstTimeReverse;

	public event Action<float, float> TimeReverse;

	public float CurrentTime { get; private set; }

	private float _timeScale;

	private float _negativeDelta;

	private bool _firstPositiveTimeScale = false;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		Time.timeScale = 0f;
	}

	private void Start()
	{
		StartCoroutine(LateStartCoroutine());
	}
		
	/// <summary>
	/// Coroutine that will run one frame after Start
	/// </summary>
	private IEnumerator LateStartCoroutine()
	{
		yield return null;
		//this is a workaround to get rid of a .2f delay on first start
		CurrentTime = 0f;
	}

	private void Update()
	{
		SmoothTimeScaleInDirection();
		SetCurrentTime();
		TimeScaleSwitch();		
	}
		
	private void SetCurrentTime()
	{
		CurrentTime += _timeScale * Time.unscaledDeltaTime;
		if (CurrentTime < 0)
			CurrentTime = 0;
	}

	public void ZeroTime()
	{
		Time.timeScale = 0f;
		TimeScaleDirection = 0f;
		_timeScale = 0f;
	}
		
	private void SmoothTimeScaleInDirection()
	{
		//return from rewind faster
		if (TimeScaleDirection > 0f && _timeScale < 0f)
			_timeScale += TimeScaleDirection * Time.unscaledDeltaTime * 6f;
		else
			_timeScale += TimeScaleDirection * Time.unscaledDeltaTime;
		_timeScale = Mathf.Clamp(_timeScale, -10f, 1f);
	}

	private void TimeScaleSwitch()
	{
		if (_timeScale > 0f)
		{
			if (_firstPositiveTimeScale)
			{
				_firstPositiveTimeScale = false;
				FirstTimeForward(CurrentTime);
			}
			Time.timeScale = _timeScale;
			TimeForward(CurrentTime, Time.deltaTime);
		}
		else if (_timeScale < 0f)
		{
			if (!_firstPositiveTimeScale)
			{
				Time.timeScale = 0f;
				_firstPositiveTimeScale = true;
				FirstTimeReverse(CurrentTime);
			}
			TimeReverse(CurrentTime, _timeScale * Time.unscaledDeltaTime);
		}
	}	
}
