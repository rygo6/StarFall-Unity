using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using EC;

public class LoseSequence : MonoBehaviour 
{
	private const float TimeScaleDirection = -10;

	public void Play()
	{
		StartCoroutine(PlayCoroutine());
	}

	private IEnumerator PlayCoroutine()
	{
		GameObject.Find("GameTime").GetComponent<GameTime>().ZeroTime();
		GetComponent<AudioSource>().Play();
		Camera.main.backgroundColor = Color.red;

		yield return StartCoroutine(CoroutineUtility.WaitForUnscaledTime(1.0f));

		GameObject.Find("InputPlane").GetComponent<InputPlane>().ResetFingers();
		GameObject.Find("GameTime").GetComponent<GameTime>().TimeScaleDirection = TimeScaleDirection;
		Camera.main.backgroundColor = Color.black;

		yield return null;
	}
}
