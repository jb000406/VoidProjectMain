using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using VoidProject;

namespace VoidProject
{
    public class LokcerSocket : PuzzleSocket
    {
        #region Variables
        [SerializeField] private string triggerName = "OpenTrigger";
        [SerializeField] private float animTerm = 2f;
        [SerializeField] private float destroyTime = 2f;

        public DoorOpenController door;

        private XRGrabInteractable grabInteractable1;
        #endregion

        protected override void OnTriggerEnter(Collider other)
        {
            Insert(other);

            StartCoroutine(LockerOpen(other));
        }

        protected override void Insert(Collider other)
        {
            if(other.transform.CompareTag(itemName))
            {
                Transform key = other.transform;
                grabInteractable1 = key.GetComponent<XRGrabInteractable>();

                grabInteractable1.selectEntered.AddListener(KeyHold);
                grabInteractable1.selectExited.AddListener(KeyRelease);

                //키 위치, 방향
                key.position = transform.position;
                key.rotation = transform.rotation;

                Rigidbody keyRb = key.GetComponent<Rigidbody>();

                if (keyRb != null)
                {
                    keyRb.isKinematic = true;
                }
            }
        }

        //자물쇠 열기
        IEnumerator LockerOpen(Collider other)
        {
            if(isRelease)
            {
                yield return new WaitForSeconds(animTerm);

                //사운드 재생
                SoundManager.Instance.PlayClipAtPoint(16, transform.position);

                //애니메이션 재생
                Animator keyAnimator = other.transform.GetComponent<Animator>();
                if (keyAnimator != null)
                {
                    keyAnimator.enabled = true;
                    keyAnimator.SetTrigger(triggerName);
                }

                //kinematic 해제
                yield return new WaitForSeconds(1.2f);

                GetComponent<Collider>().enabled = false;
                other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = false;

                //제거
                Destroy(other.gameObject, destroyTime);
                Destroy(transform.parent.gameObject, destroyTime);

                yield return new WaitForSeconds(animTerm);

                //문 열기
                if (door)
                {
                    door.DoorOpen();
                }
            }
        }
    }
}