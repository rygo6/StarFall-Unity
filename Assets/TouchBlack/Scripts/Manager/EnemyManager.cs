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

	#endregion

	#region MonoBehaviour

	private void Start()
	{
		Time.timeScale = 0f;
		StartCoroutine(RandomInstantiate());
	}
		
	private void Update()
	{
		SmoothTimeScaleInDirection();
		TimeScaleSwitch();		
		time += timeScale;
	}

	#endregion

	#region Methods

	private IEnumerator RandomInstantiate()
	{
		while (true)
		{
			float xScalar = Random.Range(0f, 1f);
			InstantiateOrRetrieveEnemy(xScalar, enemyPrefabArray[0]);
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void CheckEnemyRetire()
	{
		float y = bottomPanelCornerArray[0].y;
		for (int i = 0; i < enemyList.Count; ++i)
		{
			if (enemyList[i].transform.position.y < y)
			{
				enemyList[i].gameObject.SetActive(false);
			}
		}	
	}
		
	private void InstantiateOrRetrieveEnemy(float xScalar, Enemy enemyPrefab)
	{
		int index = enemyList.FindIndex(item =>
		{
			if (!item.gameObject.activeSelf && item.GetType() == enemyPrefab.GetType())
			{
				return true;
			}
			return false;
		}
		);

		Vector3 position = new Vector3();
		position.y = (topPanelCornerArray[2].y + topPanelCornerArray[0].y) / 2f;
		position.x = Mathf.Lerp(topPanelCornerArray[0].x, topPanelCornerArray[2].x, xScalar);

		if (index == -1)
		{
			Enemy enemy = Instantiate(enemyPrefab, position, Quaternion.identity) as Enemy;
			enemyList.Add(enemy);
		}
		else
		{
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
//				DeleteEnemyHistory();
			}
			Time.timeScale = m_TimeScale;
			RecordEnemyHistory();
			CheckEnemyRetire();
		}
		else if (timeScale < 0f)
		{
			if (!m_FirstPositiveTimeScale)
			{
				m_FirstPositiveTimeScale = true;
			}
			Time.timeScale = 0f;
			EvaluateEnemyHistory();
		}
	}
	private bool m_FirstPositiveTimeScale = false;

	private void SmoothTimeScaleInDirection()
	{
		timeScale += timeScaleDirection * Time.unscaledDeltaTime;
		timeScale = Mathf.Clamp(timeScale, -1f, 1f);
	}
		
	private void EvaluateEnemyHistory()
	{
		for (int i = 0; i < enemyList.Count; ++i)
		{
			enemyList[i].EvaluateTransformHistory(time);
		}	
	}
		
	private void DeleteEnemyHistory()
	{
		for (int i = 0; i < enemyList.Count; ++i)
		{
			enemyList[i].DeleteHistoryAfterTime(time);
			StartCoroutine(enemyList[i].UpdatePhysicsToActualPosition());
		}	
	}
		
	private void RecordEnemyHistory()
	{
		const float recordTimeSpacing = .5f;
		if (timeScale > 0f)
		{
			m_PositiveDelta += timeScale;
		}
		if (m_PositiveDelta > recordTimeSpacing)
		{
			m_PositiveDelta = 0f;
			//TODO may be smarter to make this be set iteratively over multiple frames with a coroutine
			for (int i = 0; i < m_EnemyList.Count; ++i)
			{
				enemyList[i].RecordTransformHistory(time);
			}
		}
	}
	private float m_PositiveDelta;

	#endregion

}
