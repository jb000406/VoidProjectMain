using UnityEngine;

namespace VoidProject
{
    public class StartUI : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        //플레이씬
        public Canvas startCanvas;
        public Transform playerCamera;
        private Transform head;
        [SerializeField] private float distance = 0.5f;
        #endregion

        void Start()
        {
            head = Camera.main.transform;
            //if (playerCamera != null && startCanvas != null)
            //{
            //    startCanvas.transform.position = playerCamera.position + playerCamera.forward * 0.5f;
            //    startCanvas.transform.rotation = Quaternion.LookRotation(playerCamera.forward);
            //}

            /* if (startCanvas.renderMode == RenderMode.WorldSpace)
             {
                 // 카메라 앞 0.5 유닛 뒤에 위치 설정
                 startCanvas.transform.position = playerCamera.position + playerCamera.forward * 0.2f;

                 // StartCanvas가 항상 카메라를 향하도록 설정
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
        public void StartButton()
        {
            fader.FadeTo(1);
        }
        public void QuitButton()
        {
            Debug.Log("Quit Game");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID && !UNITY_EDITOR
   
    
#else
    // PC 또는 기타 플랫폼
    Application.Quit();
#endif
        }
    }

}