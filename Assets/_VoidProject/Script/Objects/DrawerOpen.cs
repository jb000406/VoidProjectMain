using UnityEngine;

namespace HJ
{
    public class DrawerOpen : MonoBehaviour
    {
        #region Variables

        #endregion

        #region Variabels
        [SerializeField] private Transform handle; // 손잡이 Transform    [SerializeField]
        [SerializeField] private Transform lockHandlePosition; // 손잡이 Transform

        [SerializeField]
        private float openRange = 2.0f; // 슬라이드 가능한 범위 (문이 열리는 거리)
        private Vector3 initialDrawerPosition; // 문의 초기 위치
        private Vector3 initialHandlePosition; // 손잡이의 초기 위치
        private Vector3 lastHandleDirection; // 손잡이의 마지막 방향
        private bool isGrabbing = false; // 그랩 상태 확인

        //[SerializeField] private bool isLeft = true;
        #endregion

        private void Start()
        {
            // 문과 손잡이의 초기 위치 저장
            initialDrawerPosition = transform.localPosition;
            initialHandlePosition = handle.localPosition;
        }

        void Update()
        {
            if (isGrabbing)
            {
                OpneDrawer();
                LockHandlePosition(); // 핸들 위치 고정
            }
        }

        private void OpneDrawer()
        {
            // 핸들의 z축 이동 차이를 계산
            float delta = handle.position.z - lastHandleDirection.z;

            // 문 슬라이드 위치 업데이트
            Vector3 currentDoorPosition = transform.localPosition;
            currentDoorPosition.z -= delta;
            currentDoorPosition.z = Mathf.Clamp(currentDoorPosition.z, initialDrawerPosition.z, initialDrawerPosition.z + openRange);

            // 문 이동
            transform.localPosition = currentDoorPosition;

            // 손잡이 방향 업데이트
            lastHandleDirection = handle.position;
        }


        /// <summary>
        /// 손잡이 위치를 문과 고정
        /// </summary>
        private void LockHandlePosition()
        {
            // 손잡이 위치를 문과 함께 고정
            handle.position = lockHandlePosition.position;
        }

        /// <summary>
        /// 손잡이를 잡을 때 호출
        /// </summary>
        public void StartGrabbing()
        {
            isGrabbing = true;

            // 손잡이 방향 초기화
            lastHandleDirection = handle.localPosition;
        }

        /// <summary>
        /// 손잡이를 놓을 때 호출
        /// </summary>
        public void StopGrabbing()
        {
            isGrabbing = false;

            // 손잡이를 초기 위치로 복구
            ResetHandlePosition();
        }

        /// <summary>
        /// 손잡이를 초기 위치로 복구
        /// </summary>
        private void ResetHandlePosition()
        {
            handle.localPosition = initialHandlePosition;
        }
    }
}