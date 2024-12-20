using UnityEngine;

namespace VoidProject
{
    public class BornFire : MonoBehaviour
    {
        #region Variables
        [SerializeField] private string triggerObject = "Torch";
        private Torch torch;
        #endregion

        private void Start()
        {
            gameObject.GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            torch = other.GetComponent<Torch>();

            if (torch == null)
                return;

            if (other.gameObject.name == triggerObject && torch.isActive == false)
            {
                UpdateState(other);
            }
        }

        private void UpdateState(Collider other)
        {
            Debug.Log("충돌");
            other.transform.GetChild(0).gameObject.SetActive(true);
            torch.isActive = true;
        }
    }
}