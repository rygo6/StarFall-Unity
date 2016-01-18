using UnityEngine;
using System.Collections;

public class Finger : MonoBehaviour 
{
	private Rigidbody2D _rigidBody2D;

	private void Awake()
	{
		_rigidBody2D = GetComponent<Rigidbody2D>();
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == TagUtility.EnemyTag)
		{
			GameObject.Find("LoseSequence").GetComponent<LoseSequence>().Play();
		}
	}

	public void MovePosition(Vector3 position)
	{
		_rigidBody2D.MovePosition(position);
	}
}
