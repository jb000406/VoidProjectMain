using UnityEngine;

namespace VoidProject
{
    public class ClearStageDelete : MonoBehaviour
    {
        #region Variables
        public GameObject levelDelete;

        public GameObject door;
        public GameObject wall;

        #endregion

        private void OnTriggerEnter(Collider other)
        {
             if(other.CompareTag("Player"))
            {
                if (door != null)
                {
                    door.SetActive(false);
                }

                wall.SetActive(true);

                Destroy(levelDelete);
            }
        }
    }
}
