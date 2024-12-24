using UnityEngine;

namespace VoidProject
{
    public class DropObjectSfx : MonoBehaviour
    {
        #region Variables
        private Transform player;

        [SerializeField] private float velocity = 1f;       //충돌 속도
        [SerializeField] private float maxDistance = 15f;
        [SerializeField] private int soundIndex = -1;       //사운드 인덱스
        [SerializeField] private float maxSoundVolume = 1f; //재생시 사운드 볼륨
        #endregion

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > velocity && !collision.transform.CompareTag("Player"))
            {
                Debug.Log("떨어짐");
                float distance = Vector3.Distance(player.position, transform.position);

                // 거리 범위에 따른 볼륨 계산
                float volume;
                if (distance <= maxDistance)
                    volume = maxSoundVolume; // 최대 볼륨
                else
                    volume = 0.0f; // 소리 안 들림


                SoundManager.Instance.PlayClipAtPoint(soundIndex, transform.position, volume);
            }
        }
    }
}