using UnityEngine;

namespace VoidProject
{

    public class BreakObjFall : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameObject breakObJ;

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Instantiate(breakObJ, transform.position, Quaternion.identity, transform);   
            }
        }
    }
}