using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace VoidProject
{
    public class Chess : MonoBehaviour
    {
        #region Variables
        public XRGrabInteractable interactable;

        public int chessIndex = -1;
        public Vector3 initialPosition;
        public bool isGrabbed = false;
        #endregion

        private void Start()
        {
            //처음 위치값 저장
            initialPosition = transform.position;

            interactable = GetComponent<XRGrabInteractable>();

            //이벤트함수 등록
            interactable.selectEntered.AddListener(OnGrabbed);
            interactable.selectExited.AddListener(OnReleased);

            interactable.attachTransform = transform.GetChild(0);
        }

        public void OnGrabbed(SelectEnterEventArgs args)
        {
            isGrabbed = true;
        }

        public void OnReleased(SelectExitEventArgs args)
        {
            isGrabbed = false;
        }
    }
}