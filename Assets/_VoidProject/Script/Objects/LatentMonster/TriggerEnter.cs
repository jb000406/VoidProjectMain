using UnityEngine;

namespace VoidProject
{
    public class TriggerEnter : MonoBehaviour
    {
        #region variables
        private Collider m_collider;

        public LatentMonsterChest latentMonsterChest;

        [SerializeField] private string thePlayerTag = "Player";
        #endregion

        private void Start()
        {
            m_collider = GetComponent<Collider>();
            m_collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag(thePlayerTag))
            {
                Debug.Log("충돌");

                StartCoroutine(latentMonsterChest.PlayAnim());
            }
        }
    }
}