using UnityEngine;
using System.Collections;

public class EnemyGrave : MonoBehaviour 
{
	public void OnTriggerEnter2D(Collider2D other) 
	{
		const string enemyTag = "Enemy";
		if (other.CompareTag(enemyTag))
		{
			other.GetComponent<Enemy>().RetireState = RetireState.JustRetired;
			other.gameObject.SetActive(false);
		}
	}
}
