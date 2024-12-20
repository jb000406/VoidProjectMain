using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace VoidProject
{

    [System.Serializable]
    public class AudioClipInfo
    {
        public AudioSource audioSource;
        public AudioClip audioClip;
        public Vector3 playPosition;
    }

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        [Header("오디오 리스너 확인용")]
        [SerializeField] private AudioListener _AudioListener;  // 전체 오디오 소스 리스트
        [Header("오디오 소스 관리")]
        [SerializeField] private List<AudioSource> _AudioSource;  // 전체 오디오 소스 리스트
        [Header("재생 중인 오디오 클립 정보 디버그용")]
        [SerializeField] private List<AudioClipInfo> _UsedClip;   // 재생 중인 오디오 클립 정보
        [Header("오디오 풀링 관리 목록")]
        [SerializeField] private Queue<AudioSource> _AudioUnusedQueue = new Queue<AudioSource>();  // 재사용 가능한 오디오 소스 큐
        [Header("오디오 클립 목록")]
        [SerializeField] private AudioClip[] _AudioClip;
        [Header("MainMusic 목록")]
        [SerializeField] private AudioClip[] _MainMusicClip;
        [Header("믹서그룹 설정")]
        [SerializeField] public AudioMixerGroup _MusicEffect;
        [SerializeField] public AudioMixerGroup _SoundEffect;
        [Header("메인음악 플레이 플래그")]
        [SerializeField] private bool _isPlayMainMusic = false;
        [Header("초기 AudioSource 풀 크기")]
        [SerializeField] private int initialPoolSize = 10;

        public static List<AudioClipInfo> GetPlayList() => Instance._UsedClip;
        public static AudioClip GetAudioClipByIndex(int index)
        {
            return Instance._AudioClip[index];
        }

        public bool IsMainMusicPlay
        {
            get { return _isPlayMainMusic; }
            set { _isPlayMainMusic = value; }
        }

        private AudioSource MainMusicSource;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // AudioListener가 인스펙터에서 설정되지 않았으면 자동 할당
            if (_AudioListener == null)
            {
                _AudioListener = Camera.main.gameObject.GetComponent<AudioListener>();
                if (_AudioListener == null)
                {
                    Debug.LogError("AudioListener가 씬에 없습니다. AudioListener를 추가하세요.");
                }
            }

            InitailizeAudioSource();
            InitailizeMusicSource();
            _UsedClip = new List<AudioClipInfo>();
            Invoke(nameof(PlayMainMusic), 1f);
        }

        private void Update()
        {
            PlayingCheck();
        }

        private void PlayMainMusic()
        {
            _isPlayMainMusic = true;
        }

        // 백그라운드 뮤직 소스 초기화
        private void InitailizeMusicSource()
        {
            MainMusicSource = this.gameObject.AddComponent<AudioSource>(); ;
            MainMusicSource.loop = true;
            MainMusicSource.volume = 0.8f;
            MainMusicSource.mute = false;
            MainMusicSource.playOnAwake = false;
            MainMusicSource.priority = 100;
            MainMusicSource.outputAudioMixerGroup = _MusicEffect;
            _isPlayMainMusic = false;
        }

        // 오디오 소스 초기화
        private void InitailizeAudioSource()
        {
            _AudioSource = new List<AudioSource>();

            // 30개의 오디오 소스를 미리 생성하여 풀링한다.
            while (_AudioSource.Count < initialPoolSize)
            {
                AudioSource audioSource = AudioSourceAdd();
                // 기본 사운드 설정
                audioSource.playOnAwake = false;         // 자동 재생 비활성화
                audioSource.loop = false;                // 반복 재생 활성화
                audioSource.volume = 0.8f;              // 기본 볼륨 설정
                audioSource.pitch = 1f;                 // 기본 음높이 설정
                audioSource.spatialBlend = 1f;          // 3D 사운드로 설정 (0 = 2D, 1 = 3D)
                audioSource.dopplerLevel = 1f;          // 도플러 효과 기본값
                audioSource.spread = 0f;                // 소리의 확산 각도 (0: 집중, 360: 전방위)

                // Rolloff 설정 (Logarithmic Rolloff 권장)
                audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
                audioSource.minDistance = 1f;           // 최대 음량이 들리는 최소 거리
                audioSource.maxDistance = 500f;         // 소리가 완전히 사라지는 최대 거리

                // 출력 경로 설정 (Audio Mixer 연결)
                // Mixer는 사전에 Unity Editor에서 설정된 Audio Mixer Group이어야 함
                audioSource.outputAudioMixerGroup = _SoundEffect;
            }
            // 미사용 큐에 오디오 소스 추가
            _AudioUnusedQueue = new Queue<AudioSource>(_AudioSource);
        }

        private void UpdateAudioListenerPosition()
        {
            if (_AudioListener != null && Camera.main != null)
            {
                _AudioListener.transform.position = Camera.main.transform.position;
                _AudioListener.transform.rotation = Camera.main.transform.rotation;
            }
        }

        // 필요할 때만 호출 (예: VR 환경에서)
        private void PlayingCheck()
        {
            UpdateAudioListenerPosition();
            // 메인음악 플래이 플래그가 true고 메인음악클립이 있을때 음악연주
            if (_isPlayMainMusic && !MainMusicSource.isPlaying && _MainMusicClip.Length > 0)
            {
                //MainMusicSource.clip = _MainMusicClip[Random.Range(0, _MainMusicClip.Length)];
                // 테스트중일때 0번만 재생
                MainMusicSource.clip = _MainMusicClip[0];
                MainMusicSource.Play();
            }
            else if (!_isPlayMainMusic && MainMusicSource.isPlaying)
            {
                MainMusicSource.Stop();
            }

            for (int i = _UsedClip.Count - 1; i >= 0; i--)
            {
                AudioClipInfo clipInfo = _UsedClip[i];
                if (!clipInfo.audioSource.isPlaying || clipInfo.audioSource.clip == null)
                {
                    ReturnSourceToQueue(clipInfo.audioSource);
                    _UsedClip.RemoveAt(i);  // 재생 중인 클립 목록에서 제거
                }
            }
        }

        // 사용 가능한 오디오 소스 반환 (큐에서 꺼내 사용)
        public AudioSource GetAvailableSource()
        {
            AudioSource newSource = _AudioUnusedQueue.Count > 0 ? _AudioUnusedQueue.Dequeue() : AudioSourceAdd();
            newSource.enabled = true;  // 오디오 소스 활성화
            return newSource;
        }

        // 오디오 소스를 풀에 반환 (큐에 다시 추가하여 재사용 가능하게 만듬)
        private void ReturnSourceToQueue(AudioSource source)
        {
            source.Stop();  // 재생 중지
            source.clip = null;  // 클립 정보 제거
            source.enabled = false;  // 오디오 소스 비활성화
            _AudioUnusedQueue.Enqueue(source);  // 큐에 다시 추가
        }

        // 새로운 오디오 소스를 추가하여 리스트와 큐에 저장
        private AudioSource AudioSourceAdd()
        {
            GameObject _obj = transform.GetChild(0).gameObject;
            AudioSource newSource = _obj.AddComponent<AudioSource>();
            newSource.enabled = false;
            _AudioSource.Add(newSource);  // 전체 오디오 소스 리스트에 추가
            return newSource;
        }

        // 클립을 재생하는 함수
        public void PlayClip(AudioClip clip, float speed)
        {
            AudioClipInfo newUsed = new AudioClipInfo
            {
                audioSource = GetAvailableSource(),
                audioClip = clip
            };
            newUsed.audioSource.clip = clip;
            newUsed.audioSource.pitch = speed;  // 재생 속도 설정
            newUsed.audioSource.priority = 100;
            newUsed.audioSource.Play();  // 오디오 소스 재생
            _UsedClip.Add(newUsed);  // 재생 중인 클립 목록에 추가
        }

        public void PlayClipAtPoint(int index, Vector3 position, float volume = 1f)
        {
            AudioSource source = GetAvailableSource();
            source.transform.position = position; // 위치 설정
            source.spatialBlend = 1f; // 3D 사운드
            source.clip = _AudioClip[index];
            source.volume = volume;
            source.Play();

            _UsedClip.Add(new AudioClipInfo
            {
                audioSource = source,
                audioClip = _AudioClip[index],
                playPosition = position
            });
        }

        // 인덱스로 클립 재생
        public void PlayClipByIndex(int clipIndex, float speed)
        {
            if (clipIndex >= 0 && clipIndex < _AudioClip.Length)
            {
                Debug.LogError("");
                PlayClip(_AudioClip[clipIndex], speed);
            }
            else
            {
                Debug.LogWarning("잘못된 클립 인덱스입니다.");
            }
        }


        public void StopClipByIndex(int clipIndex)
        {
            // 유효한 clipIndex인지 확인
            if (clipIndex < 0 || clipIndex >= _AudioClip.Length)
            {
                Debug.LogWarning("잘못된 clipIndex입니다. 범위를 확인하세요.");
                return;
            }

            for (int i = _UsedClip.Count - 1; i >= 0; i--)
            {
                AudioClipInfo clipInfo = _UsedClip[i];

                // AudioClip 또는 AudioSource가 null인 경우 방어 처리
                if (clipInfo.audioClip == null || clipInfo.audioSource == null)
                {
                    Debug.LogWarning("AudioClip 또는 AudioSource가 null입니다.");
                    continue;
                }

                // 조건: 해당 AudioClip이 재생 중인지 확인
                if (clipInfo.audioClip == _AudioClip[clipIndex] &&
                    (clipInfo.audioSource.isPlaying || clipInfo.audioSource.clip != null))
                {
                    clipInfo.audioSource.Stop();           // 재생 중지
                    clipInfo.audioSource.clip = null;     // 클립 초기화
                    ReturnSourceToQueue(clipInfo.audioSource); // 풀에 반환
                    _UsedClip.RemoveAt(i);                // _UsedClip에서 제거
                }
            }
        }
    }
}