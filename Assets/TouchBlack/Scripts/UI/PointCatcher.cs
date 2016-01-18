using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class PointCatcher : MonoBehaviour 
{
	private AudioSource[] _coinAudioSources;

	private AudioSource[] _coinReverseAudioSources;

	private int CurrentAudioSourceIndex
	{
		get { return _currentAudioSourceIndex; }
		set
		{
			_currentAudioSourceIndex = (int)Mathf.Repeat(value, _coinAudioSources.Length);
		}
	}
	private int _currentAudioSourceIndex;

	private int CurrentReverseAudioSourceIndex
	{
		get { return _currentAudioSourceIndex; }
		set
		{
			_currentAudioSourceIndex = (int)Mathf.Repeat(value, _coinReverseAudioSources.Length);
		}
	}
	private int _currentReverseAudioSourceIndex;

	[SerializeField]
	private AudioMixerGroup _coinUpAudioMixerGroup;

	[SerializeField]
	private AudioClip _coinUpAudioClip;

	[SerializeField]
	private AudioClip _coinReverseAudioClip;

	private AnimationCurve _scoreCurve = new AnimationCurve();

	private GameTime _gameTime;

	[SerializeField]
	private Text _countText;

	[SerializeField]
	private Image _catchPanel;

	private int _count;

	private Color _pulseColor = new Color(1f, 1f, 1f, 0f);

	private Color _textPulseColor = new Color(1f, 1f, 1f, .3f);

	private void Awake()
	{
		_coinAudioSources = new AudioSource[10];
		_coinReverseAudioSources = new AudioSource[10];
		for (int i = 0; i < _coinAudioSources.Length; ++i)
		{
			_coinAudioSources[i] = gameObject.AddComponent<AudioSource>();
			_coinAudioSources[i].playOnAwake = false;
			_coinAudioSources[i].clip = _coinUpAudioClip;
			_coinAudioSources[i].outputAudioMixerGroup = _coinUpAudioMixerGroup;

			_coinReverseAudioSources[i] = gameObject.AddComponent<AudioSource>();
			_coinReverseAudioSources[i].playOnAwake = false;
			_coinReverseAudioSources[i].clip = _coinReverseAudioClip;
			_coinReverseAudioSources[i].outputAudioMixerGroup = _coinUpAudioMixerGroup;
		}

		_gameTime = GameObject.FindGameObjectWithTag("GameTime").GetComponent<GameTime>();
		_gameTime.TimeReverse += Rewind;

		_catchPanel.color = _pulseColor;
		_countText.color = _textPulseColor;
	}

	private void Update()
	{
		if (_pulseColor.a > 0f)
		{
			_pulseColor.a -= Time.deltaTime * 4f;
			_catchPanel.color = _pulseColor;
		}

		if (_textPulseColor.a > .3f)
		{
			_textPulseColor.a -= Time.deltaTime;
			_countText.color = _textPulseColor;
		}
	}

	public void Rewind(float time, float deltaTime)
	{
		int rewindCount = Mathf.FloorToInt(_scoreCurve.Evaluate(time));
		if (rewindCount != _count)
		{
			_count = rewindCount;
			_countText.text = _count.ToString();
			_scoreCurve.RemoveAfterTime(time);
			_coinReverseAudioSources[CurrentReverseAudioSourceIndex++].Play();
		}
	}

	public void OnTriggerEnter2D(Collider2D other) 
	{
		_pulseColor.a = 1f;
		_textPulseColor.a = .6f;
		++_count;
		_countText.text = _count.ToString();
		_coinAudioSources[CurrentAudioSourceIndex++].Play();
		_scoreCurve.AddKey(_gameTime.CurrentTime, _count);
	}
}
