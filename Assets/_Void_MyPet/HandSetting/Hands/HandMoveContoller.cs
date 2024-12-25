using UnityEngine;

namespace VoidProject
{
    public class HandMoveController : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float rotationSpeed = 0.5f;
        [SerializeField] private float smoothingFactor = 1f;
        [SerializeField] private float deadZoneThreshold = 0.5f;
        [SerializeField] private float rotationThreshold = 0.5f;
        [SerializeField] private float acceleration = 0.5f;
        [SerializeField] private float gravity = 9.8f; // 중력 값
        [SerializeField] private LayerMask groundLayer; // Ground 체크용 레이어
        [SerializeField] private float groundCheckDistance = 0.1f; // Ground 체크 거리

        private Vector3 previousDirection = Vector3.zero;
        private Vector3 previousRotationDirection = Vector3.zero;
        private float currentSpeed = 0f;

        private Vector3 velocity; // 현재 속도
        private bool isGrounded; // 지면 여부


        private void Update()
        {
            if (HandTrackingManager.Instance.IsWalk(out Pose leftHand, out Pose rightHand) && leftHand != null)
            {
                HandleMovement(leftHand, rightHand);
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * acceleration);
            }
            Vector3 rawDirection = transform.rotation * leftHand.forward;
            rawDirection.y = 0;

            HandleGravity();
            //HandleRotation(leftHand, rightHand, rawDirection);
        }

        private void HandleGravity()
        {
            // 캐릭터가 지면에 있는지 확인
            isGrounded = characterController.isGrounded;

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // 지면에 닿았을 때 속도를 초기화 (작은 값 유지)
            }

            // 중력 적용 (공중에 있을 때)
            if (!isGrounded)
            {
                velocity.y -= gravity * Time.deltaTime;
            }

            // Move 함수에 중력 포함
            characterController.Move(velocity * Time.deltaTime);
        }

        private void HandleMovement(Pose leftHand, Pose rightHand)
        {
            Vector3 rawDirection = transform.rotation * leftHand.forward;
            rawDirection.y = 0;

            if (rawDirection.sqrMagnitude < deadZoneThreshold * deadZoneThreshold)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * acceleration);
                return;
            }

            Vector3 smoothedDirection = Vector3.Lerp(previousDirection, rawDirection.normalized, smoothingFactor);
            previousDirection = smoothedDirection;

            float handDistance = Mathf.Abs(leftHand.position.x - rightHand.position.x);

            if (handDistance > 0.2f)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed * 3f, Time.deltaTime * acceleration);
            }
            else if (handDistance > 0.1f && handDistance <= 0.2f)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed * 2f, Time.deltaTime * acceleration);
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * acceleration);
            }

            // 중력을 포함한 이동
            Vector3 movement = smoothedDirection * currentSpeed * Time.deltaTime;
            characterController.Move(movement + velocity * Time.deltaTime);
        }


        private void HandleRotation(Pose leftHand, Pose rightHand, Vector3 rawDirection)
        {
            Vector3 smoothedDirection = Vector3.Lerp(previousDirection, rawDirection.normalized, smoothingFactor);
            previousDirection = smoothedDirection;

            if (previousDirection.sqrMagnitude > rotationThreshold * rotationThreshold)
            {
                Vector3 rotationDirection = Vector3.Lerp(previousRotationDirection, previousDirection.normalized, smoothingFactor);
                previousRotationDirection = rotationDirection;
                Quaternion currentRotation = transform.rotation;
                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}