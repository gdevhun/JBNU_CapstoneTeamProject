using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : SingletonBehaviour<SoundManager>
{
	[System.Serializable]
	public class Sound
	{
		public SoundType SoundType;
		public AudioClip Clip;
	}

	[SerializeField]
	private List<Sound> _bgms = null;
	[SerializeField]
	private List<Sound> _sfxs = null;
	[SerializeField]
	private AudioSource _bgmPlayer = null;
	public List<AudioSource> SfxPlayers = new List<AudioSource>();

	private Dictionary<SoundType, AudioClip> _sfxDictionary;

	private Queue<AudioSource> _sfxQueue = new Queue<AudioSource>();

	protected override void Awake()
	{
		base.Awake();

		// SFX ��ųʸ� �ʱ�ȭ
		_sfxDictionary = _sfxs.ToDictionary(s => s.SoundType, s => s.Clip);
	}

	private void Start()
	{
		_bgmPlayer = gameObject.AddComponent<AudioSource>();
		// SFX �÷��̾� �� ���� �ʱ⿡ �����ϰ� ����Ʈ�� �߰�
		for (int i = 0; i < 20; i++)
		{
			AudioSource sfxPlayer = gameObject.AddComponent<AudioSource>();
			SfxPlayers.Add(sfxPlayer);
			_sfxQueue.Enqueue(sfxPlayer);
		}
	}

	public void PlayBGM(SoundType soundType)
	{
		var bgm = _bgms.First(b => b.SoundType == soundType);
		_bgmPlayer.clip = bgm.Clip;
		_bgmPlayer.loop = true;
		_bgmPlayer.Play();
	}

	public void StopBGM()
	{
		_bgmPlayer.Stop();
	}

	public void PlaySFX(SoundType soundType, float volume = 1.0f)
	{
		if (_sfxDictionary.TryGetValue(soundType, out AudioClip clip))
		{
			AudioSource sfxPlayer = GetAvailableSFXPlayer();
			sfxPlayer.clip = clip;
			sfxPlayer.volume = volume;
			sfxPlayer.Play();
		}
	}

	public void PlaySFX(SoundType soundType, float volume = 1.0f, float delay = 1.0f)
	{
		if (_sfxDictionary.TryGetValue(soundType, out AudioClip clip))
		{
			AudioSource sfxPlayer = GetAvailableSFXPlayer();
			sfxPlayer.clip = clip;
			sfxPlayer.volume = volume;
			sfxPlayer.PlayDelayed(delay); // delay�� �Ŀ� ���
		}
	}

	private AudioSource GetAvailableSFXPlayer()
	{
		if (_sfxQueue.Count > 0)
		{
			return _sfxQueue.Dequeue();
		}
		else
		{
			// �� �÷��̾ �����ϰ� ����Ʈ�� �߰�
			AudioSource newSFXPlayer = gameObject.AddComponent<AudioSource>();
			SfxPlayers.Add(newSFXPlayer);
			return newSFXPlayer;
		}
	}

	// ���� �÷��̰� ������ ȣ���Ͽ� ť�� �ٽ� �ֱ�
	public void ReturnSFXPlayerToQueue(AudioSource sfxPlayer)
	{
		_sfxQueue.Enqueue(sfxPlayer);
	}
}
public enum SoundType
{
	아쳐타워123화살, 아쳐타워4화살,
	매직타워불, 매직타워얼음, 매직타워전기, 매직타워시간,
	스톤타워불, 스톤타워돌
}
