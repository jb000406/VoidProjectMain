using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace VoidProject
{
    public class GameUIManager : MonoBehaviour
    {
        #region Variables
        public SceneFader fader; // 씬 페이드 관리
        public Canvas startUI;  // 시작 화면
        public Canvas gameOverUI; // 게임오버 화면
        public GameObject inGameMenu; // 인게임 메뉴

        public InputActionProperty showMenuButton; // 메뉴 토글 버튼
        private Transform head; //플레이어 카메라
        private Vector3 playerStartPos; // 플레이어 초기 위치
        private Quaternion playerStartRot; // 플레이어 초기 회전
        [SerializeField] private float distance = 1f;

        public PlayerController player; // 플레이어 컨트롤러
        public GameObject Locomotion; //플레이어 무브
        private bool isGameMenuActive = false; // 인게임 메뉴 활성화 상태
        [SerializeField] private float menuDistance = 1f; // 메뉴 표시 거리
        #endregion

        private void Start()
        {
            head = Camera.main.transform;

            // UI 초기화
            ShowStartUI();
            Locomotion.SetActive(false);
        }

        private void Update()
        {
            // 메뉴 토글 버튼 처리
            if (showMenuButton.action.WasPressedThisFrame())
            {
                ToggleInGameMenu();
            }

            // 게임 시작 화면 UI 위치 업데이트
            if (startUI.gameObject.activeInHierarchy)
            {
                startUI.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
                startUI.transform.LookAt(new Vector3(head.position.x, startUI.transform.position.y, head.position.z));
                startUI.transform.forward *= -1;
            }

            // 게임 오버 화면 UI 위치 업데이트
            if (gameOverUI.gameObject.activeInHierarchy)
            {
                gameOverUI.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
                gameOverUI.transform.LookAt(new Vector3(head.position.x, gameOverUI.transform.position.y, head.position.z));
                gameOverUI.transform.forward *= -1;
            }

        }

        public void ShowStartUI()
        {
            // 시작 화면 UI 활성화
            StartCoroutine(ShowStartUICoroutine());
        }

        private IEnumerator ShowStartUICoroutine()
        {
            // 페이드 인
            fader.FromFade(1f);
            yield return new WaitForSeconds(1f); // 페이드 시간 대기
            startUI.gameObject.SetActive(true);
            gameOverUI.gameObject.SetActive(false);
            inGameMenu.SetActive(false);
            Locomotion.SetActive(false);
        }

        public void StartGame()
        {
            // 코루틴으로 UI 페이드 후 시작 화면 비활성화
            StartCoroutine(StartGameCoroutine());
        }

        private IEnumerator StartGameCoroutine()
        {
            yield return new WaitForSeconds(1f);
            startUI.gameObject.SetActive(false);
            Locomotion.SetActive(true);
        }

        public void GameOver()
        {
            // 게임오버 화면 UI 활성화
            StartCoroutine(GameOverCoroutine());
        }

        private IEnumerator GameOverCoroutine()
        {
            fader.FromFade(1f); // 페이드 아웃
            yield return new WaitForSeconds(1f);
            gameOverUI.gameObject.SetActive(true);
            inGameMenu.SetActive(false);
            startUI.gameObject.SetActive(false);
            Locomotion.SetActive(false);
        }

        public void Retry()
        {
            // 플레이어 상태 초기화 및 시작 위치로 이동
            StartCoroutine(RetryCoroutine());
        }

        private IEnumerator RetryCoroutine()
        {
            fader.FadeTo(1f);
            yield return new WaitForSeconds(1f);
            gameOverUI.gameObject.SetActive(false);
            LoadCurrentScene();
        }

        public void Menu()
        {
            // 시작 화면으로 복귀
            ShowStartUI();
        }

        public void ToggleInGameMenu()
        {
            fader.FromFade(1f); // 페이드 인
            isGameMenuActive = !isGameMenuActive;
            inGameMenu.SetActive(isGameMenuActive);
            if (isGameMenuActive)
            {
                UpdateMenuPosition();
            }
        }

        private void UpdateMenuPosition()
        {
            // 메뉴 위치 및 회전 업데이트
            float distance = PlayerCasting.distanceFromTarget;
            distance = Mathf.Clamp(distance - 0.05f, 0.5f, menuDistance); // 최소 0.5f, 최대 메뉴 거리
            inGameMenu.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
            inGameMenu.transform.LookAt(new Vector3(head.position.x, inGameMenu.transform.position.y, head.position.z));
            inGameMenu.transform.forward *= -1; // 뒤집어서 표시
        }

        public void QuitGame()
        {
            Debug.Log("Quit");
#if UNITY_EDITOR
            // 에디터에서 실행 중이라면 플레이 모드 종료
            UnityEditor.EditorApplication.isPlaying = false;
#else
                // 빌드된 게임에서는 애플리케이션 종료
                Application.Quit();
#endif
        }
        private void LoadCurrentScene()
        {
            // 현재 활성화된 씬 이름 가져오기
            string currentSceneName = SceneManager.GetActiveScene().name;

            // 씬을 다시 로드
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
