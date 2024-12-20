using UnityEngine;
using UnityEngine.Events;

namespace VoidProject
{
    public class TriggerController : MonoBehaviour
    {
        #region Variables
        public GameObject[] eventObject;

        public UnityEvent allTriggerObjTrue;
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Debug.Log($"{gameObject} 감지!");

                int randomNumber = Random.Range(0, eventObject.Length);

                eventObject[randomNumber].SetActive(true);

                allTriggerObjTrue.Invoke();

                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                for(int i = 0; i < eventObject.Length; i++)
                {
                    if(eventObject[i].activeInHierarchy)
                    {
                        eventObject[i].SetActive(false);
                    }
                }

                gameObject.SetActive(false);
            }
        }


    }
}