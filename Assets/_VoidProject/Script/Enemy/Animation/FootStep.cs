using UnityEngine;

namespace VoidProject
{

    public class FootStep : MonoBehaviour
    {
        [Header("사운드 설정")]
        [SerializeField] private int soundClipIndex; // SoundManager의 사운드 클립 인덱스
        [SerializeField] private float maxDistance = 15f; // 최대 거리

        // Animation Event에서 호출할 함수
        public void PlayFootstepSound()
        {
            if (SoundManager.Instance != null && GameManager.Player_Transform != null)
            {
                // 발소리 발생 위치 (현재 오브젝트 위치)
                Vector3 soundPosition = transform.position;

                // 플레이어와의 거리 계산
                float distance = Vector3.Distance(GameManager.Player_Transform.position, soundPosition);

                // 거리 기반 볼륨 계산
                float volume = CalculateVolume(distance);

                // 발소리 재생
                SoundManager.Instance.PlayClipAtPoint(soundClipIndex, soundPosition, volume);
            }
        }

        private float CalculateVolume(float distance)
        {
            if (distance <= maxDistance)
                return 1.0f; // 최대 볼륨
            else
                return 0.0f; // 소리 안 들림
        }

    }
}