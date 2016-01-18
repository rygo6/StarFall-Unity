using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour 
{
	[SerializeField]
	private Enemy[] _enemyPrefabArray;

	private BoxCollider2D _boxCollider2D;

	private EnemyPool _enemyPool;

	private void Start()
	{
		_boxCollider2D = GetComponent<BoxCollider2D>();
		StartCoroutine(RandomInstantiate());
		_enemyPool = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<EnemyPool>();
	}

	private IEnumerator RandomInstantiate()
	{
		yield return new WaitForSeconds(.2f);
		while (true)
		{
			Enemy enemy = _enemyPool.InstantiateOrRetrieveRetiredEnemy(_enemyPrefabArray[0]);
			float xScalar = Random.Range(0f, 1f);
			Vector3 newPos = new Vector3();
			newPos.y = transform.position.y;
			newPos.x = Mathf.Lerp(_boxCollider2D.bounds.min.x, _boxCollider2D.bounds.max.x, xScalar);
			enemy.transform.position = newPos;
			yield return new WaitForSeconds(.1f);
		}
	}
}
