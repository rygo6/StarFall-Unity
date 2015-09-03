using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : Manager<EnemyManager>
{

	#region Properties

	private RectTransform topPanelRectTransform
	{
		get { return m_TopPanelRectTransform; }
	}
	[Header("Panel from which enemies are spawned.")]
	[SerializeField]
	private RectTransform m_TopPanelRectTransform;

	private Vector3[] topPanelCornerArray
	{
		get
		{ 
			if (m_TopPanelCornerArray == null)
			{
				m_TopPanelCornerArray = new Vector3[4];
				topPanelRectTransform.GetWorldCorners(m_TopPanelCornerArray);
			}
			return m_TopPanelCornerArray; 
		}
	}
	private Vector3[] m_TopPanelCornerArray;

	private RectTransform bottomPanelRectTransform
	{
		get { return m_BottomPanelRectTransform; }
	}
	[Header("Panel which enemies retire into.")]
	[SerializeField]
	private RectTransform m_BottomPanelRectTransform;

	private Vector3[] bottomPanelCornerArray
	{
		get
		{ 
			if (m_BottomPanelCornerArray == null)
			{
				m_BottomPanelCornerArray = new Vector3[4];
				bottomPanelRectTransform.GetWorldCorners(m_BottomPanelCornerArray);
			}
			return m_BottomPanelCornerArray; 
		}
	}
	private Vector3[] m_BottomPanelCornerArray;

	public float timeScaleDirection
	{ 
		get { return m_TimeScaleDirection; } 
		set { m_TimeScaleDirection = value; } 
	}
	private float m_TimeScaleDirection;

	private float time
	{ 
		get { return m_Time; } 
		set
		{ 
			if (value < 0f)
			{
				m_Time = 0f;
			}
			else
			{
				m_Time = value; 
			}
		} 
	}
	private float m_Time;

	private float timeScale
	{ 
		get { return m_TimeScale; } 
		set { m_TimeScale = value; } 
	}
	private float m_TimeScale;

	private List<Enemy> enemyList
	{ 
		get { return m_EnemyList; } 
	}
	[SerializeField]
	private List<Enemy> m_EnemyList = new List<Enemy>();

	private Enemy[] enemyPrefabArray
	{ 
		get { return m_EnemyPrefabArray; } 
	}
	[SerializeField]
	private Enemy[] m_EnemyPrefabArray;

	private float recordTimeSpacing
	{ 
		get { return m_RecordTimeSpacing; } 
	}
	[SerializeField]
	private float m_RecordTimeSpacing = .2f;

	#endregion

	#region MonoBehaviour

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
		time = 0f;
	}
		
	private void Update()
	{
		SmoothTimeScaleInDirection();

		time += timeScale * Time.unscaledDeltaTime;

		TimeScaleSwitch();		
	}

	#endregion

	#region Methods

	private IEnumerator RandomInstantiate()
	{
		while (true)
		{
			float xScalar = Random.Range(0f, 1f);
			InstantiateOrRetrieveRetiredEnemy(xScalar, enemyPrefabArray[0]);
			yield return new WaitForSeconds(.1f);
		}
	}

	private void CheckEnemyRetire()
	{
		float y = bottomPanelCornerArray[0].y;
		for (int i = 0; i < enemyList.Count; ++i)
		{
			if (enemyList[i].transform.position.y < y &&
			    enemyList[i].retireState == Enemy.RetireState.NotRetired)
			{
				enemyList[i].retireState = Enemy.RetireState.JustRetired;
				enemyList[i].gameObject.SetActive(false);
			}
		}	
	}
		
	private void InstantiateOrRetrieveRetiredEnemy(float xScalar, Enemy enemyPrefab)
	{
		int index = enemyList.FindIndex(item =>
		{
			if (item.GetType() == enemyPrefab.GetType() &&
			    item.retireState == Enemy.RetireState.Retired)
			{
				return true;
			}
			return false;
		});

		Vector3 position = new Vector3();
		position.y = topPanelCornerArray[2].y;
		position.x = Mathf.Lerp(topPanelCornerArray[0].x, topPanelCornerArray[2].x, xScalar);

		if (index == -1)
		{
			Enemy enemy = Instantiate(enemyPrefab, position, Quaternion.identity) as Enemy;
			enemyList.Add(enemy);
		}
		else
		{
			enemyList[index].retireState = Enemy.RetireState.JustUnRetired;
			enemyList[index].transform.position = position;
			enemyList[index].gameObject.SetActive(true);
		}
	}
		
	private void TimeScaleSwitch()
	{
		if (timeScale > 0f)
		{
			if (m_FirstPositiveTimeScale)
			{
				m_FirstPositiveTimeScale = false;
				RemoveHistoryAfterTime(time);
			}
			Time.timeScale = m_TimeScale;
			RecordEnemyHistory();
			CheckEnemyRetire();
		}
		else if (timeScale < 0f)
		{
			if (!m_FirstPositiveTimeScale)
			{
				Time.timeScale = 0f;
				m_FirstPositiveTimeScale = true;
				//delete last key, then add a key at immediate spot
				//to allow smooth evaluation from this point backward
				//last key is deleted by chance the new key is placed too close
				//to it resulting in unideal interpolation between the two
				RemoveHistoryAfterTime(time - recordTimeSpacing);
				AddHistoryKey(time);
			}
			EvaluateEnemyHistory();
			RewindEnemyHistory();
		}
	}
	private bool m_FirstPositiveTimeScale = false;

	private void SmoothTimeScaleInDirection()
	{
		timeScale += timeScaleDirection * Time.unscaledDeltaTime;
		timeScale = Mathf.Clamp(timeScale, -10f, 1f);
	}
		
	private void EvaluateEnemyHistory()
	{
		for (int i = 0; i < enemyList.Count; ++i)
		{
			enemyList[i].EvaluateTransformHistory(time);
		}	
	}
		
	private void RewindEnemyHistory()
	{
		m_NegativeDelta -= timeScale * Time.unscaledDeltaTime;
		if (m_NegativeDelta > recordTimeSpacing)
		{
			m_NegativeDelta = 0f;
			//recordTimeSpacing * 2f to ensure it does not delete
			//keyframe it currently needs to smoothly interpolate on
			RemoveHistoryAfterTime(time + (recordTimeSpacing * 2f));
		}
	}
	private float m_NegativeDelta;

	private void RemoveHistoryAfterTime(float time)
	{
		for (int i = 0; i < enemyList.Count; ++i)
		{
			enemyList[i].RemoveHistoryAfterTime(time);
		}	
	}
		
	private void RecordEnemyHistory()
	{
		m_PositiveDelta += Time.deltaTime;
		if (m_PositiveDelta > recordTimeSpacing)
		{
			m_PositiveDelta = 0f;
			AddHistoryKey(time);
		}
	}
	private float m_PositiveDelta;

	private void AddHistoryKey(float time)
	{
		for (int i = 0; i < m_EnemyList.Count; ++i)
		{
			enemyList[i].RecordHistory(time);
		}
	}

	private float RoundToDecimal(float value, float round)
	{
		round = 1f / round;
		return Mathf.Round(value * round) / round;
	}

	#endregion

}
