using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace VoidProject
{
    public class BreakableObject : MonoBehaviour
    {
        #region Variables
        public GameObject crackedPrefab;                    //깨지는 프리팹
        public Transform player_Transform;

        //private List<Rigidbody> crackedRigidbodies = new List<Rigidbody>();

        [SerializeField] private float velocity = 1f;       //충돌 속도
        [SerializeField] private float maxDistance = 15f;
        //[SerializeField] private float maxForce = 100f;       //조각 퍼지는 힘
        #endregion

        private void Start()
        {
            //foreach (Rigidbody rb in crackedPrefab.GetComponentsInChildren<Rigidbody>())
            //{
            //    crackedRigidbodies.Add(rb);
            //}
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > velocity && !collision.transform.CompareTag("Player"))
            {
                Debug.Log("Cracked");
                float distance = Vector3.Distance(player_Transform.position, transform.position);

                // 거리 범위에 따른 볼륨 계산
                float volume;
                if (distance <= maxDistance)
                    volume =  1.0f; // 최대 볼륨
                else
                    volume  = 0.0f; // 소리 안 들림
               

                SoundManager.Instance.PlayClipAtPoint(12, transform.position, volume);

                this.gameObject.SetActive(false);

                if (crackedPrefab != null)
                {
                    //깨진 항아리 생성
                    GameObject cracked = Instantiate(crackedPrefab, this.transform.position, this.transform.rotation);

                    // 자식 오브젝트에 스크립트를 추가
                    foreach (Transform child in cracked.transform)
                    {
                        // 특정 조건이 없다면 모든 자식에 스크립트 추가
                        child.gameObject.AddComponent<XRGrabInteractable>();
                    }

                    ////조각 중력 주기
                    //foreach (Rigidbody rb in crackedRigidbodies)
                    //{
                    //    rb.isKinematic = false;

                    //    float randomForce= Random.Range(0f, maxForce);

                    //    rb.AddForce(Vector3.left * randomForce, ForceMode.Impulse);
                    //}
                }

                Destroy(this.gameObject);
            }
        }
    }
}