using UnityEngine;
using System.Collections;

public class RewindAnimator : MonoBehaviour 
{
	private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		GameTime gameTime = GameObject.FindGameObjectWithTag("GameTime").GetComponent<GameTime>();
		gameTime.TimeReverse += Rewind;
	}

	private void Rewind(float currentTime, float deltaTime)
	{
		_animator.SetTime(currentTime);
	}
}
