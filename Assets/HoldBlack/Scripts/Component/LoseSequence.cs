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
        GameTime gameTime = GameObject.Find("GameTime").GetComponent<GameTime>();

        gameTime.ZeroTime();
        Persistent.Get<EventSystem>().enabled = false;
		GetComponent<AudioSource>().Play();
		Camera.main.backgroundColor = Color.red;

		yield return StartCoroutine(CoroutineUtility.WaitForUnscaledTime(1.0f));

		GameObject.Find("InputPlane").GetComponent<InputPlane>().ResetFingers();
        gameTime.TimeScaleDirection = TimeScaleDirection;
		Camera.main.backgroundColor = Color.black;

        while (gameTime.CurrentTime > 0f)
        {
		    yield return null;
        }

        Persistent.Get<EventSystem>().enabled = true;
	}
}
