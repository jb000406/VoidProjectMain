using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VoidProject
{
    public class HideMirrorTrigger : MonoBehaviour
    {
        #region Variables
        public Transform playerCamera;  //플레이어 카메라
        public Transform targetMirror;  //거울
        public GameObject targetObejctParent;  //대상 부모 오브젝트

        [SerializeField] public float activationAngle = 30f; //각도 범위

        public int targetChild;
        #endregion

        private void Start()
        {
/*            //메쉬 렌더러 비활성화
            targetRenderer.enabled = false;

            //이벤트 함수 등록
            targetInteractable.selectEntered.AddListener(OnGrab);*/
        }

        private void OnTriggerStay(Collider other)
        {
            //플레이어 확인
            if (other.CompareTag("Player"))
            {
                Debug.Log("활성화");
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
            }
        }


        //활성화
        private void TriggerActivated()
        {
            Debug.Log("바라보는 중");
            targetObejctParent.SetActive(true);
        }
/*
        //비활성화
        private void TriggerDeactivated()
        {
            Debug.Log("비활성화");

            targetObejctParent.SetActive(false);
        }*/

        //From 카메라 To 타겟 기즈모
        private void OnDrawGizmos()
        {
            if (playerCamera == null || targetMirror == null) return;

            // 디버그용: 플레이어 카메라에서 A로의 방향 표시
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerCamera.position, targetMirror.position);
        }
/*
        //잡으면 타겟 메쉬렌더러 활성화
        private void OnGrab(SelectEnterEventArgs args)
        {
            for (int i = 0; i < targetChild; i++)
            {
                targetObejctParent.gameObject.SetActive(true);
            }
            Destroy(this.gameObject);
        }*/
    }
}