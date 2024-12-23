using UnityEngine;
using UnityEngine.InputSystem;

namespace VoidProject
{
    /// <summary>
    /// 컨트롤러 손 모델 애니메이션 구현
    /// </summary>
    public class AnimateHandOnInput : MonoBehaviour
    {
        #region Variables
        private Animator handAnimator;

        //인풋 입력값 처리
        public InputActionProperty pinchAnimationAction;
        public InputActionProperty gripAnimationAction;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            //참조
            handAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            float triggerValue = pinchAnimationAction.action.ReadValue<float>();
            float gripValue = gripAnimationAction.action.ReadValue<float>();
            //Debug.Log($"triggerValue: {triggerValue}");

            handAnimator.SetFloat("Trigger", triggerValue);
            handAnimator.SetFloat("Grip", gripValue);
        }
    }
}