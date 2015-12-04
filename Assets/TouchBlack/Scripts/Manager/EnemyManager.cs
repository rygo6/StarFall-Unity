using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : Manager<EnemyManager>
{
	[Header("Panel from which enemies are spawned.")]
	[SerializeField]
	private RectTransform _topPanelRectTransform;

	private Vector3[] TopPanelCornerArray
	{
		get
		{ 
			if (_topPanelCornerArray == null)
			{
				_topPanelCornerArray = new Vector3[4];
				_topPanelRectTransform.GetWorldCorners(_topPanelCornerArray);
			}
			return _topPanelCornerArray; 
		}
	}
	private Vector3[] _topPanelCornerArray;

	[Header("Panel which enemies retire into.")]
	[SerializeField]
	private RectTransform _bottomPanelRectTransform;

	private Vector3[] BottomPanelCornerArray
	{
		get
		{ 
			if (_bottomPanelCornerArray == null)
			{
				_bottomPanelCornerArray = new Vector3[4];
				_bottomPanelRectTransform.GetWorldCorners(_bottomPanelCornerArray);
			}
			return _bottomPanelCornerArray; 
		}
	}
	private Vector3[] _bottomPanelCornerArray;

	public float TimeScaleDirection { get; set; }

	private float CurrentTime
	{ 
		get { return _currentTime; } 
		set
		{ 
			if (value < 0)
				_currentTime = 0;
			else
				_currentTime = value;
		} 
	}
	private float _currentTime;

	private float _timeScale;

	[SerializeField]
	private List<Enemy> _enemyList = new List<Enemy>();

	[SerializeField]
	private Enemy[] _enemyPrefabArray;

	[SerializeField]
	private float _recordTimeSpacing = .2f;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		Time.timeScale = 0f;
		StartCoroutine(RandomInstantiate());
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
		CurrentTime += _timeScale * Time.unscaledDeltaTime;
		TimeScaleSwitch();		
	}

	private IEnumerator RandomInstantiate()
	{
		while (true)
		{
			float xScalar = Random.Range(0f, 1f);
			InstantiateOrRetrieveRetiredEnemy(xScalar, _enemyPrefabArray[0]);
			yield return new WaitForSeconds(.1f);
		}
	}

	private void CheckEnemyRetire()
	{
		float y = BottomPanelCornerArray[0].y;
		for (int i = 0; i < _enemyList.Count; ++i)
		{
			if (_enemyList[i].transform.position.y < y &&
			    _enemyList[i].retireState == Enemy.RetireState.NotRetired)
			{
				_enemyList[i].retireState = Enemy.RetireState.JustRetired;
				_enemyList[i].gameObject.SetActive(false);
			}
		}	
	}
		
	private void InstantiateOrRetrieveRetiredEnemy(float xScalar, Enemy enemyPrefab)
	{
		int index = _enemyList.FindIndex(item =>
		{
			if (item.GetType() == enemyPrefab.GetType() &&
			    item.retireState == Enemy.RetireState.Retired)
			{
				return true;
			}
			return false;
		});

		Vector3 position = new Vector3();
		position.y = TopPanelCornerArray[2].y;
		position.x = Mathf.Lerp(TopPanelCornerArray[0].x, TopPanelCornerArray[2].x, xScalar);

		if (index == -1)
		{
			Enemy enemy = Instantiate(enemyPrefab, position, Quaternion.identity) as Enemy;
			_enemyList.Add(enemy);
		}
		else
		{
			_enemyList[index].retireState = Enemy.RetireState.JustUnRetired;
			_enemyList[index].transform.position = position;
			_enemyList[index].gameObject.SetActive(true);
		}
	}
		
	private void TimeScaleSwitch()
	{
		if (_timeScale > 0f)
		{
			if (_firstPositiveTimeScale)
			{
				_firstPositiveTimeScale = false;
				RemoveHistoryAfterTime(CurrentTime);
			}
			Time.timeScale = _timeScale;
			RecordEnemyHistory();
			CheckEnemyRetire();
		}
		else if (_timeScale < 0f)
		{
			if (!_firstPositiveTimeScale)
			{
				Time.timeScale = 0f;
				_firstPositiveTimeScale = true;
				//then add a key at immediate spot
				//to allow smooth evaluation from this point backward
				AddHistoryKey(CurrentTime);
			}
			EvaluateEnemyHistory();
			RewindEnemyHistory();
		}
	}
	private bool _firstPositiveTimeScale = false;

	private void SmoothTimeScaleInDirection()
	{
		//return from rewind faster
		if (TimeScaleDirection > 0f && _timeScale < 0f)
			_timeScale += TimeScaleDirection * Time.unscaledDeltaTime * 6f;
		else
			_timeScale += TimeScaleDirection * Time.unscaledDeltaTime;
		_timeScale = Mathf.Clamp(_timeScale, -10f, 1f);
	}
		
	private void EvaluateEnemyHistory()
	{
		for (int i = 0; i < _enemyList.Count; ++i)
		{
			_enemyList[i].EvaluateTransformHistory(CurrentTime);
		}	
	}
		
	private void RewindEnemyHistory()
	{
		m_NegativeDelta -= _timeScale * Time.unscaledDeltaTime;
		if (m_NegativeDelta > _recordTimeSpacing)
		{
			m_NegativeDelta = 0f;
			//recordTimeSpacing * 2f to ensure it does not delete
			//keyframe it currently needs to smoothly interpolate on
			RemoveHistoryAfterTime(CurrentTime + (_recordTimeSpacing * 2f));
		}
	}
	private float m_NegativeDelta;

	private void RemoveHistoryAfterTime(float time)
	{
		for (int i = 0; i < _enemyList.Count; ++i)
		{
			_enemyList[i].RemoveHistoryAfterTime(time);
		}	
	}
		
	private void RecordEnemyHistory()
	{
		m_PositiveDelta += Time.deltaTime;
		if (m_PositiveDelta > _recordTimeSpacing)
		{
			m_PositiveDelta = 0f;
			AddHistoryKey(CurrentTime);
		}
	}
	private float m_PositiveDelta;

	private void AddHistoryKey(float time)
	{
		for (int i = 0; i < _enemyList.Count; ++i)
		{
			_enemyList[i].RecordHistory(time);
		}
	}

	private float RoundToDecimal(float value, float round)
	{
		round = 1f / round;
		return Mathf.Round(value * round) / round;
	}
}