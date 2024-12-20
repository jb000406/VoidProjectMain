using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

namespace VoidProject
{
    public class InGameMenu : MonoBehaviour
    {
        #region Variables
        [Header("Game Menu")]
        public GameObject gameMenu;
        public InputActionProperty showButton;
        public SceneFader fader;

        private Transform head;
        [SerializeField] private float distance = 1.5f;
        #endregion

        private void Start()
        {
            head = Camera.main.transform;
        }

        private void Update()
        {
            distance = PlayerCasting.distanceFromTarget;

            if (showButton.action.WasPressedThisFrame())
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            gameMenu.SetActive(!gameMenu.activeSelf);

            //show 설정
            if (gameMenu.activeSelf)
            {
                distance = (distance < 1.5f) ? distance - 0.05f : 1.5f; //distance벽과의 거리가 1.5이하면 0.1빼고 더크면 1.5f
                distance = (distance < 1.5f) ? Mathf.Max(distance - 0.05f, 0.5f) : 1.5f;
                gameMenu.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
                gameMenu.transform.LookAt(new Vector3(head.position.x, gameMenu.transform.position.y, head.position.z));
                gameMenu.transform.forward *= -1;
            }
        }

        //Menu 버튼
        public void Menu()
        {
            fader.FadeTo(0);
        }
    }
}