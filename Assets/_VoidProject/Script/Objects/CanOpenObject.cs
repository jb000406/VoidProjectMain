using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using VoidProject;

namespace HJ
{
    public class CanOpenObject : MonoBehaviour
    {
        #region Variables
        private Rigidbody rb;
        private XRGrabInteractable grabInteractable;
        private Transform grabbingHand;

        [SerializeField] private float soundVolume = 1f;
        [SerializeField] private float releaseDistance = 1.5f; //Grab 해제 거리

        [SerializeField] private float minRoatationDelta = 20f;
        private Quaternion lastRotation; //이전 프레임 회전
        #endregion

        private void Start()
        {
            //참조
            rb = GetComponent<Rigidbody>();
            grabInteractable = GetComponent<XRGrabInteractable>();

            //rigidbody 설정
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            //grabInteractable 설정
            grabInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
            grabInteractable.throwOnDetach = false;

            //이벤트 함수 등록
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);

            //초기 회전 값 저장
            lastRotation = transform.rotation;
        }

        private void Update()
        {
            if (grabbingHand != null)
            {
                //그랩 상태에서의 거리 계산
                float distance = Vector3.Distance(grabbingHand.position, transform.position);

                //거리 초과 시 그랩 해제
                if (distance > releaseDistance)
                {
                    grabInteractable.interactionManager.SelectExit(grabbingHand.GetComponent<IXRSelectInteractor>(), grabInteractable);
                }
            }

            //회전 변화량
            float rotationDelta = Quaternion.Angle(transform.rotation, lastRotation);

            //움직임 감지
            bool isMoving = rotationDelta >= minRoatationDelta;

            if (isMoving && grabbingHand != null)
            {
                SoundManager.Instance.PlayClipAtPoint(14,transform.position, soundVolume);

                //마지막 회전값 저장
                lastRotation = transform.rotation;
            }
        }

        public void OnGrabbed(SelectEnterEventArgs args)
        {
            Debug.Log("잡기");

            //그랩한 손 저장
            grabbingHand = args.interactorObject.transform;

            //키네메틱 해제
            rb.isKinematic = false;
        }

        public void OnReleased(SelectExitEventArgs args)
        {
            Debug.Log("놓기");

            //그랩한 손 해제
            grabbingHand = null;

            //키네메틱 설정
            rb.isKinematic = true;
        }
    }
}