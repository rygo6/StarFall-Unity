using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
	public readonly List<Enemy> EnemyList = new List<Enemy>();

	[SerializeField]
	private float _recordTimeSpacing = .2f;

	private float _positiveDelta;

	private float _negativeDelta;

	private void Awake()
	{
		GameTime gameTime = FindObjectOfType<GameTime>();
		gameTime.FirstTimeForward += RemoveHistoryAfterTime;
		gameTime.TimeForward += RecordEnemyHistory;
		gameTime.FirstTimeReverse += FirstRewind;
		gameTime.TimeReverse += Rewind;
	}

	public Enemy InstantiateOrRetrieveRetiredEnemy(Enemy enemyPrefab)
	{
		int index = EnemyList.FindIndex(item =>
		{
			if (item.GetType() == enemyPrefab.GetType() &&
				item.RetireState == RetireState.Retired)
			{
				return true;
			}
			return false;
		});

		if (index == -1)
		{
			Enemy enemy = Instantiate(enemyPrefab) as Enemy;
			EnemyList.Add(enemy);
			return enemy;
		}
		else
		{
			EnemyList[index].RetireState = RetireState.JustUnRetired;
			EnemyList[index].gameObject.SetActive(true);
			return EnemyList[index];
		}
	}

	private void FirstRewind(float currentTime)
	{
		//then add a key at immediate spot
		//to allow smooth evaluation from this point backward
		//last key is deleted by chance the new key is placed too close
		//to it resulting in unideal interpolation between the two
		RemoveHistoryAfterTime(currentTime - (_recordTimeSpacing / 4f));
		AddHistoryKey(currentTime);
	}
		
	private void Rewind(float currentTime, float deltaTime)
	{
		EvaluateEnemyHistory(currentTime);
		RewindEnemyHistory(currentTime, deltaTime);
	}

	private void EvaluateEnemyHistory(float currentTime)
	{
		for (int i = 0; i < EnemyList.Count; ++i)
		{
			EnemyList[i].EvaluateTransformHistory(currentTime);
		}	
	}
		
	private void RewindEnemyHistory(float currentTime, float deltaTime)
	{
		_negativeDelta -= deltaTime;
		if (_negativeDelta > _recordTimeSpacing)
		{
			_negativeDelta = 0f;
			//recordTimeSpacing * 2f to ensure it does not delete
			//keyframe it currently needs to smoothly interpolate on
			RemoveHistoryAfterTime(currentTime + (_recordTimeSpacing * 2f));
		}
	}

	private void RemoveHistoryAfterTime(float time)
	{
		for (int i = 0; i < EnemyList.Count; ++i)
		{
			EnemyList[i].RemoveHistoryAfterTime(time);
		}	
	}
		
	private void RecordEnemyHistory(float currentTime, float deltaTime)
	{
		_positiveDelta += deltaTime;
		if (_positiveDelta > _recordTimeSpacing)
		{
			_positiveDelta = 0f;
			AddHistoryKey(currentTime);
		}
	}

	private void AddHistoryKey(float time)
	{
		for (int i = 0; i < EnemyList.Count; ++i)
		{
			EnemyList[i].RecordHistory(time);
		}
	}
}