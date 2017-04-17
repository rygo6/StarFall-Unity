using UnityEngine;
using System.Collections;

public class RewindAnimator : MonoBehaviour 
{
	Animator _animator;

	void Awake()
	{
		_animator = GetComponent<Animator>();
		GameTime gameTime = FindObjectOfType<GameTime>();
		gameTime.TimeReverse += Rewind;
	}

	void Rewind(float currentTime, float deltaTime)
	{
		_animator.SetTime(currentTime);
	}
}
