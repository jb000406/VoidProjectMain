using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace HJ
{
    public class MirrorTrigger : MonoBehaviour
    {
        #region Variables
        public Transform playerCamera;  //플레이어 카메라
        public Transform targetMirror;  //거울
        public Transform targetObejct;  //대상 오브젝트

        private MeshRenderer targetRenderer;
        private XRGrabInteractable targetInteractable;

        [SerializeField] public float activationAngle = 30f; //각도 범위

        #endregion

        private void Start()
        {
            targetRenderer = targetObejct.GetComponent<MeshRenderer>();
            targetInteractable = targetObejct.GetComponent<XRGrabInteractable>();

            //메쉬 렌더러 비활성화
            targetRenderer.enabled = false;

            //이벤트 함수 등록
            targetInteractable.selectEntered.AddListener(OnGrab);
        }

        private void OnTriggerStay(Collider other)
        {
            //플레이어 확인
            if (other.CompareTag("Player"))
            {
                //플레이어 방향
                Vector3 playerForward = playerCamera.forward;

                //타겟과의 방향 계산
                Vector3 directionToA = (targetMirror.position - playerCamera.position).normalized;

                //각도 비교
                float angle = Vector3.Angle(playerForward, directionToA);

                if (angle <= activationAngle)
                {
                    //트리거 작동
                    TriggerActivated();
                }
                else
                {
                    TriggerDeactivated();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                TriggerDeactivated();
            }
        }

        //활성화
        private void TriggerActivated()
        {
            Debug.Log("바라보는 중");

            targetRenderer.enabled = true;
        }

        //비활성화
        private void TriggerDeactivated()
        {
            Debug.Log("비활성화");

            targetRenderer.enabled = false;
        }

        //From 카메라 To 타겟 기즈모
        private void OnDrawGizmos()
        {
            if (playerCamera == null || targetMirror == null) return;

            // 디버그용: 플레이어 카메라에서 A로의 방향 표시
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerCamera.position, targetMirror.position);
        }

        //잡으면 타겟 메쉬렌더러 활성화
        private void OnGrab(SelectEnterEventArgs args)
        {
            targetRenderer.enabled = true;
            Destroy(this.gameObject);
        }
    }
}