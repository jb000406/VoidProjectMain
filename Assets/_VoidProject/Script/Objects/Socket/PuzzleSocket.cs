using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace VoidProject
{
    public class PuzzleSocket : MonoBehaviour
    {
        #region Variables
        [SerializeField] protected string itemName = "item";

        [SerializeField] protected bool isRelease = false;

        [SerializeField] private int soundIndex = -1;

        private XRGrabInteractable grabInteractable;

        [SerializeField] private GameObject doorParent;
        [SerializeField] private GameObject[] doorChild;

        [SerializeField] private GameObject fakeDoorParent;
        [SerializeField] private GameObject[] fakeDoorChild;
        #endregion

        private void Start()
        {
            Collider collider= GetComponent<Collider>();
            collider.isTrigger = true;

            if (doorParent != null && fakeDoorParent != null)
            {
                int doorCount = doorParent.transform.childCount;
                int fakeDoorCount = fakeDoorParent.transform.childCount;

                doorChild = new GameObject[doorCount];
                fakeDoorChild = new GameObject[fakeDoorCount];

                for (int i = 0; i < doorCount; i++)
                {
                    doorChild[i] = doorParent.transform.GetChild(i).gameObject;
                    fakeDoorChild[i] = fakeDoorParent.transform.GetChild(i).gameObject;
                }
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            Debug.Log("충돌");
            Insert(other);
        }

        protected virtual void Insert(Collider other)
        {
            if(other.gameObject.name == itemName)
            {
                Transform key = other.transform;
                grabInteractable = key.GetComponent<XRGrabInteractable>();

                grabInteractable.selectEntered.AddListener(KeyHold);
                grabInteractable.selectExited.AddListener(KeyRelease);

                //퍼즐 아이템 위치, 방향
                other.gameObject.transform.position = transform.position;
                other.gameObject.transform.rotation = transform.rotation;

                Rigidbody itemrb = other.GetComponent<Rigidbody>();

                if (itemrb != null)
                {
                    itemrb.isKinematic = true;
                }

                
            }          
        }

        public void KeyHold(SelectEnterEventArgs args)
        {
            Debug.Log("KeyHold");
            isRelease = false;

            SoundManager.Instance.PlayClipAtPoint(soundIndex, transform.position);
        }

        public void KeyRelease(SelectExitEventArgs args)
        {
            //
            Debug.Log("KeyRelease");
            isRelease = true;

            for(int i = 0; i < doorChild.Length; i++)
            {
                doorChild[i].gameObject.SetActive(true);
                fakeDoorChild[i].gameObject.SetActive(false);    
            }
        }
    }
}