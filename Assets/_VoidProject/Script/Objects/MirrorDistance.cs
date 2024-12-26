using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

namespace VoidProject
{
    public class MirrorDistance : MonoBehaviour
    {
        #region Variables
        public GameObject mirrorLight;

        [SerializeField] private float playerDistance = 1f;
        #endregion

        private void Start()
        {
            mirrorLight.SetActive(false);
        }

        private void Update()
        {
            if(IsNearPlayer())
            {
                mirrorLight.SetActive(true);
            }
            else
            {
                mirrorLight.SetActive(false);
            }
        }

        private bool IsNearPlayer()
        {
            float distance = Vector3.Distance(GameManager.Player_Transform.position, transform.position);

            return (distance <= playerDistance);
        }
    }
}