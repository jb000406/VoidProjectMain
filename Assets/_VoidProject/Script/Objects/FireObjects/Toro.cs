using UnityEngine;

namespace VoidProject
{
    public class Toro : MonoBehaviour
    {
        #region Variables
        [SerializeField] private string triggerObject = "Torch";
        private Torch torch;
        public bool isActive = false;

        [SerializeField] private GameObject fakeWall;
        #endregion

        private void Start()
        {
            gameObject.GetComponent<SphereCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            torch = other.GetComponent<Torch>();

            if (torch == null)
                return;

            if (other.gameObject.name == triggerObject && torch.isActive == true)
            {
                UpdateState();
            }
        }

        private void UpdateState()
        {
            Debug.Log("충돌");
            transform.GetChild(0).gameObject.SetActive(true);
            isActive = true;
            if (fakeWall != null)
            {
                fakeWall.SetActive(false);
            }
        }
    }
}