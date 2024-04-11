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

		// 효과음보다 커서 0.5로 조절
		_bgmPlayer.volume = 0.3f;

		// SFX �÷��̾� �� ���� �ʱ⿡ �����ϰ� ����Ʈ�� �߰�
		for (int i = 0; i < 20; i++)
		{
			AudioSource sfxPlayer = gameObject.AddComponent<AudioSource>();
			SfxPlayers.Add(sfxPlayer);
			_sfxQueue.Enqueue(sfxPlayer);
		}

		// 처음은 메뉴 BGM 재생
		// 게임씬으로 가면 일반 BGM 재생
		// 보스가 나오면 각 보스 BGM 재생
		PlayBGM(SoundType.메뉴BGM);
	}

	// 배경음 테스트
	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayBGM(SoundType.메뉴BGM);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayBGM(SoundType.일반BGM);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayBGM(SoundType.보스BGM1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayBGM(SoundType.보스BGM2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayBGM(SoundType.보스BGM3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlayBGM(SoundType.보스BGM4);
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

			// 사운드 재생이 끝나면 풀에 반환
			StartCoroutine(ReturnSFXPlayerWhenFinished(sfxPlayer, clip.length));
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

	// 사운드 재생이 끝나면 풀에 반환
	private IEnumerator ReturnSFXPlayerWhenFinished(AudioSource sfxPlayer, float delay)
	{
		// 사운드 재생 시간만큼 대기
		yield return new WaitForSeconds(delay);

		// 사운드 재생이 끝났으니 큐에 반환
		ReturnSFXPlayerToQueue(sfxPlayer);
	}
}
public enum SoundType
{
	아쳐타워123화살, 아쳐타워4화살,
	매직타워불, 매직타워얼음, 매직타워전기, 매직타워시간,
	스톤타워불, 스톤타워돌,
	메뉴BGM, 일반BGM, 보스BGM1, 보스BGM2, 보스BGM3, 보스BGM4
}
