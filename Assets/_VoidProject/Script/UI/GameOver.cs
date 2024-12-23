using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoidProject
{
    public class GameOver : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        //플레이씬
        public Canvas startCanvas;
        public Transform playerCamera;
        private Transform head;
        [SerializeField] private float distance = 1f;
        #endregion

        void Start ()
        {
            head = Camera.main.transform;
            /*if (startCanvas.renderMode == RenderMode.WorldSpace)
            {
                // 카메라보다 Z축으로 멀리 이동
                startCanvas.transform.position = playerCamera.transform.position + playerCamera.transform.forward * 0.1f;
            }*/
            /*            if (playerCamera != null && startCanvas != null)
                        {
                            startCanvas.transform.position = playerCamera.position + playerCamera.forward * 0.5f;

                            startCanvas.transform.rotation = Quaternion.LookRotation(playerCamera.forward);
                        }*/
            if (fader == null)
            {
                Debug.LogError("SceneFader reference is missing. Please assign it in the inspector.");
                return;
            }
            if (!fader.gameObject.activeInHierarchy)
            {
                fader.gameObject.SetActive(true);  // SceneFader를 활성화
            }
            //페이드인 효과
            fader.FromFade(2f);
        }
        private void Update()
        {
            startCanvas.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
            startCanvas.transform.LookAt(new Vector3(head.position.x, startCanvas.transform.position.y, head.position.z));
            startCanvas.transform.forward *= -1;
        }
        public void Retry()
        {
           fader.FadeTo(1);
        }
        public void Menu()
        {
            fader.FadeTo(0);
        }
    }

}
