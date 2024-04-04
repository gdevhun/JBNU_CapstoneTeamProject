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

		// SFX 딕셔너리 초기화
		_sfxDictionary = _sfxs.ToDictionary(s => s.SoundType, s => s.Clip);
	}

	private void Start()
	{
		_bgmPlayer = gameObject.AddComponent<AudioSource>();
		// SFX 플레이어 몇 개를 초기에 생성하고 리스트에 추가
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
			sfxPlayer.PlayDelayed(delay); // delay초 후에 재생
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
			// 새 플레이어를 생성하고 리스트에 추가
			AudioSource newSFXPlayer = gameObject.AddComponent<AudioSource>();
			SfxPlayers.Add(newSFXPlayer);
			return newSFXPlayer;
		}
	}

	// 사운드 플레이가 끝나면 호출하여 큐에 다시 넣기
	public void ReturnSFXPlayerToQueue(AudioSource sfxPlayer)
	{
		_sfxQueue.Enqueue(sfxPlayer);
	}
}
public enum SoundType
{
	MenuBGM = 0,
	FirstBGM = 1,
	SecondBGM = 2,

	PlayerWalkSFX = 3,
	PlayerSwingSFX = 4,
	PlayerSwingSFX2 = 5,
	PlayerCounterSFX3 = 6,
	PlayerJumpSFX = 7,
	PlayerCutSFX = 8,
	PlayerHitSFX = 9,
	PlayerHitSFX2 = 10,
	PlayerElecSFX = 11,
	PlayerElecSFX2 = 12,
	PlayerUsePotion = 13,
	PlayerUseItem = 14,
	PlayerGetItem = 15,
	PlayerBubbleSFX = 16,
	PlayerInventoryOpen = 17,
	PlayerUserItem2 = 18,
	EnemyHit1 = 19,
	EnemyHit2 = 20,
	EnmeySfx = 21,
	EnemySfx2 = 22,
	EnemySfx3 = 23
}
