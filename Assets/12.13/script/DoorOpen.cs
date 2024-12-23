using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VoidProject
{
    public class DoorOpen : MonoBehaviour
    {
        #region Variables
        private Rigidbody rb;
        private XRGrabInteractable grabInteractable;
        private Transform grabbingHand;
        private AudioSource audioSource;
        private HingeJoint joint;
        public CapsuleCollider playerCollider;

        public Transform closedPosition;

        [SerializeField] private float releaseDistance = 1.5f; //Grab 해제 거리
        [SerializeField] private float lerpSpeed = 5f; //문 닫히는 속도

        [SerializeField] private float playerRadius = 0.2f;
        private float initialRadius;

        private bool isClosing = false;

        [SerializeField] private float minRoatationDelta = 30f;
        private Quaternion lastRotation; //이전 프레임 문 회전
        #endregion

        private void Start()
        {
            //참조
            rb = GetComponent<Rigidbody>();
            grabInteractable = GetComponent<XRGrabInteractable>();
            audioSource = GetComponent<AudioSource>();
            joint = GetComponent<HingeJoint>();
            playerCollider = GetComponent<CapsuleCollider>();

            //
            initialRadius = playerCollider.radius;

            //Rigidbody 설정
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            //GrabInteractable 설정
            grabInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
            grabInteractable.throwOnDetach = false;

            //이벤트 함수 등록
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);

            //초기 회전 값 저장
            lastRotation = transform.rotation;

            //사운드 초기 설정
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }

        private void Update()
        {
            //그랩 상태에서의 거리 계산
            if (grabbingHand != null)
            {
                float distance = Vector3.Distance(grabbingHand.position, transform.position);

                //거리 초과 시 그랩 해제
                if (distance > releaseDistance || joint.angle >= joint.limits.max || joint.angle <= joint.limits.min)
                {
                    grabInteractable.interactionManager.SelectExit(grabbingHand.GetComponent<IXRSelectInteractor>(), grabInteractable);
                    grabbingHand = null;
                }
            }


            //문 회전 변화량
            float rotationDelta = Quaternion.Angle(transform.rotation, lastRotation);

            //움직임 감지
            bool isMoving = rotationDelta >= minRoatationDelta;

            //문닫기
            if (isClosing)
            {
                //위치 변경
                transform.position = Vector3.Lerp(transform.position, closedPosition.position, Time.deltaTime * lerpSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, closedPosition.rotation, Time.deltaTime * lerpSpeed);
                transform.localScale = Vector3.Lerp(transform.localScale, closedPosition.localScale, Time.deltaTime * lerpSpeed);

                //키네마틱 해제
                rb.isKinematic = true;

                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
            else if (isMoving && grabInteractable != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }

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

            //닫히는 상태 종료
            isClosing = false;

            //
            if(playerCollider != null)
            {
                Debug.Log(playerRadius);
                playerCollider.radius = playerRadius;
            }
        }

        public void OnReleased(SelectExitEventArgs args)
        {
            Debug.Log("놓기");

            //그랩한 손 해제
            grabbingHand = null;

            //문 닫히기 시작
            isClosing = true;

            //
            if (playerCollider != null)
            {
                Debug.Log(initialRadius);
                playerCollider.radius = initialRadius;
            }
        }
    }
}