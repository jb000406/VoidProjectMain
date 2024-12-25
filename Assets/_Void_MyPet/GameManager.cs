using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

namespace VoidProject
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static Transform Player_Transform { get; private set; }
        public static Transform LeftController { get; private set; }
        public static Transform RightController { get; private set; }
        public static InputActionProperty LeftGrabAction { get; private set; }
        public static InputActionProperty RightGrabAction { get; private set; }
        public static InputActionProperty LeftPinchAction { get; private set; }
        public static InputActionProperty RightPinchAction { get; private set; }
        public static InputActionProperty MenuAction { get; private set; }
        public static Canvas Canvas_ScreenCamera { get; private set; }

        public static List<Vector3> SequencePointList;
        public static List<string> SequenceText;

        [SerializeField] private Transform player_transform;
        [SerializeField] private Transform leftController;
        [SerializeField] private Transform rightController;
        [SerializeField] private InputActionProperty leftGrabAction;
        [SerializeField] private InputActionProperty rightGrabAction;
        [SerializeField] private InputActionProperty leftPinchAction;
        [SerializeField] private InputActionProperty rightPinchAction;
        [SerializeField] private InputActionProperty menuAction;
        [SerializeField] private Image fadeImage;
        [SerializeField] private Canvas canvas_ScreenCamera;

        [SerializeField] private List<Vector3> sequencePointList;
        [TextArea(2, 5)][SerializeField] private List<string> sequenceText;

        public static bool IsInteractiveWithPet { get; set; } = false;
        public static bool IsGmaePlaying { get; set; } = false;
        public static bool IsGmaePause { get; set; } = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            InitializeProperty();
        }

        private void CheckPropertySetting()
        {
            bool isAllOk = true;
            if (player_transform == null )
            {
                isAllOk = false;
                Debug.LogError("플레이어 설정이 안되어 있습니다.");
            }
            if (leftController == null || rightController == null)
            {
                isAllOk = false;
                Debug.LogError("Controller 설정이 안되어 있습니다.");
            }
            if (leftGrabAction == null || rightGrabAction == null || leftPinchAction == null || rightPinchAction == null || menuAction == null)
            {
                isAllOk = false;
                Debug.LogError("인풋 바인딩 설정이 안되어 있습니다.");
            }
            if ( sequencePointList.Count != sequenceText.Count )
            {
                Debug.LogError("시퀀스 설정이 안되어 있습니다.");
            }

            if (isAllOk == false) Application.Quit();
        }

        private void InitializeProperty()
        {
            CheckPropertySetting();
            Player_Transform = player_transform;
            LeftController = leftController;
            RightController = rightController;
            LeftGrabAction = leftGrabAction;
            RightGrabAction = rightGrabAction;
            LeftPinchAction = leftPinchAction;
            RightPinchAction = rightPinchAction;
            MenuAction = menuAction;
            SequencePointList = sequencePointList;
            SequenceText = sequenceText;
            Canvas_ScreenCamera = canvas_ScreenCamera;

            //fadeImage.color = new Color(0, 0, 0, 1);
        }

        public static void ToggleCanvas(bool onOff)
        {
            Player_Transform.GetChild(1).gameObject.SetActive(!onOff);
            Instance.canvas_ScreenCamera.gameObject.SetActive(onOff);
        }

        public static void DisplayToolTipText(string text)
        {
            if (!Instance.canvas_ScreenCamera.gameObject.activeSelf) Instance.canvas_ScreenCamera.gameObject.SetActive(true);
            TextMeshProUGUI diplayText = Instance.canvas_ScreenCamera.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            diplayText.text = text;
        }

        void Start()
        {
            //StartCoroutine(FadeCoroutine(false, 2f)); // 2초 동안 페이드 아웃
        }

        private IEnumerator FadeCoroutine(bool isAction, float duration)
        {
            float startAlpha = isAction ? 0 : 1;
            float targetAlpha = isAction ? 1 : 0;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

                Color color = fadeImage.color;
                color.a = alpha;
                fadeImage.color = color;

                yield return null;
            }
        }
    }
}