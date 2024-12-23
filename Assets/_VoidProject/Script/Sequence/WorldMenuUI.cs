using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace VoidProject
{
    public class WorldMenuUI : MonoBehaviour
    {
        public GameObject worldMenuUI;
        public GameObject SquenceLogoUI;
        public TextMeshProUGUI textbox;
        public Image sequenceImg;
        private Transform head;
        private float distance;
        [SerializeField] private float offset = 1.0f;

        protected virtual void Start()
        {
            head = Camera.main.transform;
        }
        protected virtual void Update()
        {
            distance = PlayerCasting.distanceFromTarget;
        }

        protected void ShowMenuUI(string sequenceText = "")
        {
            worldMenuUI.SetActive(true);

            //show 설정
            /*            distance = (distance < offset) ? distance - 0.05f : offset; //distance벽과의 거리가 1.5이하면 0.1빼고 더크면 1.5f

                        worldMenuUI.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
                        worldMenuUI.transform.LookAt(new Vector3(head.position.x, worldMenuUI.transform.position.y, head.position.z));
                        worldMenuUI.transform.forward *= -1;*/

            // UI를 카메라 정면에 고정
            worldMenuUI.transform.position = head.position + head.forward * 0.5f;

            // UI가 항상 카메라를 향하도록 설정
            worldMenuUI.transform.rotation = Quaternion.LookRotation(head.forward);

            //text 설정
            if (textbox)
            {
                textbox.text = sequenceText;
            }
        }

        protected void HideMenuUI()
        {
            worldMenuUI.SetActive(false);
            textbox.text = "";
        }

        //쇼이미지
        protected void ShowMenuImg(Image img)
        {
            SquenceLogoUI.SetActive(true);

            //show 설정
            distance = (distance < offset) ? distance - 0.05f : offset; //distance벽과의 거리가 1.5이하면 0.1빼고 더크면 1.5f

            SquenceLogoUI.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
            SquenceLogoUI.transform.LookAt(new Vector3(head.position.x, SquenceLogoUI.transform.position.y, head.position.z));
            SquenceLogoUI.transform.forward *= -1;

            //img 설정
            if (sequenceImg)
            {
                sequenceImg = img;
            }
        }
        //하이드이미지
        protected void HideMenuImg()
        {
            SquenceLogoUI.SetActive(false);
            sequenceImg.enabled = false;
        }
    }

}