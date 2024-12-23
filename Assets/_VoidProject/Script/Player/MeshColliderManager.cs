using UnityEngine;

namespace VoidProject
{
    public class MeshColliderManager : MonoBehaviour
    {
        [Header("MeshCollider Settings")]
        [SerializeField] private string targetTag = "ColiderTarget"; // 충돌 대상 태그
        [SerializeField] private float activationDistance = 5f; // 활성화 거리
        private MeshCollider playerMeshCollider;
        private Transform targetTransform; // 감지된 태그 객체의 Transform

        private void Start()
        {
            // 플레이어의 MeshCollider 가져오기
            playerMeshCollider = GetComponent<MeshCollider>();
            if (playerMeshCollider == null)
            {
                Debug.LogWarning("MeshCollider component is missing.");
            }
            else
            {
                playerMeshCollider.enabled = false; // 초기 비활성화
            }
        }

        private void Update()
        {
            CheckTagAndDistance(); // 태그와 거리 조건 확인
        }

        private void CheckTagAndDistance()
        {
            // 태그를 가진 모든 객체 탐색
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

            foreach (GameObject obj in taggedObjects)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);

                // 활성화 조건: 태그 일치 + 거리 안에 있음
                if (distance <= activationDistance)
                {
                    if (!playerMeshCollider.enabled)
                    {
                        playerMeshCollider.enabled = true;
                        Debug.Log($"MeshCollider enabled: Close to {targetTag} object.");
                    }
                    targetTransform = obj.transform;
                    return; // 조건 충족 시 다른 객체는 검사하지 않음
                }
            }

            // 조건을 만족하는 객체가 없으면 비활성화
            if (playerMeshCollider.enabled)
            {
                playerMeshCollider.enabled = false;
                Debug.Log("MeshCollider disabled: No nearby tagged object.");
            }
        }
    }
}