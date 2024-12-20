using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VoidProject
{
    public class MatchController : MonoBehaviour
    {
        #region Variables
        public GameObject matches;

        private XRGrabInteractable interactable;
        private Animator animator;
        private Match[] match;

        [SerializeField] private string playAnimString = "IsHold";
        [SerializeField] private bool isGrabbed = false;
        #endregion

        private void Start()
        {
            animator = GetComponent<Animator>();
            interactable = GetComponent<XRGrabInteractable>();

            //이벤트 함수 등록
            interactable.selectEntered.AddListener(OnGrab);
            interactable.selectExited.AddListener(OnRelease);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Match collidedMatch = collision.transform.GetComponent<Match>();
            if (collidedMatch != null && collision.relativeVelocity.magnitude >= collidedMatch.ignitionVelocity)
            {
                // 점화
                StartCoroutine(collidedMatch.Ignition());
            }
        }

        private void OnGrab(SelectEnterEventArgs args)
        {
            if (isGrabbed)
            {
                //이미 잡혀있으면 못쥐게함
                args.interactorObject.transform.GetComponent<XRBaseInteractor>().EndManualInteraction();
            }
            else
            {
                isGrabbed = true;
                OpenMatchBox();
            }
        }

        private void OnRelease(SelectExitEventArgs args)
        {
            CloseMatchBox();
            isGrabbed = false;
        }

        private void OpenMatchBox()
        {
            Debug.Log("잡음");
            animator.SetBool(playAnimString , true);
            matches.SetActive(true);
        }

        private void CloseMatchBox()
        {
            Debug.Log("놓음");
            animator.SetBool(playAnimString, false);
            matches.SetActive(false);
        }
    }
}