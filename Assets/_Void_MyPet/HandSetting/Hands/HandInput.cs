using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VoidProject
{
    public class HandInput : MonoBehaviour
    {
        public bool isLeftHand = false;
        private InputActionProperty currentPinchActionProperty;
        private InputActionProperty currentGrabActionProperty;
        private Animator handAnimator;
        private bool isPinching = false;
        private bool isFist = false;

        private void Start()
        {
            handAnimator = GetComponent<Animator>();
            currentPinchActionProperty = isLeftHand ? GameManager.LeftPinchAction : GameManager.RightPinchAction;
            currentGrabActionProperty = isLeftHand ? GameManager.LeftGrabAction : GameManager.RightGrabAction;
        }

        void Update()
        {
            // Trigger 값 확인 및 업데이트
            isPinching = HandTrackingManager.Instance.IsPinching(isLeftHand);
            isFist = HandTrackingManager.Instance.IsFist(isLeftHand);
            if (handAnimator != null)
            {
                handAnimator.SetFloat("Trigger", isPinching ? 1 : 0);
                handAnimator.SetFloat("Grip", isFist ? 1 : 0);
            }

            if (isPinching)
            {
                if (!currentPinchActionProperty.action.enabled)
                {
                    currentPinchActionProperty.action.Enable();
                }
            }
            else
            {
                if (currentPinchActionProperty.action.enabled)
                {
                    currentPinchActionProperty.action.Disable();
                }
            }

            if (isFist)
            {
                if (!currentGrabActionProperty.action.enabled)
                {
                    currentGrabActionProperty.action.Enable();
                }
            }
            else
            {
                if (currentGrabActionProperty.action.enabled)
                {
                    currentGrabActionProperty.action.Disable();
                }
            }
        }
    }
}