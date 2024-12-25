using UnityEngine;
using UnityEngine.UIElements;

namespace VoidProject
{

    public class LookPlayer : MonoBehaviour
    {
        private Transform player_transform;

        [Header("시야 설정")]
        [SerializeField] private float detectionRange = 10f; // 감지 범위
        [SerializeField] private float fieldOfView = 60f;    // 시야각 (각도)

        // Update is called once per frame
        void Update()
        {
            if(player_transform != null)
            {
                Quaternion.LookRotation(player_transform.position);

                // 플레이어와 몬스터 간의 방향 계산
                Vector3 directionToPlayer = (player_transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToPlayer);

                // 시야각 검사
                if (angle < fieldOfView / 2)
                {
                    if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, detectionRange))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            SoundManager.Instance.PlayClipAtPoint(23, transform.position);
                            return;
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                player_transform = other.transform;
            }
        }
    }
}